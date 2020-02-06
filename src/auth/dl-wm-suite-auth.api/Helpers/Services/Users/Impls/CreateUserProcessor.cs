using System;
using System.Linq;
using System.Threading.Tasks;
using dl.wm.suite.auth.api.Helpers.Models;
using dl.wm.suite.auth.api.Helpers.Repositories.Departments;
using dl.wm.suite.auth.api.Helpers.Repositories.EmployeeRoles;
using dl.wm.suite.auth.api.Helpers.Repositories.Roles;
using dl.wm.suite.auth.api.Helpers.Repositories.Users;
using dl.wm.suite.auth.api.Helpers.Services.Users.Contracts;
using dl.wm.suite.common.dtos.Vms.Accounts;
using dl.wm.suite.common.dtos.Vms.Users;
using dl.wm.suite.common.infrastructure.Exceptions.Domain.Departments;
using dl.wm.suite.common.infrastructure.Exceptions.Domain.EmployeeRoles;
using dl.wm.suite.common.infrastructure.Exceptions.Domain.Roles;
using dl.wm.suite.common.infrastructure.Exceptions.Domain.Users;
using dl.wm.suite.common.infrastructure.TypeMappings;
using dl.wm.suite.common.infrastructure.UnitOfWorks;
using Serilog;

namespace dl.wm.suite.auth.api.Helpers.Services.Users.Impls
{
    public class CreateUserProcessor : ICreateUserProcessor
    {
        private readonly IUnitOfWork _uOf;
        private readonly IRoleRepository _roleRepository;
        private readonly IUserRepository _userRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IEmployeeRoleRepository _employeeRoleRepository;
        private readonly IAutoMapper _autoMapper;

        public CreateUserProcessor(IUnitOfWork uOf, IAutoMapper autoMapper, 
            IUserRepository userRepository,  
            IDepartmentRepository departmentRepository,  
            IEmployeeRoleRepository employeeRoleRepository,  
            IRoleRepository roleRepository)
        {
            _uOf = uOf;
            _userRepository = userRepository;
            _departmentRepository = departmentRepository;
            _employeeRoleRepository = employeeRoleRepository;
            _roleRepository = roleRepository;
            _autoMapper = autoMapper;
        }

