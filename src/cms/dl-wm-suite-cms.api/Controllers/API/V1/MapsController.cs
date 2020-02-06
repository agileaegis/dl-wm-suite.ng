using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using dl.wm.suite.cms.api.Controllers.API.Base;
using dl.wm.suite.cms.api.Redis.Maps.Contracts;
using dl.wm.suite.cms.api.Redis.Models;
using dl.wm.suite.cms.api.Redis.Models.VirtualEarths;
using dl.wm.suite.cms.api.Validators;
using dl.wm.suite.common.infrastructure.Exceptions.Controllers.Maps;
using dl.wm.suite.common.infrastructure.PropertyMappings;
using dl.wm.suite.common.infrastructure.PropertyMappings.TypeHelpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RestSharp;
using StackExchange.Redis;

namespace dl.wm.suite.cms.api.Controllers.API.V1
{
  [Produces("application/json")]
  [ApiVersion("1.0")]
  [ResponseCache(Duration = 0, NoStore = true, VaryByHeader = "*")]
  [Route("api/v{version:apiVersion}/[controller]")]
  [ApiController]
  //[Authorize]
  public class MapsController : BaseController
  {
    public IConfiguration Configuration { get; }
    private readonly IUrlHelper _urlHelper;
    private readonly ITypeHelperService _typeHelperService;
    private readonly IPropertyMappingService _propertyMappingService;
    private readonly IMapsRedisRepository _mapsRepository;

    private readonly IHostingEnvironment _environment;


    public MapsController(IHostingEnvironment environment, IConfiguration configuration, IUrlHelper urlHelper,
      ITypeHelperService typeHelperService, IPropertyMappingService propertyMappingService,
      IMapsRedisRepository mapsRedisRepository)
    {
      _environment = environment ?? throw new ArgumentNullException(nameof(environment));

      Configuration = configuration;

      _urlHelper = urlHelper;
      _typeHelperService = typeHelperService;
      _propertyMappingService = propertyMappingService;

      _mapsRepository = mapsRedisRepository;
    }


    [HttpPost("municipalities/{municipalityName}/{municipalityId}", Name = "PostMunicipalityRoot")]
    public async Task<IActionResult> PostMunicipalityAsync([Required] string municipalityName,
      [Required] Guid municipalityId)
    {
      var municipalityCreation =
        await _mapsRepository.AddMunicipalityAsync("municipalities", municipalityId, municipalityName);
      return Ok(municipalityCreation);
    }


    [HttpPost("geofence/generate/{geofenceId}", Name = "PostGeofenceGenerateRoot")]
    public async Task<IActionResult> PostGeofenceGenerateAsync(string geofenceId)
    {
      var geofencePoints = new List<GeoEntry>();

      if (string.IsNullOrWhiteSpace(_environment.WebRootPath))
      {
        _environment.WebRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
        _environment.WebRootPath = Directory.GetCurrentDirectory();
      }

      var geoFenceJsonsPath = _environment.WebRootPath + "/geofence/";

      using (StreamReader r = new StreamReader($"{geoFenceJsonsPath}{geofenceId}.json"))
      {
        string json = r.ReadToEnd();
        JsonParser items = JsonConvert.DeserializeObject<JsonParser>(json);

        if (items == null)
        {
          return BadRequest("NOTHING_GEOFENCE_GENERATE_FAILED");
        }
        else
        {
          foreach (var itemGeometry in items.geometries)
          {
            foreach (var itemGeometryCoordinate in itemGeometry.coordinates)
            {
              GeoEntry itemGeoEntryToBeAdded = new GeoEntry(itemGeometryCoordinate[0], itemGeometryCoordinate[1],
                GetLocationByCoordinates(itemGeometryCoordinate[1], itemGeometryCoordinate[0]));
              geofencePoints.Add(itemGeoEntryToBeAdded);
            }
          }
        }
      }

      try
      {
        var geoPointCreation = await _mapsRepository.AddGeofencePointAsync(geofenceId, geofencePoints);
        Created("PostGeofenceGenerateAsync", geoPointCreation);
      }
      catch (Exception e)
      {
        BadRequest("CREATION_GEOFENCE_FAILED");
      }

      return Ok(geofencePoints);
    }

    [HttpGet("address/{latitude}/{longitude}", Name = "GetPointAddressRoot")]
    public async Task<IActionResult> GetPointAddressAsync(double latitude, double longitude)
    {
      return Ok(GetLocationByCoordinates(latitude, longitude));
    }

    //52ddb6a7-396d-4193-8664-2b90b27bb19f
    [HttpGet("geofence/{geofenceId}", Name = "GetGeofencePointsRoot")]
    public async Task<IActionResult> GetGeofencePointsAsync(string geofenceId)
    {
      var geofenceStoredPointCount = await _mapsRepository.GetCountOfGeofenceEntries(geofenceId);
      List<GeoPosition> geofenceStoredPoints = new List<GeoPosition>();

      if (geofenceStoredPointCount >= 0)
      {
        var geofenceStoredRedisValues = await _mapsRepository.GetGeofenceEntries(geofenceId);
        foreach (var geofenceStoredRedisValue in geofenceStoredRedisValues)
        {
          var geoStoredPoint = await _mapsRepository.GetGeoPointAsync(geofenceId, geofenceStoredRedisValue);
          if (geoStoredPoint != null)
          {
            geofenceStoredPoints.Add((GeoPosition) geoStoredPoint);
          }
        }
      }

      return Ok(geofenceStoredPoints);
    }

