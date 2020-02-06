using System;
using System.Collections.Generic;
using System.Linq;
using dl.wm.suite.common.infrastructure.Domain.Queries;
using dl.wm.suite.common.infrastructure.Paging;
using dl.wm.suite.fleet.model.Assets;
using dl.wm.suite.fleet.model.Locations;
using dl.wm.suite.fleet.repository.ContractRepositories;
using dl.wm.suite.fleet.repository.Repositories.Base;
using NHibernate;
using NHibernate.Criterion;

namespace dl.wm.suite.fleet.repository.Repositories
{
    public class LocationRepository : RepositoryBase<Location, int>, ILocationRepository
    {
        public LocationRepository(ISession session)
            : base(session)
        {
        }

        public Location FindPoint(string point)
        {
            throw new NotImplementedException();
        }

        public Location FindPoint(int id)
        {
            //var result = Session.Query<Location>().OrderByDescending(m => m.Geo.Area)
            //    .Select(m => new {m.Name, m.Geo})
            //    .Take(1)
            //    .FirstOrDefault();

            return null;
        }
    }
}
