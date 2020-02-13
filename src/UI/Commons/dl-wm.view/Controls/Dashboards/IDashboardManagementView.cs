using System;
using dl.wm.view.Commons;

namespace dl.wm.view.Controls.Dashboards
{
    public interface IDashboardManagementView : IView
    {
        bool OnDashboardLoaded { set; }
        bool RibbonLockMapEnabled { get; set; }
        bool RibbonLockMapValue { get; set; }
        bool RibbonLockMapSvgImageIsBlack { set; }
        bool RibbonLockMapSvgImageIsOrange { set; }
        bool RibbonGeofenceValue { get; set; }
        bool RibbonGeofenceSvgImageIsBlack {  set; }
        bool RibbonGeofenceSvgImageIsOrange { set; }
    }
}
