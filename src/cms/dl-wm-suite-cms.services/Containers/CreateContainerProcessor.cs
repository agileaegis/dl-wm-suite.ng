using System;
using System.Linq;
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
    public class CreateContainerProcessor : ICreateContainerProcessor
    {
        private readonly IUnitOfWork _uOf;
        private readonly IContainerRepository _containerRepository;
        private readonly IAutoMapper _autoMapper;

        public CreateContainerProcessor(IUnitOfWork uOf, IAutoMapper autoMapper,
            IContainerRepository containerRepository)
        {
            _uOf = uOf;
            _containerRepository = containerRepository;
            _autoMapper = autoMapper;
        }

        public Task<ContainerUiModel> CreateContainerAsync(Guid accountIdToCreateThisContainer,
            ContainerForCreationModel newContainerUiModel)
        {
            var response =
                new ContainerUiModel()
                {
                    Message = "START_CREATION"
                };

            if (newContainerUiModel == null)
            {
                response.Message = "ERROR_INVALID_CONTAINER_MODEL";
                return Task.Run(() => response);
            }
            
            try
            {
                var containerToBeCreated = _autoMapper.Map<Container>(newContainerUiModel);

                containerToBeCreated.InjectWithLocation(newContainerUiModel.ContainerLat, newContainerUiModel.ContainerLong);
                containerToBeCreated.InjectWithAudit(accountIdToCreateThisContainer);

                ThrowExcIfContainerCannotBeCreated(containerToBeCreated);
                ThrowExcIfThisContainerAlreadyExist(containerToBeCreated);

                Log.Debug(
                    $"Create Container: {newContainerUiModel.ContainerName}" +
                    "--CreateContainer--  @NotComplete@ [CreateContainerProcessor]. " +
                    "Message: Just Before MakeItPersistence");

                MakeContainerPersistent(containerToBeCreated);

                Log.Debug(
                    $"Create Container: {newContainerUiModel.ContainerName}" +
                    "--CreateContainer--  @Complete@ [CreateContainerProcessor]. " +
                    "Message: Just After MakeItPersistence");
                response = ThrowExcIfContainerWasNotBeMadePersistent(containerToBeCreated);
                response.Message = "SUCCESS_CREATION";
            }
            catch (InvalidContainerException e)
            {
                response.Message = "ERROR_INVALID_CONTAINER_MODEL";
                Log.Error(
                    $"Create Container: {newContainerUiModel.ContainerName}" +
                    "--CreateContainer--  @NotComplete@ [CreateContainerProcessor]. " +
                    $"Broken rules: {e.BrokenRules}");
            }
            catch (ContainerAlreadyExistsException ex)
            {
                response.Message = "ERROR_CONTAINER_ALREADY_EXISTS";
                Log.Error(
                    $"Create Container: {newContainerUiModel.ContainerName}" +
                    "--CreateContainer--  @fail@ [CreateContainerProcessor]. " +
                    $"@innerfault:{ex?.Message} and {ex?.InnerException}");
            }
            catch (ContainerDoesNotExistAfterMadePersistentException exx)
            {
                response.Message = "ERROR_CONTAINER_NOT_MADE_PERSISTENT";
                Log.Error(
                    $"Create Container: {newContainerUiModel.ContainerName}" +
                    "--CreateContainer--  @fail@ [CreateContainerProcessor]." +
                    $" @innerfault:{exx?.Message} and {exx?.InnerException}");
            }
            catch (Exception exxx)
            {
                response.Message = "UNKNOWN_ERROR";
                Log.Error(
                    $"Create Container: {newContainerUiModel.ContainerName}" +
                    $"--CreateContainer--  @fail@ [CreateContainerProcessor]. " +
                    $"@innerfault:{exxx.Message} and {exxx.InnerException}");
            }

            return Task.Run(() => response);
        }

        private void MakeContainerPersistent(Container containerToBeCreated)
        {
            _containerRepository.Save(containerToBeCreated);
            _uOf.Commit();
        }

        private void ThrowExcIfThisContainerAlreadyExist(Container containerToBeCreated)
        {
            var customerRetrieved = _containerRepository.FindOneByName(containerToBeCreated.Name);
            if (customerRetrieved != null)
            {
                throw new ContainerAlreadyExistsException(containerToBeCreated.Name,
                    containerToBeCreated.GetBrokenRulesAsString());
            }
        }

        private ContainerUiModel ThrowExcIfContainerWasNotBeMadePersistent(Container containerToBeCreated)
        {
            var retrievedContainer = _containerRepository.FindOneByName(containerToBeCreated.Name);
            if (retrievedContainer != null)
                return _autoMapper.Map<ContainerUiModel>(retrievedContainer);
            throw new ContainerDoesNotExistAfterMadePersistentException(containerToBeCreated.Name);
        }

        private void ThrowExcIfContainerCannotBeCreated(Container containerToBeCreated)
        {
            bool canBeCreated = !containerToBeCreated.GetBrokenRules().Any();
            if (!canBeCreated)
                throw new InvalidContainerException(containerToBeCreated.GetBrokenRulesAsString());
        }

    }
}
