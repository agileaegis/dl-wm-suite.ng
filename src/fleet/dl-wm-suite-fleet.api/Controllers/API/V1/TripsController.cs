using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dl.wm.suite.common.dtos.Links;
using dl.wm.suite.common.dtos.Vms.TrackingPoints;
using dl.wm.suite.common.dtos.Vms.Trips;
using dl.wm.suite.common.infrastructure.Extensions;
using dl.wm.suite.common.infrastructure.Helpers;
using dl.wm.suite.common.infrastructure.Helpers.ResourceParameters;
using dl.wm.suite.common.infrastructure.PropertyMappings;
using dl.wm.suite.common.infrastructure.PropertyMappings.TypeHelpers;
using dl.wm.suite.fleet.api.Controllers.API.Base;
using dl.wm.suite.fleet.api.Redis.Models;
using dl.wm.suite.fleet.api.Redis.TrackingPoints;
using dl.wm.suite.fleet.api.Validators;
using dl.wm.suite.fleet.contracts.Trips;
using dl.wm.suite.fleet.contracts.V1;
using dl.wm.suite.fleet.model.Trips;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Serilog;

namespace dl.wm.suite.fleet.api.Controllers.API.V1
{
    [Produces("application/json")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    public class TripsController : BaseController
    {
        private readonly IUrlHelper _urlHelper;
        private readonly ITrackingRedisRepository _trackingRedisRepository;
        private readonly ITypeHelperService _typeHelperService;
        private readonly IPropertyMappingService _propertyMappingService;

        private readonly IInquiryAllTripsProcessor _inquiryAllTripsProcessor;

        private readonly IInquiryTripProcessor _inquiryTripProcessor;
        private readonly ICreateTripProcessor _createTripProcessor;
        private readonly IUpdateTripProcessor _updateTripProcessor;

        public TripsController(IUrlHelper urlHelper, ITrackingRedisRepository trackingRedisRepository,
            ITypeHelperService typeHelperService, IPropertyMappingService propertyMappingService,
            ITripsControllerDependencyBlock tripBlock)
        {
            _urlHelper = urlHelper;
            _trackingRedisRepository = trackingRedisRepository;
            _typeHelperService = typeHelperService;
            _propertyMappingService = propertyMappingService;

            _inquiryAllTripsProcessor = tripBlock.InquiryAllTripsProcessor;
            _inquiryTripProcessor = tripBlock.InquiryTripProcessor;
            _createTripProcessor = tripBlock.CreateTripProcessor;
            _updateTripProcessor = tripBlock.UpdateTripProcessor;
        }


        /// <summary>
        /// POST : Create a New Trip.
        /// </summary>
        /// <param name="tripForCreationUiModel">TripForCreationUiModel the Request Model for Creation</param>
        /// <remarks> return a ResponseEntity with status 201 (Created) if the new Trip is created, 400 (Bad Request), 500 (Server Error) </remarks>
        /// <response code="201">Created (if the Trip is created)</response>
        /// <response code="400">Bad Request</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPost(Name = "PostTripRoute")]
        [ValidateModel]
        [Authorize(AuthenticationSchemes = "Bearer")]

        public async Task<IActionResult> PostTripRouteAsync([FromBody] TripForCreationUiModel tripForCreationUiModel)
        {
            var userAudit = GetEmailFromClaims();

            if (userAudit == null)
                return BadRequest();

            var newCreatedTrip =
                await _createTripProcessor.CreateTripAsync(userAudit, tripForCreationUiModel);

            switch (newCreatedTrip.Message)
            {
                case ("SUCCESS_CREATION"):
                {
                    Log.Information(
                        $"--Method:PostTripRouteAsync -- Message:TRIP_CREATION_SUCCESSFULLY -- Datetime:{DateTime.Now} -- " +
                        $"TripInfo:{tripForCreationUiModel.TripCode}");

                    return Created("PostTripRouteAsync",
                        newCreatedTrip);
                }
                case ("ERROR_TRIP_ALREADY_EXISTS"):
                {
                    Log.Error(
                        $"--Method:PostTripRouteAsync -- Message:ERROR_TRIP_ALREADY_EXISTS -- Datetime:{DateTime.UtcNow} -- " +
                        $"TripInfo:{tripForCreationUiModel.TripCode}");
                    return BadRequest(new {errorMessage = "Trip_ALREADY_EXISTS"});
                }
                case ("ERROR_TRIP_NOT_MADE_PERSISTENT"):
                {
                    Log.Error(
                        $"--Method:PostTripRouteAsync -- Message:ERROR_TRIP_NOT_MADE_PERSISTENT -- Datetime:{DateTime.UtcNow} -- " +
                        $"TripInfo:{tripForCreationUiModel.TripCode}");
                    return BadRequest(new {errorMessage = "ERROR_CREATION_NEW_TRIP"});
                }
                case ("UNKNOWN_ERROR"):
                {
                    Log.Error(
                        $"--Method:PostTripRouteAsync -- Message:ERROR_CREATE_NEW_TRIP -- Datetime:{DateTime.UtcNow} -- " +
                        $"TripInfo:{tripForCreationUiModel.TripCode}");
                    return BadRequest(new {errorMessage = "ERROR_CREATION_NEW_TRIP"});
                }
            }

            return BadRequest(new {errorMessage = "UNKNOWN_ERROR_CREATION_NEW_TRIP"});
        }



        /// <summary>
        /// POST : Register Trip with Asset and Device.
        /// </summary>
        /// <param name="code">Trip Code the Request Path for Registration</param>
        /// <param name="tripForRegistrationModel">TripForRegistrationModel the Request Model for Registration</param>
        /// <remarks> return a ResponseEntity with status 200 (Ok) if the new Trip was registered, 400 (Bad Request), 500 (Server Error) </remarks>
        /// <response code="201">Created (if the Trip is created)</response>
        /// <response code="400">Bad Request</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPut("{code}/register", Name = "PutTripRegisterRoute")]
        [ValidateModel]
        //[Authorize(AuthenticationSchemes = "Bearer", Roles = "Driver")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> PutTripRegisterRouteAsync(string code, [FromBody] TripForRegistrationModel tripForRegistrationModel)
        {
            var userAudit = GetEmailFromClaims();

            if (userAudit == null)
                return BadRequest();

            var tripToBeModified = await _updateTripProcessor.UpdateTripAsync(userAudit, new TripForModificationUiModel()
            {
                TripCode = code,
                TripAssetNumPlate = tripForRegistrationModel.AssetNumPlate,
                TripTracableVendorId = tripForRegistrationModel.TrackableVendorId
            });


            switch (tripToBeModified.Message)
            {
                case ("START_REGISTRATION"):
                {
                    Log.Information(
                        $"--Method:PutTripRegisterRouteAsync -- Message:TRIP_REGISTRATION_SUCCESSFULLY -- Datetime:{DateTime.Now} -- " +
                        $"TripInfo:{code}");

                    return Ok(tripToBeModified);
                }
                case ("ERROR_TRIP_DOES_NOT_EXIST"):
                {
                    Log.Error(
                        $"--Method:PutTripRegisterRouteAsync -- Message:ERROR_TRIP_DOES_NOT_EXIST -- Datetime:{DateTime.Now} -- " +
                        $"TripInfo:{code}");
                    return BadRequest(new {errorMessage = "ERROR_TRIP_DOES_NOT_EXIST"});
                }
                case ("ERROR_ASSET_DOES_NOT_EXIST"):
                {
                    Log.Error(
                        $"--Method:PutTripRegisterRouteAsync -- Message:ERROR_ASSET_DOES_NOT_EXIST -- Datetime:{DateTime.Now} -- " +
                        $"TripInfo:{code}");
                    return BadRequest(new {errorMessage = "ERROR_ASSET_DOES_NOT_EXIST"});
                }
                case ("ERROR_DEVICE_DOES_NOT_EXIST"):
                {
                    Log.Error(
                        $"--Method:PutTripRegisterRouteAsync -- Message:ERROR_DEVICE_DOES_NOT_EXIST -- Datetime:{DateTime.Now} -- " +
                        $"TripInfo:{code}");
                    return BadRequest(new {errorMessage = "ERROR_DEVICE_DOES_NOT_EXIST"});
                }
                case ("ERROR_DEVICE_ALREADY_TO_A_TRIP_TODAY"):
                {
                    Log.Error(
                        $"--Method:PutTripRegisterRouteAsync -- Message:ERROR_DEVICE_ALREADY_TO_A_TRIP_TODAY -- Datetime:{DateTime.Now} -- " +
                        $"TripInfo:{code}");
                    return BadRequest(new {errorMessage = "ERROR_DEVICE_ALREADY_TO_A_TRIP_TODAY"});
                }
                case ("UNKNOWN_ERROR"):
                {
                    Log.Error(
                        $"--Method:PutTripRegisterRouteAsync -- Message:ERROR_REGISTRATION_TRIP -- Datetime:{DateTime.Now} -- " +
                        $"TripInfo:{code}");
                    return BadRequest(new {errorMessage = "ERROR_REGISTRATION_TRIP"});
                }
            }

            return Ok(tripToBeModified);
        }

        /// <summary>
        /// POST : UnRegister Trip with Asset and Device.
        /// </summary>
        /// <param name="vendorId">Trackable Imei the Request Path for UnRegister</param>
        /// <remarks> return a ResponseEntity with status 200 (Ok) if the new Trip was registered, 400 (Bad Request), 500 (Server Error) </remarks>
        /// <response code="201">Created (if the Trip is created)</response>
        /// <response code="400">Bad Request</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPut("{vendorId}/unregister", Name = "PutTripUnRegisterRoute")]
        [ValidateModel]
        //[Authorize(AuthenticationSchemes = "Bearer", Roles = "Driver")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> PutTripUnRegisterRouteAsync(string vendorId)
        {
            var userAudit = GetEmailFromClaims();

            if (userAudit == null)
                return BadRequest();

            var tripToBeModified = await _updateTripProcessor.UnregisterTripAsync(userAudit, vendorId);

            switch (tripToBeModified.Message)
            {
                case ("START_UNREGISTER"):
                {
                    Log.Information(
                        $"--Method:PutTripUnRegisterRouteAsync -- Message:TRIP_UNREGISTER_SUCCESSFULLY -- Datetime:{DateTime.Now} -- " +
                        $"TripInfo:{vendorId}");

                    return Ok(tripToBeModified);
                }
                case ("UNKNOWN_ERROR"):
                {
                    Log.Error(
                        $"--Method:PutTripUnRegisterRouteAsync -- Message:ERROR_UNREGISTER_TRIP -- Datetime:{DateTime.Now} -- " +
                        $"TripInfo:{vendorId}");
                    return BadRequest(new {errorMessage = "ERROR_UNREGISTER_TRIP"});
                }
            }

            return Ok(tripToBeModified);
        }

        /// <summary>
        /// Get - Retrieve Stored Trip providing Trip Id
        /// </summary>
        /// <param name="id">Trip Id the Request Index for Retrieval</param>
        /// <param name="fields">Fiends to be filtered with for the returned Trip</param>
        /// <remarks>Retrieve Trips providing Id and [Optional] fields</remarks>
        /// <response code="200">Resource retrieved correctly</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Resource Not Found</response>
        /// <response code="500">Internal Server Error.</response>

        [HttpGet("{id}", Name = "GetTripRoot")]
        public async Task<IActionResult> GetTripAsync(int id, [FromQuery] string fields)
        {
            if (!_typeHelperService.TypeHasProperties<TripUiModel>
                (fields))
            {
                return BadRequest();
            }

            var tripFromRepo = await _inquiryTripProcessor.GetTripAsync(id);

            if (tripFromRepo == null)
            {
                return NotFound();
            }

            var trip = Mapper.Map<TripUiModel>(tripFromRepo);

            var links = CreateLinksForTrip(id, fields);

            var linkedResourceToReturn = trip.ShapeData(fields)
                as IDictionary<string, object>;

            linkedResourceToReturn.Add("links", links);

            return Ok(linkedResourceToReturn);
        }


    /// <summary>
    /// Get - Retrieve Current Trip providing Trackable-Device  VendorId
    /// </summary>
    /// <param name="vendorId">Trackable-Device VendorId the Request Index for Retrieval</param>
    /// <param name="fields">Fiends to be filtered with for the returned Trip</param>
    /// <remarks>Retrieve Trips providing Id and [Optional] fields</remarks>
    /// <response code="200">Resource retrieved correctly</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="404">Resource Not Found</response>
    /// <response code="500">Internal Server Error.</response>

    [HttpGet("trackables/{vendorId}", Name = "GetCurrentTripByVendorIdRoot")]
        public async Task<IActionResult> GetCurrentTripByVendorIdAsync(string vendorId, [FromQuery] string fields)
        {
            if (!_typeHelperService.TypeHasProperties<TripUiModel>
                (fields))
            {
                return BadRequest();
            }

            var tripFromRepo = await _inquiryTripProcessor.GetTripByTrackableVendorIdAsync(vendorId);

            if (tripFromRepo == null)
            {
                return NotFound();
            }

            var trip = Mapper.Map<TripUiModel>(tripFromRepo);

            var links = CreateLinksForTrip(tripFromRepo.Id, fields);

            var linkedResourceToReturn = trip.ShapeData(fields)
                as IDictionary<string, object>;

            linkedResourceToReturn.Add("links", links);

            return Ok(linkedResourceToReturn);
        }

        /// <summary>
        /// Get - Retrieve All/or Partial Paged Stored Trips
        /// </summary>
        /// <remarks>Retrieve paged Trips providing Paging Query</remarks>
        /// <response code="200">Resource retrieved correctly.</response>
        /// <response code="500">Internal Server Error.</response>
        [HttpGet(Name = "GetTrips")]
        public async Task<IActionResult> GetTripsAsync(
            [FromQuery] TripsResourceParameters tripsResourceParameters, [FromHeader(Name = "Accept")] string mediaType)
        {
            if (!_propertyMappingService.ValidMappingExistsFor<TripUiModel, Trip>
                (tripsResourceParameters.OrderBy))
            {
                return BadRequest();
            }

            if (!_typeHelperService.TypeHasProperties<TripUiModel>
                (tripsResourceParameters.Fields))
            {
                return BadRequest();
            }

            var tripsQueryable = await _inquiryAllTripsProcessor.GetAllTripsAsync(tripsResourceParameters);

            var trips = Mapper.Map<IEnumerable<TripUiModel>>(tripsQueryable);

            if (mediaType.Contains("application/vnd.marvin.hateoas+json"))
            {
                var paginationMetadata = new
                {
                    totalCount = tripsQueryable.TotalCount,
                    pageSize = tripsQueryable.PageSize,
                    currentPage = tripsQueryable.CurrentPage,
                    totalPages = tripsQueryable.TotalPages,
                };

                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(paginationMetadata));

                var links = CreateLinksForTrips(tripsResourceParameters,
                    tripsQueryable.HasNext, tripsQueryable.HasPrevious);

                var shapedTrips = trips.ShapeData(tripsResourceParameters.Fields);

                var shapedPersonsWithLinks = shapedTrips.Select(person =>
                {
                    var tripAsDictionary = person as IDictionary<string, object>;
                    var personLinks =
                        CreateLinksForTrip((int) tripAsDictionary["Id"],
                            tripsResourceParameters.Fields);

                    tripAsDictionary.Add("links", personLinks);

                    return tripAsDictionary;
                });

                var linkedCollectionResource = new
                {
                    value = shapedPersonsWithLinks,
                    links = links
                };

                return Ok(linkedCollectionResource);
            }
            else
            {
                var previousPageLink = tripsQueryable.HasPrevious
                    ? CreateTripsResourceUri(tripsResourceParameters,
                        ResourceUriType.PreviousPage)
                    : null;

                var nextPageLink = tripsQueryable.HasNext
                    ? CreateTripsResourceUri(tripsResourceParameters,
                        ResourceUriType.NextPage)
                    : null;

                var paginationMetadata = new
                {
                    previousPageLink = previousPageLink,
                    nextPageLink = nextPageLink,
                    totalCount = tripsQueryable.TotalCount,
                    pageSize = tripsQueryable.PageSize,
                    currentPage = tripsQueryable.CurrentPage,
                    totalPages = tripsQueryable.TotalPages
                };

                Response.Headers.Add("X-Pagination",
                    JsonConvert.SerializeObject(paginationMetadata));

                return Ok(trips.ShapeData(tripsResourceParameters.Fields));
            }
        }


        /// <summary>
        /// Detect latest trips
        /// </summary>
        /// <returns>Detected trips per asset</returns>
        [HttpGet("detect", Name = "DetectTripPerAssetAsync")]
        public async Task<IActionResult> DetectTripPerAssetAsync()
        {
            return Ok();
        }

        /// <summary>
        /// Get TrackingPoints belonging to trip nearest to specified location
        /// </summary>
        /// <param name="tripId">The trip Id</param>
        /// <param name="lat">The latitude</param>
        /// <param name="lon">The longitude</param>
        /// <param name="count">The number of points to fetch</param>
        /// <returns></returns>
        // GET api/trips/321c0ef1-5430-4c10-815a-3e84e12104c4/points?lat=x&lon=y
        [HttpGet("{tripId}/points", Name = "GetTrackingPointsToNearestLocationRoot")]
        public async Task<IActionResult> GetTrackingPointsToNearestLocationAsync(Guid tripId, double lat, double lon,
            int count = 5)
        {
            return Ok();
        }


        /// <summary>
        /// Add TrackingPoints related to Trackable Device, Asset and Trip
        /// </summary>
        /// <param name="id">The Trip id</param>
        /// <param name="points">List of TrackingPoints</param>
        /// <returns>Ok response</returns>
        [HttpPost("{id}/points", Name = "PostPointsToTrackableRoot")]
        [ValidateModel]
        [Authorize]

        public async Task<IActionResult> PostPointsToTrackableAsync(int id, [FromBody] TrackingPointDto[] points)
        {
            var userAudit = GetEmailFromClaims();

            if (userAudit == null)
                return BadRequest();

            var tripToBeUpdated = await _updateTripProcessor.CreateTrtackingPoints(userAudit, id, points);

            return Ok(tripToBeUpdated);
        }


        /// <summary>
        /// Add TrackingPoint related to Trackable Device, Asset and Trip
        /// </summary>
        /// <param name="id">The Trip id</param>
        /// <param name="point">TrackingPoint</param>
        /// <returns>Ok response</returns>
        [HttpPost("{id}/point", Name = "PostPointToTrackableRoot")]
        [ValidateModel]
        [Authorize]

        public async Task<IActionResult> PostPointToTrackableAsync(int id, [FromBody] TrackingPointDto point)
        {
            var userAudit = GetEmailFromClaims();

            if (userAudit == null)
                return BadRequest();

            var tripToBeUpdated = await _updateTripProcessor.CreateTrtackingPoint(userAudit, id, point);

            return Ok(tripToBeUpdated);
        }

        /// <summary>
        /// Get TrackingPoint related to Trip
        /// </summary>
        /// <param name="id">The Trip id</param>
        /// <param name="point">TrackingPoint</param>
        /// <returns>Ok response</returns>
        [HttpGet("{id}/point-2", Name = "GetRedisPointToTrackableRoot")]
        [Authorize]

        public async Task<IActionResult> GetRedisPointToTrackableAsync(int id)
        {
            var userAudit = GetEmailFromClaims();

            if (userAudit == null)
                return BadRequest();

            var storedPointInRedisForTrip = await _trackingRedisRepository.GetTrackingPointAsync(id.ToString());

            return Ok(storedPointInRedisForTrip);
        }

        /// <summary>
        /// Register TrackingPoint related to Trip
        /// </summary>
        /// <param name="id">The Trip id</param>
        /// <param name="point">TrackingPoint</param>
        /// <returns>Ok response</returns>
        // POST api/devices/5/points
        [HttpPost("{id}/point-2", Name = "PostRedisPointToTrackableRoot")]
        [ValidateModel]
        [Authorize]

        public async Task<IActionResult> PostRedisPointToTrackableAsync(int id, [FromBody] TrackingPointDto point)
        {
            var userAudit = GetEmailFromClaims();

            if (userAudit == null)
                return BadRequest();

            TrackingPointRedisModel trackingPointRedisModel = new TrackingPointRedisModel()
            {
                Audit = userAudit,
                Time = point.Time,
                Longitude = point.Longitude,
                Latitude = point.Latitude,
                Provider = point.Provider,
                LocationProvider = point.LocationProvider,
                Accuracy = point.Accuracy,
                Speed = point.Speed,
                Altitude = point.Altitude,
                Course = point.Course,
            };

            var pointToBeStoredInRedisForTrip =
                await _trackingRedisRepository.AddTrackingPointAsync(id.ToString(), trackingPointRedisModel);

            return pointToBeStoredInRedisForTrip
                ? Ok("POINT_STORAGE_SUCCEEDED")
                : (IActionResult) BadRequest("POINT_STORAGE_FAILED");
        }


        #region Link Builder

        private IEnumerable<LinkDto> CreateLinksForTrip(int id, string fields)
        {
            var links = new List<LinkDto>();

            if (String.IsNullOrWhiteSpace(fields))
            {
                links.Add(
                    new LinkDto(_urlHelper.Link("GetTrip", new {id = id}),
                        "self",
                        "GET"));
            }
            else
            {
                links.Add(
                    new LinkDto(_urlHelper.Link("GetTrip", new {id = id, fields = fields}),
                        "self",
                        "GET"));
            }

            return links;
        }


        private IEnumerable<LinkDto> CreateLinksForTrips(TripsResourceParameters tripsResourceParameters, bool hasNext,
            bool hasPrevious)
        {
            var links = new List<LinkDto>
            {
                new LinkDto(CreateTripsResourceUri(tripsResourceParameters,
                        ResourceUriType.Current)
                    , "self", "GET")
            };

            if (hasNext)
            {
                links.Add(
                    new LinkDto(CreateTripsResourceUri(tripsResourceParameters,
                            ResourceUriType.NextPage),
                        "nextPage", "GET"));
            }

            if (hasPrevious)
            {
                links.Add(
                    new LinkDto(CreateTripsResourceUri(tripsResourceParameters,
                            ResourceUriType.PreviousPage),
                        "previousPage", "GET"));
            }

            return links;
        }

        private string CreateTripsResourceUri(TripsResourceParameters tripsResourceParameters, ResourceUriType type)
        {
            switch (type)
            {
                case ResourceUriType.PreviousPage:
                    return _urlHelper.Link("GetTrips",
                        new
                        {
                            fields = tripsResourceParameters.Fields,
                            orderBy = tripsResourceParameters.OrderBy,
                            searchQuery = tripsResourceParameters.SearchQuery,
                            pageNumber = tripsResourceParameters.PageIndex - 1,
                            pageSize = tripsResourceParameters.PageSize
                        });
                case ResourceUriType.NextPage:
                    return _urlHelper.Link("GetTrips",
                        new
                        {
                            fields = tripsResourceParameters.Fields,
                            orderBy = tripsResourceParameters.OrderBy,
                            searchQuery = tripsResourceParameters.SearchQuery,
                            pageNumber = tripsResourceParameters.PageIndex + 1,
                            pageSize = tripsResourceParameters.PageSize
                        });
                case ResourceUriType.Current:
                default:
                    return _urlHelper.Link("GetTrips",
                        new
                        {
                            fields = tripsResourceParameters.Fields,
                            orderBy = tripsResourceParameters.OrderBy,
                            searchQuery = tripsResourceParameters.SearchQuery,
                            pageNumber = tripsResourceParameters.PageIndex,
                            pageSize = tripsResourceParameters.PageSize
                        });
            }
        }

        #endregion
    }
}
