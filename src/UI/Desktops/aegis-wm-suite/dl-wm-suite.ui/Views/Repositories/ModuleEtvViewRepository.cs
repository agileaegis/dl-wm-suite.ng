using System.Collections.Generic;
using dl.wm.suite.ui.Controls;
using dl.wm.suite.ui.Views.Components.EmployeesToursVehicles;

namespace dl.wm.suite.ui.Views.Repositories
{
    public sealed class ModuleEtvViewRepository
    {
        private readonly IDictionary<string, BaseModule> _evtsViewRepository;
        private ModuleEtvViewRepository()
        {
            _evtsViewRepository = new Dictionary<string, BaseModule>()
            {
                {"EmployeeManagement", new UcClientsEmployees()},
                {"EmployeeTourManagement", new UcClientsEmployeesTours()},
                {"TourManagement", new UcClientsTours()},
                {"HistoryTourManagement", new UcClientsHistoryTours()},
                {"VehicleManagement", new UcClientsVehicles()},
            };
        }

        public static ModuleEtvViewRepository ViewRepository { get; } = new ModuleEtvViewRepository();

        public BaseModule this[string index] => _evtsViewRepository[index];
    }
}