        public Task<UserUiModel> CreateUserAsync(Guid accountIdToCreateThisUser, UserForRegistrationUiModel newUserForRegistration)
        {
            var response =
                new UserUiModel()
                {
                    Message = "START_CREATION"
                };

            if (newUserForRegistration == null)
            {
                response.Message = "ERROR_INVALID_USER_MODEL";
                return Task.Run(() => response);
            }

            try
            {
                var userToBeCreated = new User();
                userToBeCreated.InjectWithInitialAttributes(newUserForRegistration);
                userToBeCreated.InjectWithAuditCreation(accountIdToCreateThisUser);

                var roleToBeInjected = _roleRepository.FindBy(newUserForRegistration.UserRoleId);

                if (roleToBeInjected == null)
                    throw new RoleDoesNotExistException(newUserForRegistration.UserRoleId);

                var userRoleToBeInjected = new UserRole();

                userRoleToBeInjected.InjectWithRole(roleToBeInjected);
                userRoleToBeInjected.InjectWithAuditCreation(accountIdToCreateThisUser);
                var personToBeInjected = new Person()
                {
                    Firstname = newUserForRegistration.Firstname,
                    Lastname = newUserForRegistration.Lastname,
                    Gender = (GenderType) Enum.Parse(typeof(GenderType), newUserForRegistration.Gender, true),
                    Phone = newUserForRegistration.Phone,
                    ExtPhone = newUserForRegistration.ExtPhone,
                    Mobile = newUserForRegistration.Mobile,
                    ExtMobile = newUserForRegistration.ExtMobile,
                    CreatedBy = accountIdToCreateThisUser,
                    ModifiedBy = Guid.Empty,
                    Notes = newUserForRegistration.Notes,
                    Address = new Address()
                    {
                        StreetOne = newUserForRegistration.AddressStreetOne,
                        StreetTwo = newUserForRegistration.AddressStreetTwo,
                        PostCode = newUserForRegistration.AddressPostCode,
                        City = newUserForRegistration.AddressCity,
                        Region = newUserForRegistration.AddressRegion,
                    },
                    Email = newUserForRegistration.Login,
                };

                var employeeRoleToBeInjected = _employeeRoleRepository.FindBy(newUserForRegistration.EmployeeRoleId);

                if (employeeRoleToBeInjected == null)
                    throw new EmployeeRoleDoesNotExistException(newUserForRegistration.EmployeeRoleId);

                var departmentToBeInjected = _departmentRepository.FindBy(newUserForRegistration.DepartmentId);

                if (departmentToBeInjected == null)
                    throw new DepartmentDoesNotExistException(newUserForRegistration.DepartmentId);

                userToBeCreated.InjectWithPerson(personToBeInjected);
                userToBeCreated.InjectWithUserRole(userRoleToBeInjected);
                personToBeInjected.InjectWithEmployeeRole(employeeRoleToBeInjected);
                personToBeInjected.InjectWithDepartment(departmentToBeInjected);

                ThrowExcIfUserCannotBeCreated(userToBeCreated);
                ThrowExcIfThisUserAlreadyExist(userToBeCreated);

                Log.Debug(
                    $"Create User: {newUserForRegistration.Login}" +
                    "--CreateUser--  @NotComplete@ [CreateUserProcessor]. " +
                    "Message: Just Before MakeItPersistence");

                MakeUserPersistent(userToBeCreated);

                Log.Debug(
                    $"Create User: {newUserForRegistration.Login}" +
                    "--CreateUser--  @NotComplete@ [CreateUserProcessor]. " +
                    "Message: Just After MakeItPersistence");
                response = ThrowExcIfUserWasNotBeMadePersistent(userToBeCreated);
                response.Message = "SUCCESS_CREATION";
            }
            catch (RoleDoesNotExistException r)
            {
                response.Message = "ERROR_INVALID_ROLE_NAME";
                Log.Error(
                    $"Create User: {newUserForRegistration.Login}" +
                    $"Error Message:{response.Message}" +
                    "--CreateUser--  @NotComplete@ [CreateUserProcessor]");
            }
            catch (InvalidUserException e)
            {
                response.Message = "ERROR_INVALID_USER_MODEL";
                Log.Error(
                    $"Create User: {newUserForRegistration.Login}" +
                    $"Error Message:{response.Message}" +
                    "--CreateUser--  @NotComplete@ [CreateUserProcessor]. " +
                    $"Broken rules: {e.BrokenRules}");
            }
            catch (UserEmailOrLoginAlreadyExistsException ex)
            {
                response.Message = "ERROR_USER_ALREADY_EXISTS";
                Log.Error(
                    $"Create User: {newUserForRegistration.Login}" +
                    $"Error Message:{response.Message}" +
                    "--CreateUser--  @fail@ [CreateUserProcessor]. " +
                    $"@inner-fault:{ex?.Message} and {ex?.InnerException}");
            }
            catch (UserDoesNotExistAfterMadePersistentException exx)
            {
                response.Message = "ERROR_USER_NOT_MADE_PERSISTENT";
                Log.Error(
                    $"Create User: {newUserForRegistration.Login}" +
                    $"Error Message:{response.Message}" +
                    "--CreateUser--  @fail@ [CreateUserProcessor]." +
                    $" @inner-fault:{exx?.Message} and {exx?.InnerException}");
            }
            catch (Exception exxx)
            {
                response.Message = "UNKNOWN_ERROR";
                Log.Error(
                    $"Create User: {newUserForRegistration.Login}" +
                    $"Error Message:{response.Message}" +
                    $"--CreateUser--  @fail@ [CreateUserProcessor]. " +
                    $"@inner-fault:{exxx.Message} and {exxx.InnerException}");
            }

            return Task.Run(() => response);
        }



        private void ThrowExcIfThisUserAlreadyExist(User userToBeCreated)
        {
            var userRetrieved = _userRepository.FindUserByLoginAndEmail(userToBeCreated.Login, userToBeCreated.Person?.Email);
            if (userRetrieved != null)
            {
                throw new UserEmailOrLoginAlreadyExistsException(userToBeCreated.Login, userToBeCreated.Person?.Email,
                    userToBeCreated.GetBrokenRulesAsString());
            }
        }

        private UserUiModel ThrowExcIfUserWasNotBeMadePersistent(User userToBeCreated)
        {
            var retreievedUser = _userRepository.FindUserByLoginAndEmail(userToBeCreated.Login, userToBeCreated.Person?.Email);
            if (retreievedUser  != null)
                return _autoMapper.Map<UserUiModel>(retreievedUser);
            throw new UserDoesNotExistAfterMadePersistentException(userToBeCreated.Login, userToBeCreated.Person?.Email);
        }

        private void ThrowExcIfUserCannotBeCreated(User userToBeCreated)
        {
            bool canBeCreated = !userToBeCreated.GetBrokenRules().Any();
            if (!canBeCreated)
                throw new InvalidUserException(userToBeCreated.GetBrokenRulesAsString());
        }

        private void MakeUserPersistent(User userToBeMadePersistence)
        {
            _userRepository. Save(userToBeMadePersistence);
            _uOf.Commit();
        }

    }
}
