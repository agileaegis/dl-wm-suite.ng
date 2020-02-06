using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using dl.wm.suite.cms.api.Redis.Maps.Contracts;
using dl.wm.suite.cms.api.Redis.Models;
using Newtonsoft.Json;
using NHibernate.Cache;
using StackExchange.Redis;

namespace dl.wm.suite.cms.api.Redis.Maps.Impls
{
    public class MapsRedisRepository : IMapsRedisRepository
    {
        private readonly ConnectionMultiplexer _redis;
        private readonly IDatabase _database;

        public MapsRedisRepository(ConnectionMultiplexer redis)
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

        public async Task<GeoPosition?> GetGeoPointAsync(string key, RedisValue pointMember)
        {
            return await _database.GeoPositionAsync(key, pointMember);
        }
        public async Task<bool> AddGeoPointAsync(string key, GeoEntry point)
        {
            return await _database.GeoAddAsync(key, point);
        }

        public async Task<bool> AddMunicipalityAsync(string key, Guid municipalityId, string municipalityName)
        {
            var indexOfMunicipalities= await _database.StringGetAsync(key);
            if (!indexOfMunicipalities.IsNullOrEmpty)
            {
                var municipalitiesStored = await _database.StringGetAsync(key);

                var municipalities =  Deserialize<List<MunicipalityModel>>(municipalitiesStored);

                if (municipalities == null)
                    return false;

                if (municipalities.Any(x => x.Name == municipalityName))
                {
                    return false;
                }

                municipalities.Add(new MunicipalityModel(){ Id = municipalityId, Name = municipalityName});

                return await _database.StringSetAsync(key, Serialize(municipalities));
            }

            return false;
        }

        public async Task<bool> AddMapPointsAsync(string key, List<MapUiModel> mapUiModels)
        {
            var indexOfMapPoints = await _database.StringGetAsync(key);
            if (indexOfMapPoints.IsNullOrEmpty)
            {
                return  await _database.StringSetAsync(key, Serialize(mapUiModels));
            }

            return false;
        }

        public async Task<List<MapUiModel>> GetMapPointsAsync(string key)
        {
            var mapPointsForGeofence = await _database.StringGetAsync(key);
            return !mapPointsForGeofence.IsNullOrEmpty ? Deserialize<List<MapUiModel>>(mapPointsForGeofence) : null;
        }

        public async Task<long> AddGeofencePointAsync(string key, List<GeoEntry> points)
        {
            return await _database.GeoAddAsync(key, points.ToArray());
        }
        public async Task<bool> DeleteGeoPointAsync(string key, GeoEntry point)
        {
            return await _database.GeoRemoveAsync(key, point.Member);
        }
        public async Task<bool> DeleteGeoPointAsync(string key)
        {
            return await _database.KeyDeleteAsync(key, CommandFlags.FireAndForget);
        }

        //Sorted Sets
        public async Task<long> GetCountOfGeofenceEntries(string key)
        {
            return await _database.SortedSetLengthAsync(key);
        }
        public async Task<RedisValue[]> GetGeofenceEntries(string key)
        {
            return await _database.SortedSetRangeByRankAsync(key);
        }
        public async Task<RedisValue> GetPointEntry(string key)
        {
            return await Task.Run(() =>  _database.SortedSetRangeByRank(key).FirstOrDefault());
        }
    }
}
