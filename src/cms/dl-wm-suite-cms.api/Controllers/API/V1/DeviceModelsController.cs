using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dl.wm.suite.cms.api.Controllers.API.Base;
using dl.wm.suite.cms.api.Validators;
using dl.wm.suite.cms.contracts.Devices.DeviceModels;
using dl.wm.suite.cms.model.Devices;
using dl.wm.suite.common.dtos.Links;
using dl.wm.suite.common.dtos.Vms.Devices.DeviceModels;
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
    public class DeviceModelsController : BaseController
    {
        private readonly IUrlHelper _urlHelper;
        private readonly ITypeHelperService _typeHelperService;
        private readonly IPropertyMappingService _propertyMappingService;

        private readonly IInquiryAllDeviceModelsProcessor _inquiryAllDeviceModelsProcessor;

        private readonly IInquiryDeviceModelProcessor _inquiryDeviceModelProcessor;
        private readonly ICreateDeviceModelProcessor _createDeviceModelProcessor;
        private readonly IUpdateDeviceModelProcessor _updateDeviceModelProcessor;

        public DeviceModelsController(IUrlHelper urlHelper,
            ITypeHelperService typeHelperService, IPropertyMappingService propertyMappingService,
            IInquiryAllDeviceModelsProcessor inquiryAllDeviceModelsProcessor,
            IInquiryDeviceModelProcessor inquiryDeviceModelProcessor,
            ICreateDeviceModelProcessor createDeviceModelProcessor,
            IUpdateDeviceModelProcessor updateDeviceModelProcessor
            )
        {
            _urlHelper = urlHelper;
            _typeHelperService = typeHelperService;
            _propertyMappingService = propertyMappingService;

            _inquiryAllDeviceModelsProcessor = inquiryAllDeviceModelsProcessor;
            _inquiryDeviceModelProcessor = inquiryDeviceModelProcessor;
            _createDeviceModelProcessor = createDeviceModelProcessor;
            _updateDeviceModelProcessor = updateDeviceModelProcessor;
        }

        /// <summary>
        /// POST : Create a New DeviceModel.
        /// </summary>
        /// <param name="deviceForCreationUiModel">DeviceModelForCreationUiModel the Request Model for Creation</param>
        /// <remarks> return a ResponseEntity with status 201 (Created) if the new DeviceModel is created, 400 (Bad Request), 500 (Server Error) </remarks>
        /// <response code="201">Created (if the DeviceModel is created)</response>
        /// <response code="400">Bad Request</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPost(Name = "PostDeviceModelRoute")]
        [ValidateModel]
        public async Task<IActionResult> PostDeviceModelRouteAsync([FromBody] DeviceModelForCreationUiModel deviceForCreationUiModel)
        {
            return BadRequest(new {errorMessage = "UNKNOWN_ERROR_CREATION_NEW_DEVICE"});
        }


        /// <summary>
        /// PUT : Update an Existing DeviceModel.
        /// </summary>
        /// <param name="id">DeviceModel Id for Modification</param>
        /// <param name="deviceForModificationUiModel">DeviceModelForCreationUiModel the Request Model for Modification</param>
        /// <remarks> return a ResponseEntity with status 201 (Created) if the new DeviceModel is created, 400 (Bad Request), 500 (Server Error) </remarks>
        /// <response code="200">Ok (if the DeviceModel is updated)</response>
        /// <response code="400">Bad Request</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPut("{id}", Name = "PutDeviceModelRoute")]
        [ValidateModel]
        public async Task<IActionResult> PutDeviceModelRouteAsync(Guid  id, [FromBody] DeviceModelForModificationUiModel deviceForModificationUiModel)
        {
            return BadRequest(new { errorMessage = "UNKNOWN_ERROR_UPDATE_DEVICE" });
        }

        /// <summary>
        /// Get - Retrieve Stored DeviceModel providing DeviceModel Id
        /// </summary>
        /// <param name="id">DeviceModel Id the Request Index for Retrieval</param>
        /// <param name="fields">Fiends to be filtered with for the returned DeviceModel</param>
        /// <remarks>Retrieve DeviceModels providing Id and [Optional] fields</remarks>
        /// <response code="200">Resource retrieved correctly</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Resource Not Found</response>
        /// <response code="500">Internal Server Error.</response>

        [HttpGet("{id}", Name = "GetDeviceModelRoot")]
        public async Task<IActionResult> GetDeviceModelAsync(Guid id, [FromQuery] string fields)
        {
            if (!_typeHelperService.TypeHasProperties<DeviceModelUiModel>
                (fields))
            {
                return BadRequest();
            }

            var deviceFromRepo = await _inquiryDeviceModelProcessor.GetDeviceModelAsync(id);

            if (deviceFromRepo == null)
            {
                return NotFound();
            }

            var device = Mapper.Map<DeviceModelUiModel>(deviceFromRepo);

            var links = CreateLinksForDeviceModel(id, fields);

            var linkedResourceToReturn = device.ShapeData(fields)
                as IDictionary<string, object>;

            linkedResourceToReturn.Add("links", links);

            return Ok(linkedResourceToReturn);
        }

        /// <summary>
        /// Get - Retrieve All/or Partial Paged Stored DeviceModels
        /// </summary>
        /// <remarks>Retrieve paged DeviceModels providing Paging Query</remarks>
        /// <response code="200">Resource retrieved correctly.</response>
        /// <response code="500">Internal Server Error.</response>
        [HttpGet(Name = "GetDeviceModels")]
        public async Task<IActionResult> GetDeviceModelsAsync(
            [FromQuery] DeviceModelsResourceParameters devicesResourceParameters,
            [FromHeader(Name = "Accept")] string mediaType)
        {
            if (!_propertyMappingService.ValidMappingExistsFor<DeviceModelUiModel, DeviceModel>
                (devicesResourceParameters.OrderBy))
            {
                return BadRequest();
            }

            if (!_typeHelperService.TypeHasProperties<DeviceModelUiModel>
                (devicesResourceParameters.Fields))
            {
                return BadRequest();
            }

            //return Ok(DeviceModels.ShapeData(DeviceModelsResourceParameters.Fields));
            return Ok();
        }
    

        #region Link Builder

        private IEnumerable<LinkDto> CreateLinksForDeviceModel(Guid id, string fields)
        {
            var links = new List<LinkDto>();

            if (String.IsNullOrWhiteSpace(fields))
            {
                links.Add(
                    new LinkDto(_urlHelper.Link("GetDeviceModel", new {id = id}),
                        "self",
                        "GET"));
            }
            else
            {
                links.Add(
                    new LinkDto(_urlHelper.Link("GetDeviceModel", new {id = id, fields = fields}),
                        "self",
                        "GET"));
            }

            return links;
        }


        private IEnumerable<LinkDto> CreateLinksForDeviceModels(
            DeviceModelsResourceParameters devicesResourceParameters,
            bool hasNext, bool hasPrevious)
        {
            var links = new List<LinkDto>
            {
                new LinkDto(CreateDeviceModelsResourceUri(devicesResourceParameters,
                        ResourceUriType.Current)
                    , "self", "GET")
            };

            if (hasNext)
            {
                links.Add(
                    new LinkDto(CreateDeviceModelsResourceUri(devicesResourceParameters,
                            ResourceUriType.NextPage),
                        "nextPage", "GET"));
            }

            if (hasPrevious)
            {
                links.Add(
                    new LinkDto(CreateDeviceModelsResourceUri(devicesResourceParameters,
                            ResourceUriType.PreviousPage),
                        "previousPage", "GET"));
            }

            return links;
        }

        private string CreateDeviceModelsResourceUri(
            DeviceModelsResourceParameters devicesResourceParameters,
            ResourceUriType type)
        {
            switch (type)
            {
                case ResourceUriType.PreviousPage:
                    return _urlHelper.Link("GetDeviceModels",
                        new
                        {
                            fields = devicesResourceParameters.Fields,
                            orderBy = devicesResourceParameters.OrderBy,
                            searchQuery = devicesResourceParameters.SearchQuery,
                            pageNumber = devicesResourceParameters.PageIndex - 1,
                            pageSize = devicesResourceParameters.PageSize
                        });
                case ResourceUriType.NextPage:
                    return _urlHelper.Link("GetDeviceModels",
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
                    return _urlHelper.Link("GetDeviceModels",
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
