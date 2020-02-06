using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dl.wm.suite.fleet.api.Redis.Models;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace dl.wm.suite.fleet.api.Redis.TrackingPoints
{
    public class TrackingRedisRepository : ITrackingRedisRepository
    {
        private readonly ConnectionMultiplexer _redis;
        private readonly IDatabase _database;

        public TrackingRedisRepository(ConnectionMultiplexer redis)
        {
            _redis = redis;
            _database = redis.GetDatabase();
        }

        private string Serialize(object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }
        private T Deserialize<T>(string serialized)
        {
            return JsonConvert.DeserializeObject<T>(serialized);
        }

        public async Task<bool> AddTrackingPointAsync(string key, TrackingPointRedisModel trackingPointModel)
        {
            return _database.ListRightPush(key, Serialize(trackingPointModel)) > 0;
        }

        public async Task<TrackingPointRedisModel> GetTrackingPointAsync(string key)
        {
            var trackingPointRedisValue = _database.ListLeftPop(key);
            return !trackingPointRedisValue.IsNullOrEmpty ? Deserialize<TrackingPointRedisModel>(trackingPointRedisValue) : null;
        }
    }
}
