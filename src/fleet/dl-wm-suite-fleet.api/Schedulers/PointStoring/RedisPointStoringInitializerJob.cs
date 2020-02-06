using System.Threading.Tasks;
using Quartz;

namespace dl.wm.suite.fleet.api.Schedulers.PointStoring
{
    public class RedisPointStoringInitializerJob : IJob
    {

        public RedisPointStoringInitializerJob()
        {
        }

        public Task Execute(IJobExecutionContext context)
        {
            FleetServer.GetFleetServer.RaisePointStoringDetection();
            return Task.CompletedTask;
        }
    }
}