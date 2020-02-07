using System.Collections.Generic;
using DevExpress.XtraMap;

namespace dl.wm.suite.ui.Views.Modules.Clustering
{
    public class CustomClusterFactory : DefaultClusterItemFactory
    {
        protected override MapItem CreateItemInstance(IList<MapItem> obj)
        {
            return new MapCallout();
        }
    }
}