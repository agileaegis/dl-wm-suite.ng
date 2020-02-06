using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    public class UpdateContainerProcessor : IUpdateContainerProcessor
    {
        private readonly IUnitOfWork _uOf;
        private readonly IContainerRepository _containerRepository;
        private readonly IAutoMapper _autoMapper;

        public UpdateContainerProcessor(IUnitOfWork uOf, IAutoMapper autoMapper, IContainerRepository containerRepository)
        {
            _uOf = uOf;
            _containerRepository = containerRepository;
            _autoMapper = autoMapper;
        }

        public Task<ContainerUiModel> UpdateContainerAsync(Guid accountIdToUpdateThisContainer, Guid containerToBeModified, ContainerForModificationUiModel updatedContainer)
        {
            var response =
                new ContainerUiModel()
                {
                    Message = "START_UPDATE"
                };

            if (updatedContainer == null)
            {
                response.Message = "ERROR_INVALID_CONTAINER_MODEL";
                return Task.Run(() => response);
            }

            if (containerToBeModified == Guid.Empty)
            {
                response.Message = "ERROR_INVALID_CONTAINER_ID";
                return Task.Run(() => response);
            }

            try
            {
                var containerToBeUpdated = ThrowExceptionIfContainerDoesNotExist(containerToBeModified);
                _autoMapper.Map(updatedContainer, containerToBeUpdated);

                ThrowExcIfContainerCanNotBeUpdated(containerToBeUpdated);
                ThrowExcIfThisContainerAlreadyExist(containerToBeUpdated);
                MakeContainerPersistent(containerToBeUpdated);

                response = ThrowExcIfContainerWasNotBeMadePersistent(containerToBeUpdated);
                response.Message = "SUCCESS_UPDATE";
            }
            catch (ContainerDoesNotExistException e)
            {
                response.Message = "ERROR_CONTAINER_NOT_EXIST";
                Log.Error(
                    $"Update Container: {updatedContainer.ContainerName}" +
                    "does not exist --UpdateContainer--  @NotComplete@ [UpdateContainerProcessor]." +
                    $"\nException message:{e.Message}");
            }
            catch (InvalidContainerException ex)
            {
                response.Message = "ERROR_INVALID_CONTAINER_MODEL";
                Log.Error(
                    $"Update Container: {updatedContainer.ContainerName}" +
                    "--UpdateContainer--  @NotComplete@ [UpdateContainerProcessor]. " +
                    $"Broken rules: {ex.BrokenRules}");
            }
            catch (ContainerDoesNotExistAfterMadePersistentException exx)
            {
                response.Message = "ERROR_CONTAINER_NOT_MADE_PERSISTENT";
                Log.Error(
                    $"Update Container: {updatedContainer.ContainerName}" +
                    $"Error Message:{response.Message}" +
                    "--UpdateContainer--  @fail@ [UpdateContainerProcessor]." +
                    $" @innerfault:{exx?.Message} and {exx?.InnerException}");
            }
            catch (ContainerAlreadyExistsException exx)
            {
                response.Message = "ERROR_Container_ALREADY_EXISTS";
                Log.Error(
                    $"Update Container: {updatedContainer.ContainerName}" +
                    "already exists --UpdateContainer--  @NotComplete@ [UpdateContainerProcessor]." +
                    $"\nException message:{exx.Message}");
            }
            catch (Exception exxx)
            {
                response.Message = "UNKNOWN_ERROR";
                Log.Error(
                    $"Update Container: {updatedContainer.ContainerName}" +
                    $"unknown error. " +
                    $"Exception message: {exxx.Message} --UpdateContainer--  @NotComplete@ [UpdateContainerProcessor].");
            }

            return Task.Run(() => response);
        }

        private void MakeContainerPersistent(Container containerToBeMadePersistence)
        {
            _containerRepository.Save(containerToBeMadePersistence);
            _uOf.Commit();
        }

        private Container ThrowExceptionIfContainerDoesNotExist(Guid idContainer)
        {
            var containerToBeUpdated = _containerRepository.FindBy(idContainer);
            if (containerToBeUpdated == null)
                throw new ContainerDoesNotExistException(idContainer);
            return containerToBeUpdated;
        }

        private ContainerUiModel ThrowExcIfContainerWasNotBeMadePersistent(Container containerToBeUpdated)
        {
            var retrievedContainer = _containerRepository.FindOneByName(containerToBeUpdated.Name);
            if (retrievedContainer != null)
                return _autoMapper.Map<ContainerUiModel>(retrievedContainer);
            throw new ContainerDoesNotExistAfterMadePersistentException(containerToBeUpdated.Name);
        }

        private void ThrowExcIfContainerCanNotBeUpdated(Container containerToBeUpdated)
        {
            var canBeUpdated = !containerToBeUpdated.GetBrokenRules().Any();
            if (!canBeUpdated)
                throw new InvalidContainerException(containerToBeUpdated.GetBrokenRulesAsString());
        }

        private void ThrowExcIfThisContainerAlreadyExist(Container containerToBeUpdated)
        {
            var Container =
                _containerRepository.FindOneByName(containerToBeUpdated.Name);
            if (Container != null && Container.Id != containerToBeUpdated.Id)
            {
                throw new ContainerAlreadyExistsException(containerToBeUpdated.Name);
            }
        }
    }
}
