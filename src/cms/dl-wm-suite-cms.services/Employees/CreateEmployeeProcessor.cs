using System;
using System.Linq;
using System.Threading.Tasks;
using dl.wm.suite.cms.contracts.Employees;
using dl.wm.suite.cms.model.Employees;
using dl.wm.suite.cms.repository.ContractRepositories;
using dl.wm.suite.common.dtos.Vms.Employees;
using dl.wm.suite.common.infrastructure.Exceptions.Domain.Persons;
using dl.wm.suite.common.infrastructure.TypeMappings;
using dl.wm.suite.common.infrastructure.UnitOfWorks;
using Serilog;

namespace dl.wm.suite.cms.services.Employees
{
    public class CreateEmployeeProcessor : ICreateEmployeeProcessor
    {
        private readonly IUnitOfWork _uOf;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IAutoMapper _autoMapper;

        public CreateEmployeeProcessor(IUnitOfWork uOf, IAutoMapper autoMapper, IEmployeeRepository employeeRepository)
        {
            _uOf = uOf;
            _employeeRepository = employeeRepository;
            _autoMapper = autoMapper;
        }

        public Task<EmployeeUiModel> CreateEmployeeAsync(EmployeeForCreationUiModel newEmployeeUiModel)
        {
            var response =
                new EmployeeUiModel()
                {
                    Message = "START_CREATION"
                };

            if (newEmployeeUiModel == null)
            {
                response.Message = "ERROR_INVALID_PERSON_MODEL";
                return Task.Run(() => response);
            }

            var personToBeCreated = new Employee();

            try
            {
                personToBeCreated.InjectWithInitialAttributes(newEmployeeUiModel.PersonEmail);

                ThrowExcIfPersonCannotBeCreated(personToBeCreated);
                ThrowExcIfThisPersonAlreadyExist(personToBeCreated);

                //var tenantToBeInjected = new Tenant()
                //{
                //    LastTimeLoggedIn = personToBeCreated.CreatedDate,
                //    Login = newEmployeeUiModel.PersonLogin,
                //};

                //personToBeCreated.InjectWithTenant(tenantToBeInjected);

                Log.Debug(
                    $"Create Person: {newEmployeeUiModel.PersonEmail}" +
                    "--CreatePerson--  @NotComplete@ [CreateEmployeeProcessor]. " +
                    "Message: Just Before MakeItPersistense");

                MakePersonPersistent(personToBeCreated);

                Log.Debug(
                    $"Create Person: {newEmployeeUiModel.PersonEmail}" +
                    "--CreatePerson--  @NotComplete@ [CreateEmployeeProcessor]. " +
                    "Message: Just After MakeItPersistense");
                response = ThrowExcIfPersonWasNotBeMadePersistent(personToBeCreated);
                response.Message = "SUCCESS_CREATION";
            }
            catch (InvalidPersonException e)
            {
                response.Message = "ERROR_INVALID_PERSON_MODEL";
                Log.Error(
                    $"Create Person: {newEmployeeUiModel.PersonEmail}" +
                    "--CreatePerson--  @NotComplete@ [CreateEmployeeProcessor]. " +
                    $"Broken rules: {e.BrokenRules}");
            }
            catch (PersonAlreadyExistsException ex)
            {
                response.Message = "ERRR_PERSON_ALREADY_EXISTS";
                Log.Error(
                    $"Create Person: {newEmployeeUiModel.PersonEmail}" +
                    "--CreatePerson--  @fail@ [CreateEmployeeProcessor]. " +
                    $"@innerfault:{ex?.Message} and {ex?.InnerException}");
            }
            catch (PersonDoesNotExistAfterMadePersistentException exx)
            {
                response.Message = "ERROR_PERSON_NOT_MADE_PERSISTENT";
                Log.Error(
                    $"Create Person: {newEmployeeUiModel.PersonEmail}" +
                    "--CreatePerson--  @fail@ [CreateEmployeeProcessor]." +
                    $" @innerfault:{exx?.Message} and {exx?.InnerException}");
            }
            catch (Exception exxx)
            {
                response.Message = "UNKNOWN_ERROR";
                Log.Error(
                    $"Create Person: {newEmployeeUiModel.PersonEmail}" +
                    $"--CreatePerson--  @fail@ [CreateEmployeeProcessor]. " +
                    $"@innerfault:{exxx.Message} and {exxx.InnerException}");
            }

            return Task.Run(() => response);
        }

        private void ThrowExcIfThisPersonAlreadyExist(Employee personToBeCreated)
        {
            var customerRetrieved = _employeeRepository.FindEmployeeByEmail(personToBeCreated.Email);
            if (customerRetrieved != null)
            {
                throw new PersonAlreadyExistsException(personToBeCreated.Email,
                    personToBeCreated.GetBrokenRulesAsString());
            }
        }

        private EmployeeUiModel ThrowExcIfPersonWasNotBeMadePersistent(Employee personToBeCreated)
        {
            var retreievedPerson = _employeeRepository.FindEmployeeByEmail(personToBeCreated.Email);
            if (retreievedPerson  != null)
                return _autoMapper.Map<EmployeeUiModel>(retreievedPerson);
            throw new PersonDoesNotExistAfterMadePersistentException(personToBeCreated.Email);
        }

        private void ThrowExcIfPersonCannotBeCreated(Employee personToBeCreated)
        {
            bool canBeCreated = !personToBeCreated.GetBrokenRules().Any();
            if (!canBeCreated)
                throw new InvalidPersonException(personToBeCreated.GetBrokenRulesAsString());
        }

        private void MakePersonPersistent(Employee personToBeMadePersistence)
        {
            _employeeRepository.Save(personToBeMadePersistence);
            _uOf.Commit();
        }
    }
}
