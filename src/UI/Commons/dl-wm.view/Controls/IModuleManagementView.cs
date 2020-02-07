namespace dl.wm.view.Controls
{
    public interface IModuleManagementView : IView
    {
        string SelectedModuleItem { get; set; }
        bool PopulateUcCtrl { set; }
    }
}
