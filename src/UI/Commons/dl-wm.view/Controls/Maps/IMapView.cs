using System.Collections.Generic;
using dl.wm.models.DTOs.Users;

namespace dl.wm.view.Controls.Maps
{
    public interface IMapView : IView
    {
        double PointLat { get; set; }
        double PointLon { get; set; }
        string PointAddress { get; set; }
    }
}