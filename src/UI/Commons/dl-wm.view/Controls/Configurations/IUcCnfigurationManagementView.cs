namespace dl.wm.view.Controls.Configurations
{
    public interface IUcCnfigurationManagementView : IView
    {
        string SelectedModuleItem { get; set; }

        bool PopulateUcCtrl { set; }
    }
}
