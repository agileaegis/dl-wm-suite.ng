using System;
using System.Collections.Generic;
using dl.wm.suite.common.infrastructure.Domain;
using dl.wm.suite.common.infrastructure.Domain.Queries;
using dl.wm.suite.fleet.model.Trips;

namespace dl.wm.suite.fleet.repository.ContractRepositories
{
    public interface ITripRepository : IRepository<Trip, int>
    {
        QueryResult<Trip> FindAllTripsPagedOf(int? pageNum = -1, int? pageSize = -1);

        IList<Trip> FindAtLeastOneByCode(string code);
        IList<int> FindAllTodaysTripIds();
        Trip FindOneByCode(string code);
        Trip FindOneEnabledByVendorId(string vendorId);
    }
}
