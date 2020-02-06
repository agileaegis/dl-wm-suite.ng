using System;
using System.Collections.Generic;
using dl.wm.suite.common.dtos.Vms.Assets;
using dl.wm.suite.common.dtos.Vms.Trackables;
using dl.wm.suite.common.dtos.Vms.Trips;
using dl.wm.suite.common.infrastructure.PropertyMappings;
using dl.wm.suite.fleet.model.Assets;
using dl.wm.suite.fleet.model.Trackables;
using dl.wm.suite.fleet.model.Trips;

namespace dl.wm.suite.fleet.api.Helpers
{
    public class PropertyMappingService : BasePropertyMapping
    {
        private readonly Dictionary<string, PropertyMappingValue> _assetPropertyMapping =
            new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
            {
                {"id", new PropertyMappingValue(new List<string>() {"id"})},
                {"Name", new PropertyMappingValue(new List<string>() {"Name"})},
                {"NumPlate", new PropertyMappingValue(new List<string>() {"NumPlate"})},
            };

        private readonly Dictionary<string, PropertyMappingValue> _tripPropertyMapping =
            new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
            {
                {"id", new PropertyMappingValue(new List<string>() {"id"})},
                {"Code", new PropertyMappingValue(new List<string>() {"Code"})},
            };

        private readonly Dictionary<string, PropertyMappingValue> _trackablePropertyMapping =
            new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
            {
                {"id", new PropertyMappingValue(new List<string>() {"id"})},
                {"Name", new PropertyMappingValue(new List<string>() {"Name"})},
                {"Imei", new PropertyMappingValue(new List<string>() {"Imei"})},
                {"Phone", new PropertyMappingValue(new List<string>() {"Phone"})},
            };


        private static readonly IList<IPropertyMapping> PropertyMappings = new List<IPropertyMapping>();

        public PropertyMappingService() : base(PropertyMappings)
        {
            PropertyMappings.Add(new PropertyMapping<TripUiModel, Trip>(_tripPropertyMapping));
            PropertyMappings.Add(new PropertyMapping<AssetUiModel, Asset>(_assetPropertyMapping));
            PropertyMappings.Add(new PropertyMapping<TrackableUiModel, Trackable>(_trackablePropertyMapping));
        }
    }
}
