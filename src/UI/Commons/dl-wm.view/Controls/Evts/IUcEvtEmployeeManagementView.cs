namespace dl.wm.view.Controls.Evts
{
    public interface IUcEvtEmployeeManagementView : IView
    {
        bool OpenFlyoutForAddEmployee { set; }
        bool OnEmployeeManagementLoaded { set; }
    }
}