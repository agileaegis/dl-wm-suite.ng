using System.Collections.Generic;
using dl.wm.suite.ui.Controls;
using dl.wm.suite.ui.Views.Components;

namespace dl.wm.suite.ui.Views.Repositories
{
    public sealed class ModuleViewRepository
    {
        private readonly IDictionary<string, BaseModule> _publishersViewRepository;
        private ModuleViewRepository()
        {
            _publishersViewRepository = new Dictionary<string, BaseModule>()
            {
                {"DicomSettings", new UcSettingsDicom()},
                {"DicomClients", new UcClientsDicom()},
                {"LabelSettings", new UcSettingsLabel()},
            };
        }

        public static ModuleViewRepository ViewRepository { get; } = new ModuleViewRepository();

        public BaseModule this[string index] => _publishersViewRepository[index];
    }
}
