using dl.wm.suite.ui.Controls;
using DevExpress.Utils.Menu;

namespace dl.wm.suite.ui.Views.Modules
{
    public partial class UcSensors : BaseModule
    {
        public override string ModuleCaption => "Sersors";
        public override bool AllowWaitDialog => true;

        internal override void InitModule(IDXMenuManager manager, object data)
        {
            IsInitialized = true;
            base.InitModule(manager, data);
        }

        internal override void ShowModule(object item)
        {
            if (!IsInitialized)
                return;
            IsShown = true;
            base.ShowModule(item);

            OnShowModuleLocal();
        }

        private void OnShowModuleLocal()
        {
        }

        internal override void HideModule()
        {
            IsShown = false;
            base.HideModule();
        }

        public UcSensors()
        {
            InitializeComponent();
        }
    }
}
