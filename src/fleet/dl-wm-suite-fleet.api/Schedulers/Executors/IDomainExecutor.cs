using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dl.wm.suite.common.dtos.Vms.TrackingPoints;
using dl.wm.suite.fleet.api.Commanding.Events.Args;
using dl.wm.suite.fleet.api.Commanding.Listeners;
using dl.wm.suite.fleet.api.Redis.TrackingPoints;
using dl.wm.suite.fleet.contracts.Trips;
using dl.wm.suite.fleet.contracts.V1;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace dl.wm.suite.fleet.api.Schedulers.Executors
{
    public interface IDomainExecutor
    {    
        void InitExecutor();
    }

    public class DomainExecutor : IDomainExecutor, IPointStoringActionListener
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ITrackingRedisRepository _trackingRedisRepository;
        private IUpdateTripProcessor _updateTripProcessor;

        public IConfiguration Configuration { get; }

        public DomainExecutor(IConfiguration configuration, IServiceScopeFactory scopeFactory, 
            ITrackingRedisRepository trackingRedisRepository)
        {
            _scopeFactory = scopeFactory;
            _trackingRedisRepository = trackingRedisRepository;
            Configuration = configuration;
        }

        public void InitExecutor()
        {
            FleetServer.GetFleetServer.Attach((IPointStoringActionListener) this);
        }

        public async void Update(object sender, PointStoringEventArgs e)
        {
          using (var scope = _scopeFactory.CreateScope())
          {
            //var tripBlock = scope.ServiceProvider.GetRequiredService<ITripsControllerDependencyBlock>();
            //_updateTripProcessor = tripBlock.UpdateTripProcessor;

            //var inquiryAllTripsProcessor = scope.ServiceProvider.GetRequiredService<IInquiryAllTripsProcessor>();
            //var todaysTripsIds = await inquiryAllTripsProcessor.GetAllTripTodaysIdsAsync();

            //if (todaysTripsIds == null || todaysTripsIds.Count <= 0)
            //  return;

            //foreach (var todaysTripsId in todaysTripsIds)
            //{
            //  var redisTrackingPoint = await _trackingRedisRepository.GetTrackingPointAsync(todaysTripsId.ToString());
            //  var tripToBeUpdated = _updateTripProcessor.CreateTrtackingPoint(redisTrackingPoint.Audit, todaysTripsId,
            //    new TrackingPointDto()
            //    {
            //      Time = redisTrackingPoint.Time,
            //      Latitude = redisTrackingPoint.Latitude,
            //      Longitude = redisTrackingPoint.Longitude,
            //      Accuracy = redisTrackingPoint.Accuracy,
            //      Course = redisTrackingPoint.Course,
            //      Provider = redisTrackingPoint.Provider,
            //      LocationProvider = redisTrackingPoint.LocationProvider,
            //      Altitude = redisTrackingPoint.Altitude,
            //      Speed = redisTrackingPoint.Speed,
            //    });
            //}
          }
        }
    }
}
