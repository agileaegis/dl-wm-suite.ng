using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dl.wm.suite.cms.api.Redis.Models;
using StackExchange.Redis;

namespace dl.wm.suite.cms.api.Redis.Maps.Contracts
{
    public interface IMapsRedisRepository
    {
        Task<GeoPosition?> GetGeoPointAsync(string key, RedisValue pointMember);
        Task<bool> AddGeoPointAsync(string key, GeoEntry point);
        Task<long> AddGeofencePointAsync(string key, List<GeoEntry> points);
        Task<bool> DeleteGeoPointAsync(string key, GeoEntry point);
        Task<bool> DeleteGeoPointAsync(string key);


        Task<long> GetCountOfGeofenceEntries(string key);
        Task<RedisValue[]> GetGeofenceEntries(string key);
        Task<RedisValue> GetPointEntry(string key);

        Task<bool> AddMunicipalityAsync(string key, Guid municipalityId, string municipalityName);
        Task<bool> AddMapPointsAsync(string key, List<MapUiModel> mapUiModels);
        Task<List<MapUiModel>> GetMapPointsAsync(string key);
    }
}
