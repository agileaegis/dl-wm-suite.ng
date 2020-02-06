using System;
using System.Linq;
using System.Threading.Tasks;
using dl.wm.suite.auth.api.Helpers.Models;
using dl.wm.suite.auth.api.Helpers.Repositories.Users;
using dl.wm.suite.auth.api.Helpers.Services.Users.Contracts;
using dl.wm.suite.common.dtos.Vms.Accounts;
using dl.wm.suite.common.dtos.Vms.Users;
using dl.wm.suite.common.infrastructure.Exceptions.Domain.Users;
using dl.wm.suite.common.infrastructure.TypeMappings;
using dl.wm.suite.common.infrastructure.UnitOfWorks;
using Serilog;

namespace dl.wm.suite.auth.api.Helpers.Services.Users.Impls
{
    public class ActivateUserProcessor : IActivateUserProcessor
    {
        private readonly IUnitOfWork _uOf;
        private readonly IUserRepository _userRepository;
        private readonly IAutoMapper _autoMapper;
        public ActivateUserProcessor(IUnitOfWork uOf, IAutoMapper autoMapper, IUserRepository userRepository)
        {
            _uOf = uOf;
            _userRepository = userRepository;
            _autoMapper = autoMapper;
        }
        public Task<UserActivationUiModel> UpdateUserActivatioAnync(Guid userIdToBeActivated, Guid accountIdToActivateThisUser,
            AccountForActivationModification activationAccount)
        {
            var response =
                new UserActivationUiModel()
                {
                    Message = "START_ACTIVATION"
                };

            if(userIdToBeActivated == Guid.Empty || activationAccount?.ActivationKey == Guid.Empty)
            {
                response.Message = "ERROR_INVALID_ACTIVATION_MODEL";
                return Task.Run(() => response);
            }

            try
            {
                var userToBeActivated = ThrowExceptionIfUserDoesNotExist(userIdToBeActivated, activationAccount.ActivationKey);
                ThrowExcIfUserCanNotBeUpdated(userToBeActivated);

                userToBeActivated.Activate();
                userToBeActivated.InjectWithAudit(accountIdToActivateThisUser);


                Log.Debug(
                    $"Activate User: {userIdToBeActivated}" +
                    "--ActivateUser--  @NotComplete@ [ActivateUserProcessor]. " +
                    "Message: Just Before MakeItPersistence");
                MakeUserPersistent(userToBeActivated);

                Log.Debug(
                    $"Activate User: {userIdToBeActivated}" +
                    "--ActivateUser--  @Complete@ [ActivateUserProcessor]. " +
                    "Message: Just After MakeItPersistence");

                response = ThrowExcIfUserWasNotBeActivateAfterPersistent(userIdToBeActivated);

                response.Message = "SUCCESS_ACTIVATION";
            }
            catch (InvalidUserException e)
            {
                response.Message = "ERROR_INVALID_USER_MODEL";
                Log.Error(
                    $"Activate User: {userIdToBeActivated}" +
                    $"Error Message:{response.Message}" +
                    "--ActivateUser--  @NotComplete@ [ActivateUserProcessor]. " +
                    $"Broken rules: {e.BrokenRules}");
            }
            catch (UserDoesNotExistException ex)
            {
                response.Message = "ERROR_USER_DOES_NOT_EXISTS";
                Log.Error(
                    $"Activate User: {userIdToBeActivated}" +
                    $"Error Message:{response.Message}" +
                    "--ActivateUser--  @fail@ [ActivateUserProcessor]. " +
                    $"@innerfault:{ex?.Message} and {ex?.InnerException}");
            }
            catch (UserDoesNotActivatedAfterMadePersistentException exx)
            {
                response.Message = "ERROR_USER_NOT_ACTIVATE_PERSISTENT";
                Log.Error(
                    $"Activate User: {userIdToBeActivated}" +
                    $"Error Message:{response.Message}" +
                    "--ActivateUser--  @fail@ [ActivateUserProcessor]." +
                    $" @innerfault:{exx?.Message} and {exx?.InnerException}");
            }
            catch (Exception exxx)
            {
                response.Message = "UNKNOWN_ERROR";
                Log.Error(
                    $"Activate User: {userIdToBeActivated}" +
                    $"Error Message:{response.Message}" +
                    $"--ActivateUser--  @fail@ [ActivateUserProcessor]. " +
                    $"@innerfault:{exxx.Message} and {exxx.InnerException}");
            }

            return Task.Run(() => response);
        }

        private UserActivationUiModel ThrowExcIfUserWasNotBeActivateAfterPersistent(Guid userIdToBeActivated)
        {
            var retrievedUser = _userRepository.FindBy(userIdToBeActivated);
            if (retrievedUser != null && retrievedUser.IsActive)
                return _autoMapper.Map<UserActivationUiModel>(retrievedUser);
            throw new UserDoesNotActivatedAfterMadePersistentException(retrievedUser.Login);
        }

        private void MakeUserPersistent(User userToBeMadePersistence)
        {
            _userRepository.Save(userToBeMadePersistence);
            _uOf.Commit();
        }

        private User ThrowExceptionIfUserDoesNotExist(Guid userIdToBeActivated, Guid activationKey)
        {
            var userToBeUpdated = _userRepository.FindByUserIdAndActivationKey(userIdToBeActivated, activationKey);
            if (userToBeUpdated == null)
                throw new UserDoesNotExistException(userIdToBeActivated);
            return userToBeUpdated;
        }

        private void ThrowExcIfUserCanNotBeUpdated(User userToBeActivated)
        {
            var canBeUpdated = !userToBeActivated.GetBrokenRules().Any();
            if (!canBeUpdated)
                throw new InvalidUserException(userToBeActivated.GetBrokenRulesAsString());
        }
    }
}
