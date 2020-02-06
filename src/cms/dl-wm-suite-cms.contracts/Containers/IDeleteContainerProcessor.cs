using System;
using System.Threading.Tasks;
using dl.wm.suite.common.dtos.Vms.Containers;

namespace dl.wm.suite.cms.contracts.Containers
{
    public interface IDeleteContainerProcessor
    {
        Task<ContainerForDeletionUiModel> SoftDeleteContainerAsync(Guid accountIdToDeleteThisContainer, Guid containerToBeDeletedId);
        Task<ContainerForDeletionUiModel> HardDeleteContainerAsync(Guid accountIdToDeleteThisContainer, Guid containerToBeDeletedId);
    }
}