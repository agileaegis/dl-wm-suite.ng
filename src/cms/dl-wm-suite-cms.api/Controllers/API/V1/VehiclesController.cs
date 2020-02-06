using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dl.wm.suite.cms.api.Controllers.API.Base;
using dl.wm.suite.cms.api.Validators;
using dl.wm.suite.cms.contracts.Vehicles;
using dl.wm.suite.cms.model.Vehicles;
using dl.wm.suite.common.dtos.Links;
using dl.wm.suite.common.dtos.Vms.Vehicles;
using dl.wm.suite.common.infrastructure.Extensions;
using dl.wm.suite.common.infrastructure.Helpers;
using dl.wm.suite.common.infrastructure.Helpers.ResourceParameters;
using dl.wm.suite.common.infrastructure.PropertyMappings;
using dl.wm.suite.common.infrastructure.PropertyMappings.TypeHelpers;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Serilog;

namespace dl.wm.suite.cms.api.Controllers.API.V1
{
    [Produces("application/json")]
    [ApiVersion("1.0")]
    [ResponseCache(Duration = 0, NoStore = true, VaryByHeader = "*")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize]
    public class VehiclesController : BaseController
    {
        private readonly IUrlHelper _urlHelper;
        private readonly ITypeHelperService _typeHelperService;
        private readonly IPropertyMappingService _propertyMappingService;

        private readonly IInquiryAllVehiclesProcessor _inquiryAllVehiclesProcessor;

        private readonly IInquiryVehicleProcessor _inquiryVehicleProcessor;
        private readonly ICreateVehicleProcessor _createVehicleProcessor;
        private readonly IUpdateVehicleProcessor _updateVehicleProcessor;

        public VehiclesController(IUrlHelper urlHelper,
            ITypeHelperService typeHelperService, IPropertyMappingService propertyMappingService,
            IInquiryAllVehiclesProcessor inquiryAllVehiclesProcessor,
            IInquiryVehicleProcessor inquiryVehicleProcessor,
            ICreateVehicleProcessor createVehicleProcessor,
            IUpdateVehicleProcessor updateVehicleProcessor
            )
        {
            _urlHelper = urlHelper;
            _typeHelperService = typeHelperService;
            _propertyMappingService = propertyMappingService;

            _inquiryAllVehiclesProcessor = inquiryAllVehiclesProcessor;
            _inquiryVehicleProcessor = inquiryVehicleProcessor;
            _createVehicleProcessor = createVehicleProcessor;
            _updateVehicleProcessor = updateVehicleProcessor;
        }

        /// <summary>
        /// POST : Create a New Vehicle.
        /// </summary>
        /// <param name="vehicleForCreationUiModel">VehicleForCreationUiModel the Request Model for Creation</param>
        /// <remarks> return a ResponseEntity with status 201 (Created) if the new Vehicle is created, 400 (Bad Request), 500 (Server Error) </remarks>
        /// <response code="201">Created (if the Vehicle is created)</response>
        /// <response code="400">Bad Request</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPost(Name = "PostVehicleRoute")]
        [ValidateModel]
        public async Task<IActionResult> PostVehicleRouteAsync([FromBody] VehicleForCreationUiModel vehicleForCreationUiModel)
        {
            var request = this.Request;

            var newCreatedVehicle =
                await _createVehicleProcessor.CreateVehicleAsync(vehicleForCreationUiModel);

            switch (newCreatedVehicle.Message)
            {
                case ("SUCCESS_CREATION"):
                {
                    Log.Information(
                        $"--Method:PostVehicleRouteAsync -- Message:VEHICLE_CREATION_SUCCESSFULLY -- Datetime:{DateTime.Now} -- VehicleInfo:{vehicleForCreationUiModel.VehicleNumPlate}");

                    return Created("PostVehicleRouteAsync",
                        newCreatedVehicle);
                }
                case ("ERROR_VEHICLE_ALREADY_EXISTS"):
                {
                    Log.Error(
                        $"--Method:PostVehicleRouteAsync -- Message:ERROR_VEHICLE_ALREADY_EXISTS -- Datetime:{DateTime.UtcNow} -- VehicleInfo:{vehicleForCreationUiModel.VehicleNumPlate}");
                    return BadRequest(new {errorMessage = "VEHICLE_ALREADY_EXISTS"});
                }
                case ("ERROR_Vehicle_NOT_MADE_PERSISTENT"):
                {
                    Log.Error(
                        $"--Method:PostVehicleRouteAsync -- Message:ERROR_VEHICLE_NOT_MADE_PERSISTENT -- Datetime:{DateTime.UtcNow} -- VehicleInfo:{vehicleForCreationUiModel.VehicleNumPlate}");
                    return BadRequest(new {errorMessage = "ERROR_CREATION_NEW_VEHICLE"});
                }
                case ("UNKNOWN_ERROR"):
                {
                    Log.Error(
                        $"--Method:PostVehicleRouteAsync -- Message:ERROR_REGISTER_NEW_USER -- Datetime:{DateTime.UtcNow} -- VehicleInfo:{vehicleForCreationUiModel.VehicleNumPlate}");
                    return BadRequest(new {errorMessage = "ERROR_CREATION_NEW_VEHICLE"});
                }
            }

            return BadRequest(new {errorMessage = "UNKNOWN_ERROR_CREATION_NEW_VEHICLE"});
        }


