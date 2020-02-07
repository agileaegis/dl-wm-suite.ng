namespace dl.wm.view.Controls.Evts
{
    public interface IUcEvtManagementView : IView
    {
        string SelectedModuleItem { get; set; }

        bool PopulateUcCtrl { set; }
    }
}
