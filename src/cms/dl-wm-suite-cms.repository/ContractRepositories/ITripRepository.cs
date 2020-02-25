using System;
using System.Collections.Generic;
using dl.wm.suite.cms.model.Tours.Trips;
using dl.wm.suite.common.infrastructure.Domain;
using dl.wm.suite.common.infrastructure.Domain.Queries;

namespace dl.wm.suite.cms.repository.ContractRepositories
{
  public interface ITripRepository : IRepository<Trip, Guid>
  {
    QueryResult<Trip> FindAllTripsPagedOf(int? pageNum = -1, int? pageSize = -1);

    IList<Trip> FindAtLeastOneByCode(string code);
    IList<int> FindAllTodaysTripIds();
    Trip FindOneByCode(string code);
    Trip FindOneEnabledByVendorId(string vendorId);
  }
}