        /// <summary>
        /// PUT : Update an Existing Vehicle.
        /// </summary>
        /// <param name="id">Vehicle Id for Modification</param>
        /// <param name="vehicleForModificationUiModel">VehicleForCreationUiModel the Request Model for Modification</param>
        /// <remarks> return a ResponseEntity with status 201 (Created) if the new Vehicle is created, 400 (Bad Request), 500 (Server Error) </remarks>
        /// <response code="200">Ok (if the Vehicle is updated)</response>
        /// <response code="400">Bad Request</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPut("{id}", Name = "PutVehicleRoute")]
        [ValidateModel]
        public async Task<IActionResult> PutVehicleRouteAsync(Guid  id, [FromBody] VehicleForModificationUiModel vehicleForModificationUiModel)
        {
            var updatedVehicle = await  _updateVehicleProcessor.UpdateVehicleAsync(id, vehicleForModificationUiModel);

            switch (updatedVehicle.Message)
            {
                case ("SUCCESS_UPDATE"):
                {
                    Log.Information(
                        $"--Method:PutVehicleRouteAsync -- Message:VEHICLE_UPDATE_SUCCESSFULLY -- Datetime:{DateTime.Now} -- VehicleInfo:{vehicleForModificationUiModel.VehicleNumPlate}");

                    return Created("PutVehicleRouteAsync",
                        updatedVehicle);
                }
                case ("ERROR_VEHICLE_NOT_EXIST"):
                {
                    Log.Error(
                        $"--Method:PutVehicleRouteAsync -- Message:ERROR_VEHICLE_NOT_EXISTS -- Datetime:{DateTime.UtcNow} -- VehicleInfo:{vehicleForModificationUiModel.VehicleNumPlate}");
                    return BadRequest(new { errorMessage = "ERROR_VEHICLE_NOT_EXIST" });
                }
                case ("ERROR_INVALID_VEHICLE_MODEL"):
                {
                    Log.Error(
                        $"--Method:PutVehicleRouteAsync -- Message:ERROR_INVALID_VEHICLE_MODEL -- Datetime:{DateTime.UtcNow} -- VehicleInfo:{vehicleForModificationUiModel.VehicleNumPlate}");
                    return BadRequest(new { errorMessage = "ERROR_INVALID_VEHICLE_MODEL" });
                }
                case ("ERROR_VEHICLE_NOT_MADE_PERSISTENT"):
                {
                    Log.Error(
                        $"--Method:PutVehicleRouteAsync -- Message:ERROR_VEHICLE_NOT_MADE_PERSISTENT -- Datetime:{DateTime.UtcNow} -- VehicleInfo:{vehicleForModificationUiModel.VehicleNumPlate}");
                    return BadRequest(new { errorMessage = "ERROR_VEHICLE_NOT_MADE_PERSISTENT" });
                }
                case ("ERROR_VEHICLE_ALREADY_EXISTS"):
                {
                    Log.Error(
                        $"--Method:PutVehicleRouteAsync -- Message:ERROR_VEHICLE_ALREADY_EXISTS -- Datetime:{DateTime.UtcNow} -- VehicleInfo:{vehicleForModificationUiModel.VehicleNumPlate}");
                    return BadRequest(new { errorMessage = "ERROR_VEHICLE_ALREADY_EXISTS" });
                }
                case ("UNKNOWN_ERROR"):
                {
                    Log.Error(
                        $"--Method:PutVehicleRouteAsync -- Message:ERROR_REGISTER_NEW_USER -- Datetime:{DateTime.UtcNow} -- VehicleInfo:{vehicleForModificationUiModel.VehicleNumPlate}");
                    return BadRequest(new { errorMessage = "ERROR_CREATION_NEW_VEHICLE" });
                }
            }

            return BadRequest(new { errorMessage = "UNKNOWN_ERROR_UPDATE_VEHICLE" });
        }