    //52ddb6a7-396d-4193-8664-2b90b27bb190
    [HttpGet("geofenceMaps/{geofenceId}", Name = "GetGeofenceMapPointsRoot")]
    public async Task<IActionResult> GetGeofenceMapPointsAsync(string geofenceId)
    {
      var geofenceStoredPointCount = await _mapsRepository.GetMapPointsAsync(geofenceId);

      return Ok(geofenceStoredPointCount);
    }

    [HttpGet("point/{pointId}", Name = "GetPointRoot")]
    public async Task<IActionResult> GetPointAsync(string pointId)
    {
      var geoStoredPoint = await _mapsRepository.GetPointEntry(pointId);
      GeoPosition? geoPointCreation;
      if (!geoStoredPoint.IsNullOrEmpty)
      {
        geoPointCreation = await _mapsRepository.GetGeoPointAsync(pointId, geoStoredPoint);
        if (geoPointCreation == null)
        {
          return BadRequest("RETRIEVE_POINT_FAILED");
        }
      }
      else
      {
        return BadRequest("RETRIEVE_POINT_FAILED");
      }


      return Ok(geoPointCreation);
    }


    [HttpPost("point", Name = "PostPointRoot")]
    [ValidateModel]
    public async Task<IActionResult> PostPointAsync([FromBody] GeoEntryForCreation geoPointForCreation)
    {
      string physicalAddr = String.Empty;
      try
      {
        physicalAddr =
          GetLocationByCoordinates(geoPointForCreation.GeoPoint.Lat, geoPointForCreation.GeoPoint.Long);
      }
      catch (GeolocationNotFound ex)
      {
        BadRequest("CREATION_POINT_FAILED");
      }
      catch (Exception e)
      {
        BadRequest("CREATION_POINT_FAILED");
      }

      var geoPointCreation = await _mapsRepository.AddGeoPointAsync(geoPointForCreation.PointId, new GeoEntry(
        geoPointForCreation.GeoPoint.Long,
        geoPointForCreation.GeoPoint.Lat, physicalAddr));

      return geoPointCreation
        ? (IActionResult) Created("PostPointRoot", "CREATION_POINT_SUCCESS")
        : BadRequest("CREATION_POINT_FAILED");
    }

    [HttpPost("geofence", Name = "PostGeofencePointsRoot")]
    [ValidateModel]
    public async Task<IActionResult> PostGeofencePointsAsync([FromBody] GeofenceForCreation geofenceForCreation)
    {
      var geofencePoints = new List<GeoEntry>();

      foreach (var geoEntryUiModel in geofenceForCreation.GeoPoints)
      {
        geofencePoints.Add(new GeoEntry(geoEntryUiModel.Long, geoEntryUiModel.Lat, geoEntryUiModel.Name));
      }

      try
      {
        var geoPointCreation = await _mapsRepository.AddGeofencePointAsync(geofenceForCreation.PointId, geofencePoints);
        Created("PostGeofencePointsRoot", "CREATION_GEOFENCE_SUCCESS");
      }
      catch (Exception e)
      {
        BadRequest("CREATION_GEOFENCE_FAILED");
      }

      return Ok();
    }

    [HttpPut("geofence/{geofenceId}", Name = "PutGeofencePointsRoot")]
    //[ValidateModel]
    public async Task<IActionResult> PutGeofencePointsAsync(string geofenceId,
      [FromBody] GeofenceForModification geofenceForModification)
    {
      var geofencePoints = new List<GeoEntry>();

      foreach (var mapPoint in geofenceForModification.GeofenceMapPointForModification)
      {
        GeoEntry itemGeoEntryToBeAdded = new GeoEntry(mapPoint.Longitude, mapPoint.Latitude,
          GetLocationByCoordinates(mapPoint.Latitude, mapPoint.Longitude));
        geofencePoints.Add(itemGeoEntryToBeAdded);
      }

      try
      {
        var geoPointCreation = await _mapsRepository.AddGeofencePointAsync(geofenceId, geofencePoints);
        var geoMapPointStored = await _mapsRepository.AddMapPointsAsync($"{geofenceId}-s",
          geofenceForModification.GeofenceMapPointForModification);

        if (geoMapPointStored)
          Ok(new {geoPointCreation, geofencePoints});

        BadRequest("MODIFICATION_GEOFENCE_FAILED");
      }
      catch (Exception e)
      {
        BadRequest("MODIFICATION_GEOFENCE_FAILED");
      }

      return Ok(geofencePoints);
    }

    private string GetLocationByCoordinates(double lat, double lng)
    {
      string url =
        $"http://dev.virtualearth.net/REST/v1/Locations/{lat},{lng}?o=json&c=el&key={Configuration["BingMapsAPIKey"]}";

      var client = new RestClient(url);
      var request = new RestRequest(Method.GET);

      VirtualEarth result = new VirtualEarth();

      request.AddHeader("Content-Type", "application/json");

      var response = client.Execute(request);
      if (response.IsSuccessful)
      {
        result = JsonConvert.DeserializeObject<VirtualEarth>(response.Content);
        return $"{result.resourceSets[0]?.resources.FirstOrDefault()?.address.addressLine} - " +
               $"{result.resourceSets[0]?.resources.FirstOrDefault()?.address.postalCode}";
      }

      throw new GeolocationNotFound();
    }
  }
}
