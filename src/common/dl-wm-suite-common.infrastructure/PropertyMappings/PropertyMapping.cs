using System.Collections.Generic;

namespace dl.wm.suite.common.infrastructure.PropertyMappings
{
    public class PropertyMapping<TSource, TDestination> : IPropertyMapping
    {
        public Dictionary<string, PropertyMappingValue> MappingDictionary { get; private set; }
        public PropertyMapping(Dictionary<string, PropertyMappingValue> mappingDictionary)
        {
            MappingDictionary = mappingDictionary;
        }
    }

}
