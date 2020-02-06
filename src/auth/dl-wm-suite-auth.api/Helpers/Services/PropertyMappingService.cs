using System;
using System.Collections.Generic;
using dl.wm.suite.auth.api.Helpers.Models;
using dl.wm.suite.common.dtos.Vms.Roles;
using dl.wm.suite.common.dtos.Vms.Users;
using dl.wm.suite.common.infrastructure.PropertyMappings;

namespace dl.wm.suite.auth.api.Helpers.Services
{
    public class PropertyMappingService : BasePropertyMapping
    {
        private readonly Dictionary<string, PropertyMappingValue> _rolePropertyMapping =
            new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
            {
                { "Id", new PropertyMappingValue(new List<string>() { "Id" } ) },
                { "Name", new PropertyMappingValue(new List<string>() { "Name"}) },
            };

        private readonly Dictionary<string, PropertyMappingValue> _userPropertyMapping =
            new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
            {
                { "Id", new PropertyMappingValue(new List<string>() { "Id" } ) },
                { "Login", new PropertyMappingValue(new List<string>() { "Login"}) },
                { "IsActivated", new PropertyMappingValue(new List<string>() { "IsActivated"}) },
                { "CreatedBy", new PropertyMappingValue(new List<string>() { "CreatedBy"}) },
            };

        private static readonly IList<IPropertyMapping> PropertyMappings = new List<IPropertyMapping>();

        public PropertyMappingService() : base(PropertyMappings)
        {
            PropertyMappings.Add(new PropertyMapping<RoleUiModel, Role>(_rolePropertyMapping));
            PropertyMappings.Add(new PropertyMapping<UserUiModel, User>(_userPropertyMapping));
        }
    }
}
