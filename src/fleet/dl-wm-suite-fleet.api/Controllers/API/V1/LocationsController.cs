using System;
using System.Threading.Tasks;
using dl.wm.suite.common.dtos.Vms.Locations;
using dl.wm.suite.common.infrastructure.PropertyMappings;
using dl.wm.suite.common.infrastructure.PropertyMappings.TypeHelpers;
using dl.wm.suite.fleet.api.Validators;
using dl.wm.suite.fleet.contracts.Locations;
using dl.wm.suite.fleet.contracts.V1;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace dl.wm.suite.fleet.api.Controllers.API.V1
{
    [Produces("application/json")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    //[Authorize(AuthenticationSchemes = "Bearer")]
    public class LocationsController : ControllerBase
    {
        public IConfiguration Configuration { get; }

        private IUrlHelper _urlHelper;
        private ITypeHelperService _typeHelperService;
        private IPropertyMappingService _propertyMappingService;
        private ICreateLocationProcessor _createLocationProcessor;
        private IInquiryLocationProcessor _inquiryLocationProcessor;

        public LocationsController(IUrlHelper urlHelper, IConfiguration configuration,
            ITypeHelperService typeHelperService, IPropertyMappingService propertyMappingService,
             ILocationsControllerDependencyBlock locationsBlock)
        {
            _urlHelper = urlHelper;
            _typeHelperService = typeHelperService;
            _propertyMappingService = propertyMappingService;

            Configuration = configuration;

            _createLocationProcessor = locationsBlock.CreateLocationProcessor;
            _inquiryLocationProcessor = locationsBlock.InquiryLocationProcessor;
        }


        /// <summary>
        /// Get location by id
        /// </summary>
        /// <param name="id">The location id</param>
        /// <returns>The location</returns>
        // GET api/locations/5
        [HttpGet("{id}", Name = "GeLocationRoot")]
        public async Task<IActionResult> GeLocationAsync(int id)
        {
            await _inquiryLocationProcessor.GetLocationAsync(id);

            return Ok();
        }

        /// <summary>
        /// Get the number of times assets have visited a location
        /// </summary>
        /// <param name="locationId">The location id</param>
        /// <returns>Dictionary containing asset names vs visit count</returns>
        //GET api/locations/321c0ef1-5430-4c10-815a-3e84e12104c4/assetsCount
        [HttpGet("{locationId}/assetsCountVisited", Name = "GetLocationVisitedByAssetsCountRoot")]
        public async Task<IActionResult> GetLocationVisitedByAssetsCountAsync(Guid locationId)
        {
            return Ok();
        }

        /// <summary>
        /// POST : Create a New Location.
        /// </summary>
        /// <param name="locationForCreationUiModel">LocationForCreationModel the Request Model for Creation</param>
        /// <remarks> return a ResponseEntity with status 201 (Created) if the new Location is created, 400 (Bad Request), 500 (Server Error) </remarks>
        /// <response code="201">Created (if the Location is created)</response>
        /// <response code="400">Bad Request</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPost(Name = "PostCreateNewLocationRoot")]
        [ValidateModel]
        public async Task<IActionResult> PostCreateNewLocationAsync([FromBody] LocationForCreationModel locationForCreationUiModel)
        {
            await _createLocationProcessor.CreateLocationAsync(locationForCreationUiModel);

            return Ok();
        }
    }
}
