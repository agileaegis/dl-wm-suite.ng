using System;
using System.Linq;
using System.Threading.Tasks;
using dl.wm.suite.common.dtos.Vms.Assets;
using dl.wm.suite.common.infrastructure.Exceptions.Domain.Assets;
using dl.wm.suite.common.infrastructure.TypeMappings;
using dl.wm.suite.common.infrastructure.UnitOfWorks;
using dl.wm.suite.fleet.contracts.Assets;
using dl.wm.suite.fleet.model.Assets;
using dl.wm.suite.fleet.repository.ContractRepositories;
using Serilog;

namespace dl.wm.suite.fleet.services.Assets
{
    public class CreateAssetProcessor : ICreateAssetProcessor
    {
        private readonly IUnitOfWork _uOf;
        private readonly IAssetRepository _assetRepository;
        private readonly IAutoMapper _autoMapper;

        public CreateAssetProcessor(IUnitOfWork uOf, IAutoMapper autoMapper,
            IAssetRepository assetRepository)
        {
            _uOf = uOf;
            _assetRepository = assetRepository;
            _autoMapper = autoMapper;
        }


        public Task<AssetUiModel> CreateAssetAsync(string accountEmailToCreateThisAsset, AssetForCreationUiModel newAssetUiModel)
        {
            var response =
                new AssetUiModel()
                {
                    Message = "START_CREATION"
                };

            if (newAssetUiModel == null)
            {
                response.Message = "ERROR_INVALID_ASSET_MODEL";
                return Task.Run(() => response);
            }
            
            try
            {
                var assetToBeCreated = _autoMapper.Map<Asset>(newAssetUiModel);

                ThrowExcIfTackableCannotBeCreated(assetToBeCreated);
                ThrowExcIfThisAssetAlreadyExist(assetToBeCreated);

                assetToBeCreated.InjectWithAudit(accountEmailToCreateThisAsset);

                Log.Debug(
                    $"Create Asset: with Name: {newAssetUiModel.AssetName} and NumPlate: {newAssetUiModel.AssetNumPlate}" +
                    "--CreateAsset--  @Ready@ [CreateAssetProcessor]. " +
                    "Message: Just Before MakeItPersistence");

                MakeAssetPersistent(assetToBeCreated);

                Log.Debug(
                    $"Create Asset: with Name: {newAssetUiModel.AssetName} and NumPlate: {newAssetUiModel.AssetNumPlate}" +
                    "--CreateAsset--  @Ready@ [CreateAssetProcessor]. " +
                    "Message: Just After MakeItPersistence");

                response = ThrowExcIfAssetWasNotBeMadePersistent(assetToBeCreated);
                response.Message = "SUCCESS_CREATION";
            }
            catch (InvalidAssetException e)
            {
                response.Message = "ERROR_INVALID_ASSET_MODEL";
                Log.Error(
                    $"Create Asset: Name: {newAssetUiModel.AssetName} - NumPlate: {newAssetUiModel.AssetNumPlate}" +
                    $"Error Message:{response.Message}" +
                    "--CreateAsset--  @NotComplete@ [CreateAssetProcessor]. " +
                    $"Broken rules: {e.BrokenRules}");
            }
            catch (AssetAlreadyExistsException ex)
            {
                response.Message = "ERROR_ASSET_ALREADY_EXISTS";
                Log.Error(
                    $"Create Asset: Name: {newAssetUiModel.AssetName} - NumPlate: {newAssetUiModel.AssetNumPlate}" +
                    $"Error Message:{response.Message}" +
                    "--CreateAsset--  @fail@ [CreateAssetProcessor]. " +
                    $"@innerfault:{ex?.Message} and {ex?.InnerException}");
            }
            catch (AssetDoesNotExistAfterMadePersistentException exx)
            {
                response.Message = "ERROR_ASSET_NOT_MADE_PERSISTENT";
                Log.Error(
                    $"Create Asset: Name: {newAssetUiModel.AssetName} - NumPlate: {newAssetUiModel.AssetNumPlate}" +
                    $"Error Message:{response.Message}" +
                    "--CreateAsset--  @fail@ [CreateAssetProcessor]." +
                    $" @innerfault:{exx?.Message} and {exx?.InnerException}");
            }
            catch (Exception exxx)
            {
                response.Message = "UNKNOWN_ERROR";
                Log.Error(
                    $"Create Asset: Name: {newAssetUiModel.AssetName} - NumPlate: {newAssetUiModel.AssetNumPlate}" +
                    $"Error Message:{response.Message}" +
                    $"--CreateAsset--  @fail@ [CreateAssetProcessor]. " +
                    $"@innerfault:{exxx.Message} and {exxx.InnerException}");
            }
            
            return Task.Run(() => response);
        }


        private void ThrowExcIfThisAssetAlreadyExist(Asset assetToBeCreated)
        {
            var assetRetrieved =
                _assetRepository.FindAtLeastOneByNameOrNumPlate(assetToBeCreated.Name, assetToBeCreated.NumPlate);
            if (assetRetrieved.Count != 0)
            {
                throw new AssetAlreadyExistsException(assetToBeCreated.GetBrokenRulesAsString(),
                    assetToBeCreated.NumPlate);
            }
        }

        private AssetUiModel ThrowExcIfAssetWasNotBeMadePersistent(Asset assetToBeCreated)
        {
            var retrievedAsset =
                _assetRepository.FindOneByNameOrNumPlate(assetToBeCreated.Name, assetToBeCreated.NumPlate);
            if (retrievedAsset != null)
                return _autoMapper.Map<AssetUiModel>(retrievedAsset);
            throw new AssetDoesNotExistAfterMadePersistentException(assetToBeCreated.NumPlate);
        }

        private void ThrowExcIfTackableCannotBeCreated(Asset assetToBeCreated)
        {
            bool canBeCreated = !assetToBeCreated.GetBrokenRules().Any();
            if (!canBeCreated)
                throw new InvalidAssetException(assetToBeCreated.GetBrokenRulesAsString());
        }

        private void MakeAssetPersistent(Asset assetToBeCreated)
        {
            _assetRepository.Save(assetToBeCreated);
            _uOf.Commit();
        }

    }
}
