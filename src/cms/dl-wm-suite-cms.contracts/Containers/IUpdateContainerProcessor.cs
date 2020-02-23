using System;
using System.Threading.Tasks;
using dl.wm.suite.common.dtos.Vms.Containers;
using dl.wm.suite.common.infrastructure.Helpers.Azure;

namespace dl.wm.suite.cms.contracts.Containers
{
    public interface IUpdateContainerProcessor
    {
        Task<ContainerUiModel> UpdateContainerAsync(Guid accountIdToUpdateThisContainer, Guid containerToBeModified, ContainerForModificationUiModel updatedContainer);
        Task<ContainerDeviceProvisioningUiModel> ProvisioningDeviceToContainerAsync(Guid userAuditId, Guid id, Guid deviceId, ContainerForModificationProvisioningModel containerForModificationProvisioningModel);
        Task BatchUpdateContainerAsync(AzureStorageConfig azureStorage);
    }
}
