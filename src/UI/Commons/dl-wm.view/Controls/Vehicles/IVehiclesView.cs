using System.Collections.Generic;
using dl.wm.models.DTOs.Vehicles;
using dl.wm.view;

namespace dl.wm.view.Controls.Vehicles
{
    public interface IVehiclesView : IMsgView
    {
        List<VehicleUiModel> Vehicles { get; set; }
        bool NoneVehicleWasRetrieved { set; }
    }
}