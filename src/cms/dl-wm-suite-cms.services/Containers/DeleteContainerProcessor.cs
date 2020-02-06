using System;
using System.Threading.Tasks;
using dl.wm.suite.cms.contracts.Containers;
using dl.wm.suite.cms.model.Containers;
using dl.wm.suite.cms.repository.ContractRepositories;
using dl.wm.suite.common.dtos.Vms.Containers;
using dl.wm.suite.common.infrastructure.Exceptions.Domain.Containers;
using dl.wm.suite.common.infrastructure.TypeMappings;
using dl.wm.suite.common.infrastructure.UnitOfWorks;
using Serilog;

namespace dl.wm.suite.cms.services.Containers
{
    public class DeleteContainerProcessor : IDeleteContainerProcessor
    {
        private readonly IUnitOfWork _uOf;
        private readonly IContainerRepository _containerRepository;
        private readonly IAutoMapper _autoMapper;

        public DeleteContainerProcessor(IUnitOfWork uOf, IAutoMapper autoMapper, IContainerRepository containerRepository)
        {
            _uOf = uOf;
            _containerRepository = containerRepository;
            _autoMapper = autoMapper;
        }


        public Task<ContainerForDeletionUiModel> SoftDeleteContainerAsync(Guid accountIdToDeleteThisContainer, Guid containerToBeDeletedId)
        {
            var response =
                new ContainerForDeletionUiModel()
                {
                    Message = "START_DELETION"
                };

            if (containerToBeDeletedId == Guid.Empty)
            {
                response.Message = "ERROR_INVALID_CONTAINER_ID";
                return Task.Run(() => response);
            }

            try
            {
                var containerToBeSoftDeleted = _containerRepository.FindBy(containerToBeDeletedId);

                if (containerToBeSoftDeleted == null)
                    throw new ContainerDoesNotExistException(containerToBeDeletedId);

                containerToBeSoftDeleted.SoftDeleted();

                Log.Debug(
                    $"Update-Delete Container: with Id: {containerToBeDeletedId}" +
                    "--SoftDeleteContainer--  @Ready@ [DeleteContainerProcessor]. " +
                    "Message: Just Before MakeItPersistence");

                MakeContainerPersistent(containerToBeSoftDeleted);

                Log.Debug(
                    $"Update-Delete Container: with Id: {containerToBeDeletedId}" +
                    "--SoftDeleteContainer--  @Ready@ [DeleteContainerProcessor]. " +
                    "Message: Just After MakeItPersistence");

                response = ThrowExcIfContainerWasNotBeMadePersistent(containerToBeSoftDeleted);
                response.Message = "SUCCESS_DELETION";
            }
            catch (ContainerDoesNotExistException e)
            {
                response.Message = "ERROR_CONTAINER_DOES_NOT_EXIST";
                Log.Error(
                    $"Update-Delete Container: Id: {containerToBeDeletedId}" +
                    $"Error Message:{response.Message}" +
                    "--SoftDeleteContainer--  @NotComplete@ [DeleteContainerProcessor]. " +
                    $"@innerfault:{e?.Message} and {e?.InnerException}");
            }
            catch (ContainerDoesNotExistAfterMadePersistentException ex)
            {
                response.Message = "ERROR_CONTAINER_DOES_NOT_MADE_PERSISTENCE";
                Log.Error(
                    $"Update-Delete Container: Id: {containerToBeDeletedId}" +
                    $"Error Message:{response.Message}" +
                    "--SoftDeleteContainer--  @NotComplete@ [DeleteContainerProcessor]. " +
                    $"@innerfault:{ex?.Message} and {ex?.InnerException}");
            }
            catch (Exception exx)
            {
                response.Message = "UNKNOWN_ERROR";
                Log.Error(
                    $"Update-Delete Container: Id: {containerToBeDeletedId}" +
                    $"Error Message:{response.Message}" +
                    $"--SoftDeleteContainer--  @fail@ [DeleteContainerProcessor]. " +
                    $"@innerfault:{exx.Message} and {exx.InnerException}");
            }

            return Task.Run(() => response);
        }


