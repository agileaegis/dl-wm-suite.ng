using System.Collections.Generic;
using DevExpress.XtraMap;

namespace dl.wm.suite.ui.Views.Modules.Clustering
{
    class CustomClusterItemFactory : IClusterItemFactory
    {
        public MapItem CreateClusterItem(IList<MapItem> objects)
        {
            MapPushpin dot = new MapPushpin { ClusteredItems = objects};
            //dot.TitleOptions.Pattern = objects.Count.ToString();
            return dot;
        }
    }
}