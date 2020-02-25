using System;
using System.Collections.Generic;
using dl.wm.suite.cms.model.Tours.Trackables;
using dl.wm.suite.common.infrastructure.Domain;
using dl.wm.suite.common.infrastructure.Domain.Queries;

namespace dl.wm.suite.cms.repository.ContractRepositories
{
  public interface ITrackableRepository : IRepository<Trackable, Guid>
  {
    Trackable FindOneByName(string name);
    Trackable FindOneByVendorId(string vendorId);

    QueryResult<Trackable> FindAllTrackablesPagedOf(int? pageNum = -1, int? pageSize = -1);
    QueryResult<Trackable> FindAllActiveTrackablesPagedOf(int? pageNum = -1, int? pageSize = -1);
    IList<Trackable> FindAtLeastOneByImeiOrPhone(string imei, string phone);
    Trackable FindOneByImeiOrPhone(string imei, string phone);
  }
}