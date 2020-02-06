using System;
using System.Linq;
using System.Threading.Tasks;
using dl.wm.suite.cms.contracts.Employees.Departments;
using dl.wm.suite.cms.model.Employees.Departments;
using dl.wm.suite.cms.repository.ContractRepositories;
using dl.wm.suite.common.dtos.Vms.Employees.Departments;
using dl.wm.suite.common.infrastructure.Exceptions.Domain.Departments;
using dl.wm.suite.common.infrastructure.TypeMappings;
using dl.wm.suite.common.infrastructure.UnitOfWorks;
using Serilog;

namespace dl.wm.suite.cms.services.Employees.Departments
{
    public class CreateDepartmentProcessor : ICreateDepartmentProcessor
    {
        private readonly IUnitOfWork _uOf;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IAutoMapper _autoMapper;

        public CreateDepartmentProcessor(IUnitOfWork uOf, IAutoMapper autoMapper,
            IDepartmentRepository departmentRepository)
        {
            _uOf = uOf;
            _departmentRepository = departmentRepository;
            _autoMapper = autoMapper;
        }

        public Task<DepartmentUiModel> CreateDepartmentAsync(Guid accountIdToCreateThisDepartment, DepartmentForCreationUiModel newDepartmentUiModel)
        {
            var response =
                new DepartmentUiModel()
                {
                    Message = "START_CREATION"
                };

            if (newDepartmentUiModel == null)
            {
                response.Message = "ERROR_INVALID_DEPARTMENT_MODEL";
                return Task.Run(() => response);
            }

            var departmentToBeCreated = new Department();

            try
            {
                departmentToBeCreated.InjectWithInitialAttributes(newDepartmentUiModel.DepartmentName, newDepartmentUiModel.DepartmentNotes);
                departmentToBeCreated.InjectWithAudit(accountIdToCreateThisDepartment);

                ThrowExcIfDepartmentCannotBeCreated(departmentToBeCreated);
                ThrowExcIfThisDepartmentAlreadyExist(departmentToBeCreated);

                Log.Debug(
                    $"Create Department: {newDepartmentUiModel.DepartmentName}" +
                    "--CreateDepartment--  @NotComplete@ [CreateDepartmentProcessor]. " +
                    "Message: Just Before MakeItPersistence");

                MakeDepartmentPersistent(departmentToBeCreated);

                Log.Debug(
                    $"Create Department: {newDepartmentUiModel.DepartmentName}" +
                    "--CreateDepartment--  @NotComplete@ [CreateDepartmentProcessor]. " +
                    "Message: Just After MakeItPersistence");
                response = ThrowExcIfDepartmentWasNotBeMadePersistent(departmentToBeCreated);
                response.Message = "SUCCESS_CREATION";
            }
            catch (InvalidDepartmentException e)
            {
                response.Message = "ERROR_INVALID_DEPARTMENT_MODEL";
                Log.Error(
                    $"Create Department: {newDepartmentUiModel.DepartmentName}" +
                    "--CreateDepartment--  @NotComplete@ [CreateDepartmentProcessor]. " +
                    $"Broken rules: {e.BrokenRules}");
            }
            catch (DepartmentAlreadyExistsException ex)
            {
                response.Message = "ERROR_DEPARTMENT_ALREADY_EXISTS";
                Log.Error(
                    $"Create Department: {newDepartmentUiModel.DepartmentName}" +
                    "--CreateDepartment--  @fail@ [CreateDepartmentProcessor]. " +
                    $"@innerfault:{ex?.Message} and {ex?.InnerException}");
            }
            catch (DepartmentDoesNotExistAfterMadePersistentException exx)
            {
                response.Message = "ERROR_DEPARTMENT_NOT_MADE_PERSISTENT";
                Log.Error(
                    $"Create Department: {newDepartmentUiModel.DepartmentName}" +
                    "--CreateDepartment--  @fail@ [CreateDepartmentProcessor]." +
                    $" @innerfault:{exx?.Message} and {exx?.InnerException}");
            }
            catch (Exception exxx)
            {
                response.Message = "UNKNOWN_ERROR";
                Log.Error(
                    $"Create Department: {newDepartmentUiModel.DepartmentName}" +
                    $"--CreateDepartment--  @fail@ [CreateDepartmentProcessor]. " +
                    $"@innerfault:{exxx.Message} and {exxx.InnerException}");
            }

            return Task.Run(() => response);
        }

        private void ThrowExcIfThisDepartmentAlreadyExist(Department departmentToBeCreated)
        {
            var customerRetrieved = _departmentRepository.FindOneByName(departmentToBeCreated.Name);
            if (customerRetrieved != null)
            {
                throw new DepartmentAlreadyExistsException(departmentToBeCreated.Name,
                    departmentToBeCreated.GetBrokenRulesAsString());
            }
        }

        private DepartmentUiModel ThrowExcIfDepartmentWasNotBeMadePersistent(Department departmentToBeCreated)
        {
            var retrievedDepartment = _departmentRepository.FindOneByName(departmentToBeCreated.Name);
            if (retrievedDepartment != null)
                return _autoMapper.Map<DepartmentUiModel>(retrievedDepartment);
            throw new DepartmentDoesNotExistAfterMadePersistentException(departmentToBeCreated.Name);
        }

        private void ThrowExcIfDepartmentCannotBeCreated(Department departmentToBeCreated)
        {
            bool canBeCreated = !departmentToBeCreated.GetBrokenRules().Any();
            if (!canBeCreated)
                throw new InvalidDepartmentException(departmentToBeCreated.GetBrokenRulesAsString());
        }

        private void MakeDepartmentPersistent(Department departmentToBeMadePersistence)
        {
            _departmentRepository.Save(departmentToBeMadePersistence);
            _uOf.Commit();
        }
    }
}
