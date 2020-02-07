using System.Collections.Generic;
using dl.wm.models.DTOs.Dashboards;

namespace dl.wm.view.Controls.Dashboards.Maps
{
    public interface IMapManagementView : IView
    {
        List<MapUiModel> Geofence { get; set; }
        List<MapUiModel> ChangedGeofence { get; set; }
        bool NoneGeofencePointWasRetrieved { set; }
        bool CanAddPointToMap { get; set; }
        bool ToggleCanAddPointToMap { get; set; }
        bool ChkPopulateGeofenceOnDemand { get; set; }
    }
}
