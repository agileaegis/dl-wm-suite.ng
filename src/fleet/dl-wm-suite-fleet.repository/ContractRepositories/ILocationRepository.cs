using dl.wm.suite.common.infrastructure.Domain;
using dl.wm.suite.fleet.model.Locations;

namespace dl.wm.suite.fleet.repository.ContractRepositories
{
    public interface ILocationRepository : IRepository<Location, int>
    {
        Location FindPoint(string point);
        Location FindPoint(int id);
    }
}
