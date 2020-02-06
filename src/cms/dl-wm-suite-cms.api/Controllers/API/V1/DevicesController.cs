using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dl.wm.suite.cms.api.Controllers.API.Base;
using dl.wm.suite.cms.api.Validators;
using dl.wm.suite.cms.contracts.Devices;
using dl.wm.suite.cms.model.Devices;
using dl.wm.suite.common.dtos.Links;
using dl.wm.suite.common.dtos.Vms.Devices;
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
    public class DevicesController : BaseController
    {
        private readonly IUrlHelper _urlHelper;
        private readonly ITypeHelperService _typeHelperService;
        private readonly IPropertyMappingService _propertyMappingService;

        private readonly IInquiryAllDevicesProcessor _inquiryAllDevicesProcessor;

        private readonly IInquiryDeviceProcessor _inquiryDeviceProcessor;
        private readonly ICreateDeviceProcessor _createDeviceProcessor;
        private readonly IUpdateDeviceProcessor _updateDeviceProcessor;

        public DevicesController(IUrlHelper urlHelper,
            ITypeHelperService typeHelperService, IPropertyMappingService propertyMappingService,
            IInquiryAllDevicesProcessor inquiryAllDevicesProcessor,
            IInquiryDeviceProcessor inquiryDeviceProcessor,
            ICreateDeviceProcessor createDeviceProcessor,
            IUpdateDeviceProcessor updateDeviceProcessor
            )
        {
            _urlHelper = urlHelper;
            _typeHelperService = typeHelperService;
            _propertyMappingService = propertyMappingService;

            _inquiryAllDevicesProcessor = inquiryAllDevicesProcessor;
            _inquiryDeviceProcessor = inquiryDeviceProcessor;
            _createDeviceProcessor = createDeviceProcessor;
            _updateDeviceProcessor = updateDeviceProcessor;
        }

        /// <summary>
        /// POST : Create a New Device.
        /// </summary>
        /// <param name="deviceForCreationUiModel">DeviceForCreationUiModel the Request Model for Creation</param>
        /// <remarks> return a ResponseEntity with status 201 (Created) if the new Device is created, 400 (Bad Request), 500 (Server Error) </remarks>
        /// <response code="201">Created (if the Device is created)</response>
        /// <response code="400">Bad Request</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPost(Name = "PostDeviceRoute")]
        [ValidateModel]
        public async Task<IActionResult> PostDeviceRouteAsync([FromBody] DeviceForCreationUiModel deviceForCreationUiModel)
        {
            return BadRequest(new {errorMessage = "UNKNOWN_ERROR_CREATION_NEW_DEVICE"});
        }


        /// <summary>
        /// PUT : Update an Existing Device.
        /// </summary>
        /// <param name="id">Device Id for Modification</param>
        /// <param name="deviceForModificationUiModel">DeviceForCreationUiModel the Request Model for Modification</param>
        /// <remarks> return a ResponseEntity with status 201 (Created) if the new Device is created, 400 (Bad Request), 500 (Server Error) </remarks>
        /// <response code="200">Ok (if the Device is updated)</response>
        /// <response code="400">Bad Request</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPut("{id}", Name = "PutDeviceRoute")]
        [ValidateModel]
        public async Task<IActionResult> PutDeviceRouteAsync(Guid  id, [FromBody] DeviceForModificationUiModel deviceForModificationUiModel)
        {
            return BadRequest(new { errorMessage = "UNKNOWN_ERROR_UPDATE_DEVICE" });
        }

        /// <summary>
        /// Get - Retrieve Stored Device providing Device Id
        /// </summary>
        /// <param name="id">Device Id the Request Index for Retrieval</param>
        /// <param name="fields">Fiends to be filtered with for the returned Device</param>
        /// <remarks>Retrieve Devices providing Id and [Optional] fields</remarks>
        /// <response code="200">Resource retrieved correctly</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Resource Not Found</response>
        /// <response code="500">Internal Server Error.</response>

        [HttpGet("{id}", Name = "GetDeviceRoot")]
        public async Task<IActionResult> GetDeviceAsync(Guid id, [FromQuery] string fields)
        {
            if (!_typeHelperService.TypeHasProperties<DeviceUiModel>
                (fields))
            {
                return BadRequest();
            }

            var deviceFromRepo = await _inquiryDeviceProcessor.GetDeviceAsync(id);

            if (deviceFromRepo == null)
            {
                return NotFound();
            }

            var device = Mapper.Map<DeviceUiModel>(deviceFromRepo);

            var links = CreateLinksForDevice(id, fields);

            var linkedResourceToReturn = device.ShapeData(fields)
                as IDictionary<string, object>;

            linkedResourceToReturn.Add("links", links);

            return Ok(linkedResourceToReturn);
        }

        /// <summary>
        /// Get - Retrieve All/or Partial Paged Stored Devices
        /// </summary>
        /// <remarks>Retrieve paged Devices providing Paging Query</remarks>
        /// <response code="200">Resource retrieved correctly.</response>
        /// <response code="500">Internal Server Error.</response>
        [HttpGet(Name = "GetDevices")]
        public async Task<IActionResult> GetDevicesAsync(
            [FromQuery] DevicesResourceParameters devicesResourceParameters,
            [FromHeader(Name = "Accept")] string mediaType)
        {
            if (!_propertyMappingService.ValidMappingExistsFor<DeviceUiModel, Device>
                (devicesResourceParameters.OrderBy))
            {
                return BadRequest();
            }

            if (!_typeHelperService.TypeHasProperties<DeviceUiModel>
                (devicesResourceParameters.Fields))
            {
                return BadRequest();
            }

            //return Ok(Devices.ShapeData(DevicesResourceParameters.Fields));
            return Ok();
        }
    

        #region Link Builder

        private IEnumerable<LinkDto> CreateLinksForDevice(Guid id, string fields)
        {
            var links = new List<LinkDto>();

            if (String.IsNullOrWhiteSpace(fields))
            {
                links.Add(
                    new LinkDto(_urlHelper.Link("GetDevice", new {id = id}),
                        "self",
                        "GET"));
            }
            else
            {
                links.Add(
                    new LinkDto(_urlHelper.Link("GetDevice", new {id = id, fields = fields}),
                        "self",
                        "GET"));
            }

            return links;
        }


        private IEnumerable<LinkDto> CreateLinksForDevices(
            DevicesResourceParameters devicesResourceParameters,
            bool hasNext, bool hasPrevious)
        {
            var links = new List<LinkDto>
            {
                new LinkDto(CreateDevicesResourceUri(devicesResourceParameters,
                        ResourceUriType.Current)
                    , "self", "GET")
            };

            if (hasNext)
            {
                links.Add(
                    new LinkDto(CreateDevicesResourceUri(devicesResourceParameters,
                            ResourceUriType.NextPage),
                        "nextPage", "GET"));
            }

            if (hasPrevious)
            {
                links.Add(
                    new LinkDto(CreateDevicesResourceUri(devicesResourceParameters,
                            ResourceUriType.PreviousPage),
                        "previousPage", "GET"));
            }

            return links;
        }

        private string CreateDevicesResourceUri(
            DevicesResourceParameters devicesResourceParameters,
            ResourceUriType type)
        {
            switch (type)
            {
                case ResourceUriType.PreviousPage:
                    return _urlHelper.Link("GetDevices",
                        new
                        {
                            fields = devicesResourceParameters.Fields,
                            orderBy = devicesResourceParameters.OrderBy,
                            searchQuery = devicesResourceParameters.SearchQuery,
                            pageNumber = devicesResourceParameters.PageIndex - 1,
                            pageSize = devicesResourceParameters.PageSize
                        });
                case ResourceUriType.NextPage:
                    return _urlHelper.Link("GetDevices",
                        new
                        {
                            fields = devicesResourceParameters.Fields,
                            orderBy = devicesResourceParameters.OrderBy,
                            searchQuery = devicesResourceParameters.SearchQuery,
                            pageNumber = devicesResourceParameters.PageIndex + 1,
                            pageSize = devicesResourceParameters.PageSize
                        });
                case ResourceUriType.Current:
                default:
                    return _urlHelper.Link("GetDevices",
                        new
                        {
                            fields = devicesResourceParameters.Fields,
                            orderBy = devicesResourceParameters.OrderBy,
                            searchQuery = devicesResourceParameters.SearchQuery,
                            pageNumber = devicesResourceParameters.PageIndex,
                            pageSize = devicesResourceParameters.PageSize
                        });
            }
        }

        #endregion
    }
}
