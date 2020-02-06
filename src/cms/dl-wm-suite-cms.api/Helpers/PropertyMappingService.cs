using System;
using System.Collections.Generic;
using dl.wm.suite.cms.model.Containers;
using dl.wm.suite.cms.model.Employees;
using dl.wm.suite.cms.model.Employees.Departments;
using dl.wm.suite.cms.model.Employees.EmployeeRoles;
using dl.wm.suite.cms.model.Vehicles;
using dl.wm.suite.common.dtos.Vms.Containers;
using dl.wm.suite.common.dtos.Vms.Employees;
using dl.wm.suite.common.dtos.Vms.Employees.Departments;
using dl.wm.suite.common.dtos.Vms.Employees.EmployeeRoles;
using dl.wm.suite.common.dtos.Vms.Vehicles;
using dl.wm.suite.common.infrastructure.PropertyMappings;

namespace dl.wm.suite.cms.api.Helpers
{
    public class PropertyMappingService : BasePropertyMapping
    {
        private readonly Dictionary<string, PropertyMappingValue> _containerPropertyMapping =
            new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
            {
                {"id", new PropertyMappingValue(new List<string>() {"id"})},
                {"Name", new PropertyMappingValue(new List<string>() {"Name"})},
                {"Address", new PropertyMappingValue(new List<string>() {"Address"})},
            };

        private readonly Dictionary<string, PropertyMappingValue> _employeeRolePropertyMapping =
            new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
            {
                {"id", new PropertyMappingValue(new List<string>() {"id"})},
                {"Name", new PropertyMappingValue(new List<string>() {"Name"})},
            };

        private readonly Dictionary<string, PropertyMappingValue> _departmentPropertyMapping =
            new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
            {
                {"id", new PropertyMappingValue(new List<string>() {"id"})},
                {"Name", new PropertyMappingValue(new List<string>() {"Name"})},
            };

        private readonly Dictionary<string, PropertyMappingValue> _employeePropertyMapping =
            new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
            {
                {"id", new PropertyMappingValue(new List<string>() {"id"})},
                {"Firstname", new PropertyMappingValue(new List<string>() {"Firstname"})},
                {"Lastname", new PropertyMappingValue(new List<string>() {"Lastname"})},
            };

        private readonly Dictionary<string, PropertyMappingValue> _vehiclePropertyMapping =
            new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
            {
                {"id", new PropertyMappingValue(new List<string>() {"id"})},
                {"VehicleBrand", new PropertyMappingValue(new List<string>() {"Brand"})},
                {"VehicleNumPlate", new PropertyMappingValue(new List<string>() {"NumPlate"})},
            };

        private static readonly IList<IPropertyMapping> PropertyMappings = new List<IPropertyMapping>();

        public PropertyMappingService() : base(PropertyMappings)
        {
            PropertyMappings.Add(new PropertyMapping<ContainerUiModel, Container>(_containerPropertyMapping));
            PropertyMappings.Add(new PropertyMapping<EmployeeUiModel, Employee>(_employeePropertyMapping));
            PropertyMappings.Add(new PropertyMapping<VehicleUiModel, Vehicle>(_vehiclePropertyMapping));
            PropertyMappings.Add(new PropertyMapping<DepartmentUiModel, Department>(_departmentPropertyMapping));
            PropertyMappings.Add(new PropertyMapping<EmployeeRoleUiModel, EmployeeRole>(_employeeRolePropertyMapping));
        }
    }
}
