using System.Collections.Generic;
using dl.wm.suite.ui.Controls;
using dl.wm.suite.ui.Views.Components.UsersRolesDepartments;

namespace dl.wm.suite.ui.Views.Repositories
{
    public sealed class ModuleUserRoleViewRepository
    {
        private readonly IDictionary<string, BaseModule> _userRoleViewRepository;
        private ModuleUserRoleViewRepository()
        {
            _userRoleViewRepository = new Dictionary<string, BaseModule>()
            {
                {"UserManagement", new UcClientsUsers()},
                {"RoleManagement", new UcClientsRoles()},
                {"UserEmployeeDepartmentManagement", new UcClientsUserDepartments()},
                {"UserEmployeeRoleManagement", new UcClientsUserDepartments()},
            };
        }

        public static ModuleUserRoleViewRepository ViewRepository { get; } = new ModuleUserRoleViewRepository();

        public BaseModule this[string index] => _userRoleViewRepository[index];
    }
}