        public Task<ContainerForDeletionUiModel> HardDeleteContainerAsync(Guid accountIdToDeleteThisContainer, Guid containerToBeDeletedId)
        {
            var response =
                new ContainerForDeletionUiModel()
                {
                    Message = "START_HARD_DELETION"
                };

            if (containerToBeDeletedId == Guid.Empty)
            {
                response.Message = "ERROR_INVALID_CONTAINER_ID";
                return Task.Run(() => response);
            }

            try
            {
                var containerToBeSoftDeleted = _containerRepository.FindBy(containerToBeDeletedId);

                if (containerToBeSoftDeleted == null)
                    throw new ContainerDoesNotExistException(containerToBeDeletedId);

                Log.Debug(
                    $"Update-Delete Container: with Id: {containerToBeDeletedId}" +
                    "--HardDeleteContainer--  @Ready@ [DeleteContainerProcessor]. " +
                    "Message: Just Before MakeItPersistence");

                MakeContainerTransient(containerToBeSoftDeleted);

                Log.Debug(
                    $"Update-Delete Container: with Id: {containerToBeDeletedId}" +
                    "--HardDeleteContainer--  @Ready@ [DeleteContainerProcessor]. " +
                    "Message: Just After MakeItPersistence");

                response.DeletionStatus  = ThrowExcIfContainerWasNotBeMadeTransient(containerToBeSoftDeleted);
                response.Message = "SUCCESS_DELETION";

            }
            catch (ContainerDoesNotExistException e)
            {
                response.Message = "ERROR_CONTAINER_DOES_NOT_EXIST";
                Log.Error(
                    $"Delete Container: Id: {containerToBeDeletedId}" +
                    $"Error Message:{response.Message}" +
                    "--HardDeleteContainer--  @NotComplete@ [DeleteContainerProcessor]. " +
                    $"@innerfault:{e?.Message} and {e?.InnerException}");
            }
            catch (ContainerDoesNotExistAfterMadeTransientException ex)
            {
                response.Message = "ERROR_CONTAINER_DOES_NOT_MADE_TRANSIENT";
                Log.Error(
                    $"Delete Container: Id: {containerToBeDeletedId}" +
                    $"Error Message:{response.Message}" +
                    "--HardDeleteContainer--  @NotComplete@ [DeleteContainerProcessor]. " +
                    $"@innerfault:{ex?.Message} and {ex?.InnerException}");
            }
            catch (Exception exxx)
            {
                response.Message = "UNKNOWN_ERROR";
                Log.Error(
                    $"Delete Container: Id: {containerToBeDeletedId}" +
                    $"Error Message:{response.Message}" +
                    $"--HardDeleteContainer--  @fail@ [DeleteContainerProcessor]. " +
                    $"@innerfault:{exxx.Message} and {exxx.InnerException}");
            }

            return Task.Run(() => response);
        }

        private ContainerForDeletionUiModel ThrowExcIfContainerWasNotBeMadePersistent(Container containerToBeSoftDeleted)
        {
            var retrievedContainer =
                _containerRepository.FindBy(containerToBeSoftDeleted.Id);
            if (retrievedContainer != null)
                return _autoMapper.Map<ContainerForDeletionUiModel>(retrievedContainer);
            throw new ContainerDoesNotExistAfterMadePersistentException(containerToBeSoftDeleted.Id);
        }

        private bool ThrowExcIfContainerWasNotBeMadeTransient(Container containerToBeSoftDeleted)
        {
            var retrievedContainer =
                _containerRepository.FindBy(containerToBeSoftDeleted.Id);
            return retrievedContainer != null
                ? throw new ContainerDoesNotExistAfterMadePersistentException(containerToBeSoftDeleted.Id)
                : true;
        }

        private void MakeContainerTransient(Container containerToBeSoftDeleted)
        {
            _containerRepository.Remove(containerToBeSoftDeleted);
            _uOf.Commit();
        }



        private void MakeContainerPersistent(Container containerToBeUpdated)
        {
            _containerRepository.Save(containerToBeUpdated);
            _uOf.Commit();
        }

    }
}