        /// <summary>
        /// Get - Retrieve Stored Vehicle providing Vehicle Id
        /// </summary>
        /// <param name="id">Vehicle Id the Request Index for Retrieval</param>
        /// <param name="fields">Fiends to be filtered with for the returned Vehicle</param>
        /// <remarks>Retrieve Vehicles providing Id and [Optional] fields</remarks>
        /// <response code="200">Resource retrieved correctly</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Resource Not Found</response>
        /// <response code="500">Internal Server Error.</response>

        [HttpGet("{id}", Name = "GetVehicle")]
        public async Task<IActionResult> GetVehicleAsync(Guid id, [FromQuery] string fields)
        {
            if (!_typeHelperService.TypeHasProperties<VehicleUiModel>
                (fields))
            {
                return BadRequest();
            }

            var vehicleFromRepo = await _inquiryVehicleProcessor.GetVehicleAsync(id);

            if (vehicleFromRepo == null)
            {
                return NotFound();
            }

            var vehicle = Mapper.Map<VehicleUiModel>(vehicleFromRepo);

            var links = CreateLinksForVehicle(id, fields);

            var linkedResourceToReturn = vehicle.ShapeData(fields)
                as IDictionary<string, object>;

            linkedResourceToReturn.Add("links", links);

            return Ok(linkedResourceToReturn);
        }

