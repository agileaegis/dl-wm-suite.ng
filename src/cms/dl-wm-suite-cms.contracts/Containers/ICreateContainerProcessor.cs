﻿using System;
using System.Threading.Tasks;
using dl.wm.suite.common.dtos.Vms.Containers;

namespace dl.wm.suite.cms.contracts.Containers
{
    public interface ICreateContainerProcessor
    {
        Task<ContainerUiModel> CreateContainerAsync(Guid accountIdToCreateThisContainer, ContainerForCreationModel newContainerUiModel);
    }
}