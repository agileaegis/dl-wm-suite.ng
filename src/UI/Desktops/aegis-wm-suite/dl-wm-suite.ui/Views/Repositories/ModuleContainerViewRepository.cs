using System.Collections.Generic;
using dl.wm.suite.ui.Controls;
using dl.wm.suite.ui.Views.Components.Containers;

namespace dl.wm.suite.ui.Views.Repositories
{
    public sealed class ModuleContainerViewRepository
    {
        public readonly IDictionary<string, BaseModule> ContainersViewRepository;
        private ModuleContainerViewRepository()
        {
            ContainersViewRepository = new Dictionary<string, BaseModule>()
            {
                {"ContainerManagement", new UcContainerClientsManagementContainers()},
                {"ContainerMonitoring", new UcClientsMonitoringContainers()},
            };
        }

        public static ModuleContainerViewRepository ViewRepository { get; } = new ModuleContainerViewRepository();

        public BaseModule this[string index] => ContainersViewRepository[index];
    }
}