        /// <summary>
        /// Get - Retrieve All/or Partial Paged Stored Vehicles
        /// </summary>
        /// <remarks>Retrieve paged Vehicles providing Paging Query</remarks>
        /// <response code="200">Resource retrieved correctly.</response>
        /// <response code="500">Internal Server Error.</response>
        [HttpGet(Name = "GetVehicles")]
        public async Task<IActionResult> GetVehiclesAsync(
            [FromQuery] VehiclesResourceParameters vehiclesResourceParameters,
            [FromHeader(Name = "Accept")] string mediaType)
        {
            if (!_propertyMappingService.ValidMappingExistsFor<VehicleUiModel, Vehicle>
                (vehiclesResourceParameters.OrderBy))
            {
                return BadRequest();
            }

            if (!_typeHelperService.TypeHasProperties<VehicleUiModel>
                (vehiclesResourceParameters.Fields))
            {
                return BadRequest();
            }

            var vehiclesQueryable =
                await _inquiryAllVehiclesProcessor.GetAllActivePagedVehiclesAsync(vehiclesResourceParameters);

            var vehicles = Mapper.Map<IEnumerable<VehicleUiModel>>(vehiclesQueryable);

            if (mediaType.Contains("application/vnd.marvin.hateoas+json"))
            {
                var paginationMetadata = new
                {
                    totalCount = vehiclesQueryable.TotalCount,
                    pageSize = vehiclesQueryable.PageSize,
                    currentPage = vehiclesQueryable.CurrentPage,
                    totalPages = vehiclesQueryable.TotalPages,
                };

                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(paginationMetadata));

                var links = CreateLinksForVehicles(vehiclesResourceParameters,
                    vehiclesQueryable.HasNext, vehiclesQueryable.HasPrevious);

                var shapedPersons = vehicles.ShapeData(vehiclesResourceParameters.Fields);

                var shapedPersonsWithLinks = shapedPersons.Select(person =>
                {
                    var personAsDictionary = person as IDictionary<string, object>;
                    var personLinks =
                        CreateLinksForVehicle((Guid) personAsDictionary["Id"],
                            vehiclesResourceParameters.Fields);

                    personAsDictionary.Add("links", personLinks);

                    return personAsDictionary;
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
                var previousPageLink = vehiclesQueryable.HasPrevious
                    ? CreateVehiclesResourceUri(vehiclesResourceParameters,
                        ResourceUriType.PreviousPage)
                    : null;

                var nextPageLink = vehiclesQueryable.HasNext
                    ? CreateVehiclesResourceUri(vehiclesResourceParameters,
                        ResourceUriType.NextPage)
                    : null;

                var paginationMetadata = new
                {
                    previousPageLink = previousPageLink,
                    nextPageLink = nextPageLink,
                    totalCount = vehiclesQueryable.TotalCount,
                    pageSize = vehiclesQueryable.PageSize,
                    currentPage = vehiclesQueryable.CurrentPage,
                    totalPages = vehiclesQueryable.TotalPages
                };

                Response.Headers.Add("X-Pagination",
                    JsonConvert.SerializeObject(paginationMetadata));

                return Ok(vehicles.ShapeData(vehiclesResourceParameters.Fields));
            }
        }

        #region Link Builder

        private IEnumerable<LinkDto> CreateLinksForVehicle(Guid id, string fields)
        {
            var links = new List<LinkDto>();

            if (String.IsNullOrWhiteSpace(fields))
            {
                links.Add(
                    new LinkDto(_urlHelper.Link("GetVehicle", new {id = id}),
                        "self",
                        "GET"));
            }
            else
            {
                links.Add(
                    new LinkDto(_urlHelper.Link("GetVehicle", new {id = id, fields = fields}),
                        "self",
                        "GET"));
            }

            return links;
        }


        private IEnumerable<LinkDto> CreateLinksForVehicles(
            VehiclesResourceParameters vehiclesResourceParameters,
            bool hasNext, bool hasPrevious)
        {
            var links = new List<LinkDto>
            {
                new LinkDto(CreateVehiclesResourceUri(vehiclesResourceParameters,
                        ResourceUriType.Current)
                    , "self", "GET")
            };

            if (hasNext)
            {
                links.Add(
                    new LinkDto(CreateVehiclesResourceUri(vehiclesResourceParameters,
                            ResourceUriType.NextPage),
                        "nextPage", "GET"));
            }

            if (hasPrevious)
            {
                links.Add(
                    new LinkDto(CreateVehiclesResourceUri(vehiclesResourceParameters,
                            ResourceUriType.PreviousPage),
                        "previousPage", "GET"));
            }

            return links;
        }

        private string CreateVehiclesResourceUri(
            VehiclesResourceParameters vehiclesResourceParameters,
            ResourceUriType type)
        {
            switch (type)
            {
                case ResourceUriType.PreviousPage:
                    return _urlHelper.Link("GetVehicles",
                        new
                        {
                            fields = vehiclesResourceParameters.Fields,
                            orderBy = vehiclesResourceParameters.OrderBy,
                            searchQuery = vehiclesResourceParameters.SearchQuery,
                            pageNumber = vehiclesResourceParameters.PageIndex - 1,
                            pageSize = vehiclesResourceParameters.PageSize
                        });
                case ResourceUriType.NextPage:
                    return _urlHelper.Link("GetVehicles",
                        new
                        {
                            fields = vehiclesResourceParameters.Fields,
                            orderBy = vehiclesResourceParameters.OrderBy,
                            searchQuery = vehiclesResourceParameters.SearchQuery,
                            pageNumber = vehiclesResourceParameters.PageIndex + 1,
                            pageSize = vehiclesResourceParameters.PageSize
                        });
                case ResourceUriType.Current:
                default:
                    return _urlHelper.Link("GetVehicles",
                        new
                        {
                            fields = vehiclesResourceParameters.Fields,
                            orderBy = vehiclesResourceParameters.OrderBy,
                            searchQuery = vehiclesResourceParameters.SearchQuery,
                            pageNumber = vehiclesResourceParameters.PageIndex,
                            pageSize = vehiclesResourceParameters.PageSize
                        });
            }
        }

        #endregion
    }
}
