using System;
using System.Linq;
using System.Threading.Tasks;
using dl.wm.suite.cms.contracts.Employees.EmployeeRoles;
using dl.wm.suite.cms.model.Employees.EmployeeRoles;
using dl.wm.suite.cms.repository.ContractRepositories;
using dl.wm.suite.common.dtos.Vms.Employees.EmployeeRoles;
using dl.wm.suite.common.infrastructure.Exceptions.Domain.EmployeeRoles;
using dl.wm.suite.common.infrastructure.TypeMappings;
using dl.wm.suite.common.infrastructure.UnitOfWorks;
using Serilog;

namespace dl.wm.suite.cms.services.Employees.EmployeeRoles
{
    public class CreateEmployeeRoleProcessor : ICreateEmployeeRoleProcessor
    {
        private readonly IUnitOfWork _uOf;
        private readonly IEmployeeRoleRepository _employeeRoleRepository;
        private readonly IAutoMapper _autoMapper;

        public CreateEmployeeRoleProcessor(IUnitOfWork uOf, IAutoMapper autoMapper,
            IEmployeeRoleRepository employeeRoleRepository)
        {
            _uOf = uOf;
            _employeeRoleRepository = employeeRoleRepository;
            _autoMapper = autoMapper;
        }

        public Task<EmployeeRoleUiModel> CreateEmployeeRoleAsync(Guid accountIdToCreateThisEmployeeRole, EmployeeRoleForCreationUiModel newEmployeeRoleUiModel)
        {
            var response =
                new EmployeeRoleUiModel()
                {
                    Message = "START_CREATION"
                };

            if (newEmployeeRoleUiModel == null)
            {
                response.Message = "ERROR_INVALID_EMPLOYEE_ROLE_MODEL";
                return Task.Run(() => response);
            }

            var employeeRoleToBeCreated = new EmployeeRole();

            try
            {
                employeeRoleToBeCreated.InjectWithInitialAttributes(newEmployeeRoleUiModel.EmployeeRoleName, newEmployeeRoleUiModel.EmployeeRoleNotes);
                employeeRoleToBeCreated.InjectWithAudit(accountIdToCreateThisEmployeeRole);

                ThrowExcIfEmployeeRoleCannotBeCreated(employeeRoleToBeCreated);
                ThrowExcIfThisEmployeeRoleAlreadyExist(employeeRoleToBeCreated);
                
                Log.Debug(
                    $"Create EmployeeRole: {newEmployeeRoleUiModel.EmployeeRoleName}" +
                    "--CreateEmployeeRole--  @NotComplete@ [CreateEmployeeRoleProcessor]. " +
                    "Message: Just Before MakeItPersistence");

                MakeEmployeeRolePersistent(employeeRoleToBeCreated);

                Log.Debug(
                    $"Create EmployeeRole: {newEmployeeRoleUiModel.EmployeeRoleName}" +
                    "--CreateEmployeeRole--  @NotComplete@ [CreateEmployeeRoleProcessor]. " +
                    "Message: Just After MakeItPersistence");
                response = ThrowExcIfEmployeeRoleWasNotBeMadePersistent(employeeRoleToBeCreated);
                response.Message = "SUCCESS_CREATION";
            }
            catch (InvalidEmployeeRoleException e)
            {
                response.Message = "ERROR_INVALID_EMPLOYEE_ROLE_MODEL";
                Log.Error(
                    $"Create EmployeeRole: {newEmployeeRoleUiModel.EmployeeRoleName}" +
                    "--CreateEmployeeRole--  @NotComplete@ [CreateEmployeeRoleProcessor]. " +
                    $"Broken rules: {e.BrokenRules}");
            }
            catch (EmployeeRoleAlreadyExistsException ex)
            {
                response.Message = "ERROR_EMPLOYEE_ROLE_ALREADY_EXISTS";
                Log.Error(
                    $"Create EmployeeRole: {newEmployeeRoleUiModel.EmployeeRoleName}" +
                    "--CreateEmployeeRole--  @fail@ [CreateEmployeeRoleProcessor]. " +
                    $"@innerfault:{ex?.Message} and {ex?.InnerException}");
            }
            catch (EmployeeRoleDoesNotExistAfterMadePersistentException exx)
            {
                response.Message = "ERROR_EMPLOYEE_ROLE_NOT_MADE_PERSISTENT";
                Log.Error(
                    $"Create EmployeeRole: {newEmployeeRoleUiModel.EmployeeRoleName}" +
                    "--CreateEmployeeRole--  @fail@ [CreateEmployeeRoleProcessor]." +
                    $" @innerfault:{exx?.Message} and {exx?.InnerException}");
            }
            catch (Exception exxx)
            {
                response.Message = "UNKNOWN_ERROR";
                Log.Error(
                    $"Create EmployeeRole: {newEmployeeRoleUiModel.EmployeeRoleName}" +
                    $"--CreateEmployeeRole--  @fail@ [CreateEmployeeRoleProcessor]. " +
                    $"@innerfault:{exxx.Message} and {exxx.InnerException}");
            }

            return Task.Run(() => response);
        }

        private void ThrowExcIfThisEmployeeRoleAlreadyExist(EmployeeRole employeeRoleToBeCreated)
        {
            var employeeRoleRetrieved = _employeeRoleRepository.FindOneByName(employeeRoleToBeCreated.Name);
            if (employeeRoleRetrieved != null)
            {
                throw new EmployeeRoleAlreadyExistsException(employeeRoleToBeCreated.Name,
                    employeeRoleToBeCreated.GetBrokenRulesAsString());
            }
        }

        private EmployeeRoleUiModel ThrowExcIfEmployeeRoleWasNotBeMadePersistent(EmployeeRole employeeRoleToBeCreated)
        {
            var retrievedEmployeeRole = _employeeRoleRepository.FindOneByName(employeeRoleToBeCreated.Name);
            if (retrievedEmployeeRole != null)
                return _autoMapper.Map<EmployeeRoleUiModel>(retrievedEmployeeRole);
            throw new EmployeeRoleDoesNotExistAfterMadePersistentException(employeeRoleToBeCreated.Name);
        }

        private void ThrowExcIfEmployeeRoleCannotBeCreated(EmployeeRole employeeRoleToBeCreated)
        {
            bool canBeCreated = !employeeRoleToBeCreated.GetBrokenRules().Any();
            if (!canBeCreated)
                throw new InvalidEmployeeRoleException(employeeRoleToBeCreated.GetBrokenRulesAsString());
        }

        private void MakeEmployeeRolePersistent(EmployeeRole employeeRoleToBeMadePersistence)
        {
            _employeeRoleRepository.Save(employeeRoleToBeMadePersistence);
            _uOf.Commit();
        }
    }
}
