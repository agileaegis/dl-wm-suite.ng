using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using dl.wm.suite.fleet.api.Redis.Models;
using StackExchange.Redis;

namespace dl.wm.suite.fleet.api.Redis.TrackingPoints
{
    public interface ITrackingRedisRepository
    {
        Task<bool> AddTrackingPointAsync(string key, TrackingPointRedisModel trackingPointModel);
        Task<TrackingPointRedisModel> GetTrackingPointAsync(string key);
    }
}