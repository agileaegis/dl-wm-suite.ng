using System;
using System.Collections.Generic;
using dl.wm.suite.cms.model.Vehicles;
using dl.wm.suite.common.infrastructure.Domain;
using dl.wm.suite.common.infrastructure.Domain.Queries;

namespace dl.wm.suite.cms.repository.ContractRepositories
{
    public interface IVehicleRepository : IRepository<Vehicle, Guid>
    {
        Vehicle FindByNumPlate(string numPlate);

        Vehicle FindByBrandAndNumPlate(string brand, string numPlate);
        IList<Vehicle> FindAllActiveVehicles();

        QueryResult<Vehicle> FindAllVehiclesPagedOf(int? pageNum = -1, int? pageSize = -1);
        QueryResult<Vehicle> FindAllActiveVehiclesPagedOf(int? pageNum = -1, int? pageSize = -1);
    }
}
