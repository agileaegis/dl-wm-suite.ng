using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using dl.wm.suite.cms.api.Controllers.API.Base;
using dl.wm.suite.cms.api.Validators;
using dl.wm.suite.cms.contracts.Trackables;
using dl.wm.suite.cms.contracts.V1;
using dl.wm.suite.cms.model.Tours.Trackables;
using dl.wm.suite.common.dtos.Links;
using dl.wm.suite.common.dtos.Vms.Trackables;
using dl.wm.suite.common.infrastructure.Extensions;
using dl.wm.suite.common.infrastructure.Helpers;
using dl.wm.suite.common.infrastructure.Helpers.ResourceParameters;
using dl.wm.suite.common.infrastructure.PropertyMappings;
using dl.wm.suite.common.infrastructure.PropertyMappings.TypeHelpers;
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
  public class TrackablesController : BaseController
  {
    private readonly IUrlHelper _urlHelper;
    private readonly ITypeHelperService _typeHelperService;
    private readonly IPropertyMappingService _propertyMappingService;

    private readonly IInquiryAllTrackablesProcessor _inquiryAllTrackablesProcessor;

    private readonly IInquiryTrackableProcessor _inquiryTrackableProcessor;
    private readonly ICreateTrackableProcessor _createTrackableProcessor;

    public TrackablesController(IUrlHelper urlHelper,
        ITypeHelperService typeHelperService, IPropertyMappingService propertyMappingService,
        ITrackablesControllerDependencyBlock trackableBlock)
    {
      _urlHelper = urlHelper;
      _typeHelperService = typeHelperService;
      _propertyMappingService = propertyMappingService;

      _inquiryAllTrackablesProcessor = trackableBlock.InquiryAllTrackablesProcessor;
      _inquiryTrackableProcessor = trackableBlock.InquiryTrackableProcessor;
      _createTrackableProcessor = trackableBlock.CreateTrackableProcessor;
    }

    /// <summary>
    /// POST : Create a New Trackable.
    /// </summary>
    /// <param name="trackableForCreationUiModel">TrackableForCreationUiModel the Request Model for Creation</param>
    /// <remarks> return a ResponseEntity with status 201 (Created) if the new Trackable is created, 400 (Bad Request), 500 (Server Error) </remarks>
    /// <response code="201">Created (if the Trackable is created)</response>
    /// <response code="400">Bad Request</response>
    /// <response code="500">Internal Server Error</response>
    [HttpPost(Name = "PostTrackableRoute")]
    [ValidateModel]
    [Authorize]
    public async Task<IActionResult> PostTrackableRouteAsync(
        [FromBody] TrackableForCreationUiModel trackableForCreationUiModel)
    {
      var userAudit = GetEmailFromClaims();

      if (userAudit == null)
        return BadRequest();

      var newCreatedTrackable =
          await _createTrackableProcessor.CreateTrackableAsync(userAudit, trackableForCreationUiModel);

      switch (newCreatedTrackable.Message)
      {
        case ("SUCCESS_CREATION"):
          {
            Log.Information(
                $"--Method:PostTrackableRouteAsync -- Message:TRACKABLE_CREATION_SUCCESSFULLY -- Datetime:{DateTime.Now} -- " +
                $"TrackableInfo:{trackableForCreationUiModel.TrackableImei}");

            return Created("PostTrackableRouteAsync",
                newCreatedTrackable);
          }
        case ("ERROR_TACKABLE_ALREADY_EXISTS"):
          {
            Log.Error(
                $"--Method:PostTrackableRouteAsync -- Message:ERROR_TRACKABLE_ALREADY_EXISTS -- Datetime:{DateTime.UtcNow} -- " +
                $"TrackableInfo:{trackableForCreationUiModel.TrackableImei}");
            return BadRequest(new { errorMessage = "TRACKABLE_ALREADY_EXISTS" });
          }
        case ("ERROR_TRACKABLE_NOT_MADE_PERSISTENT"):
          {
            Log.Error(
                $"--Method:PostTrackableRouteAsync -- Message:ERROR_TRACKABLE_NOT_MADE_PERSISTENT -- Datetime:{DateTime.UtcNow} -- " +
                $"TrackableInfo:{trackableForCreationUiModel.TrackableImei}");
            return BadRequest(new { errorMessage = "ERROR_CREATION_NEW_TRACKABLE" });
          }
        case ("UNKNOWN_ERROR"):
          {
            Log.Error(
                $"--Method:PostTrackableRouteAsync -- Message:ERROR_CREATE_NEW_TRACKABLE -- Datetime:{DateTime.UtcNow} -- " +
                $"TrackableInfo:{trackableForCreationUiModel.TrackableImei}");
            return BadRequest(new { errorMessage = "ERROR_CREATION_NEW_TRACKABLE" });
          }
      }

      return BadRequest(new { errorMessage = "UNKNOWN_ERROR_CREATION_NEW_TRACKABLE" });
    }


    /// <summary>
    /// PUT : Update an Existing Trackable.
    /// </summary>
    /// <param name="id">Trackable Id for Modification</param>
    /// <param name="TrackableForModificationUiModel">TrackableForCreationUiModel the Request Model for Modification</param>
    /// <remarks> return a ResponseEntity with status 201 (Created) if the new Trackable is created, 400 (Bad Request), 500 (Server Error) </remarks>
    /// <response code="200">Ok (if the Trackable is updated)</response>
    /// <response code="400">Bad Request</response>
    /// <response code="500">Internal Server Error</response>
    [HttpPut("{id}", Name = "PutTrackableRoute")]
    [ValidateModel]
    [Authorize]
    public async Task<IActionResult> PutTrackableRouteAsync(Guid id,
        [FromBody] TrackableForModificationUiModel TrackableForModificationUiModel)
    {
      return BadRequest(new { errorMessage = "UNKNOWN_ERROR_UPDATE_Trackable" });
    }

    /// <summary>
    /// Get - Retrieve Stored Trackable providing Trackable Id
    /// </summary>
    /// <param name="id">Trackable Id the Request Index for Retrieval</param>
    /// <param name="fields">Fiends to be filtered with for the returned Trackable</param>
    /// <remarks>Retrieve Trackables providing Id and [Optional] fields</remarks>
    /// <response code="200">Resource retrieved correctly</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="404">Resource Not Found</response>
    /// <response code="500">Internal Server Error.</response>

    [HttpGet("{id}", Name = "GetTrackableRoot")]
    public async Task<IActionResult> GetTrackableAsync(Guid id, [FromQuery] string fields)
    {
      if (!_typeHelperService.TypeHasProperties<TrackableUiModel>
          (fields))
      {
        return BadRequest();
      }

      var trackableFromRepo = await _inquiryTrackableProcessor.GetTrackableAsync(id);

      if (trackableFromRepo == null)
      {
        return NotFound();
      }

      var trackable = Mapper.Map<TrackableUiModel>(trackableFromRepo);

      var links = CreateLinksForTrackable(id, fields);

      var linkedResourceToReturn = trackable.ShapeData(fields)
          as IDictionary<string, object>;

      linkedResourceToReturn.Add("links", links);

      return Ok(linkedResourceToReturn);
    }

    /// <summary>
    /// Get - Retrieve Stored Trackable providing Trackable Imei
    /// </summary>
    /// <param name="imei">Trackable Imei the Request Index for Retrieval</param>
    /// <param name="fields">Fiends to be filtered with for the returned Trackable</param>
    /// <remarks>Retrieve Trackables providing Id and [Optional] fields</remarks>
    /// <response code="200">Resource retrieved correctly</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="404">Resource Not Found</response>
    /// <response code="500">Internal Server Error.</response>

    [HttpGet("device/{imei}", Name = "GetTrackableImeiRoot")]
    public async Task<IActionResult> GetTrackableImeiAsync(string imei, [FromQuery] string fields)
    {
      if (!_typeHelperService.TypeHasProperties<TrackableUiModel>
          (fields))
      {
        return BadRequest();
      }

      var trackableFromRepo = await _inquiryTrackableProcessor.GetTrackableByImeiAsync(imei);

      if (trackableFromRepo == null)
      {
        return NotFound();
      }

      var trackable = Mapper.Map<TrackableUiModel>(trackableFromRepo);

      var links = CreateLinksForTrackable(trackable.Id, fields);

      var linkedResourceToReturn = trackable.ShapeData(fields)
          as IDictionary<string, object>;

      linkedResourceToReturn.Add("links", links);

      return Ok(linkedResourceToReturn);
    }


    /// <summary>
    /// Get - Retrieve All/or Partial Paged Stored Trackables
    /// </summary>
    /// <remarks>Retrieve paged Trackables providing Paging Query</remarks>
    /// <response code="200">Resource retrieved correctly.</response>
    /// <response code="500">Internal Server Error.</response>
    [HttpGet(Name = "GetTrackables")]
    public async Task<IActionResult> GetTrackablesAsync(
        [FromQuery] TrackablesResourceParameters trackablesResourceParameters,
        [FromHeader(Name = "Accept")] string mediaType)
    {
      if (!_propertyMappingService.ValidMappingExistsFor<TrackableUiModel, Trackable>
          (trackablesResourceParameters.OrderBy))
      {
        return BadRequest();
      }

      if (!_typeHelperService.TypeHasProperties<TrackableUiModel>
          (trackablesResourceParameters.Fields))
      {
        return BadRequest();
      }

      var trackablesQueryable =
          await _inquiryAllTrackablesProcessor.GetAllPagedTrackablesAsync(trackablesResourceParameters);

      var trackables = Mapper.Map<IEnumerable<TrackableUiModel>>(trackablesQueryable);

      if (mediaType.Contains("application/vnd.marvin.hateoas+json"))
      {
        var paginationMetadata = new
        {
          totalCount = trackablesQueryable.TotalCount,
          pageSize = trackablesQueryable.PageSize,
          currentPage = trackablesQueryable.CurrentPage,
          totalPages = trackablesQueryable.TotalPages,
        };

        Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(paginationMetadata));

        var links = CreateLinksForTrackables(trackablesResourceParameters,
            trackablesQueryable.HasNext, trackablesQueryable.HasPrevious);

        var shapedTrackables = trackables.ShapeData(trackablesResourceParameters.Fields);

        var shapedTrackablesWithLinks = shapedTrackables.Select(trackable =>
        {
          var trackableAsDictionary = trackable as IDictionary<string, object>;
          var trackableLinks =
              CreateLinksForTrackable((Guid)trackableAsDictionary["Id"],
                  trackablesResourceParameters.Fields);

          trackableAsDictionary.Add("links", trackableLinks);

          return trackableAsDictionary;
        });

        var linkedCollectionResource = new
        {
          value = shapedTrackablesWithLinks,
          links = links
        };

        return Ok(linkedCollectionResource);
      }
      else
      {
        var previousPageLink = trackablesQueryable.HasPrevious
            ? CreateTrackablesResourceUri(trackablesResourceParameters,
                ResourceUriType.PreviousPage)
            : null;

        var nextPageLink = trackablesQueryable.HasNext
            ? CreateTrackablesResourceUri(trackablesResourceParameters,
                ResourceUriType.NextPage)
            : null;

        var paginationMetadata = new
        {
          previousPageLink = previousPageLink,
          nextPageLink = nextPageLink,
          totalCount = trackablesQueryable.TotalCount,
          pageSize = trackablesQueryable.PageSize,
          currentPage = trackablesQueryable.CurrentPage,
          totalPages = trackablesQueryable.TotalPages
        };

        Response.Headers.Add("X-Pagination",
            JsonConvert.SerializeObject(paginationMetadata));

        return Ok(trackables.ShapeData(trackablesResourceParameters.Fields));
      }
    }

    #region Link Builder

    private IEnumerable<LinkDto> CreateLinksForTrackable(Guid id, string fields)
    {
      var links = new List<LinkDto>();

      if (String.IsNullOrWhiteSpace(fields))
      {
        links.Add(
            new LinkDto(_urlHelper.Link("GetTrackable", new { id = id }),
                "self",
                "GET"));
      }
      else
      {
        links.Add(
            new LinkDto(_urlHelper.Link("GetTrackable", new { id = id, fields = fields }),
                "self",
                "GET"));
      }

      return links;
    }


    private IEnumerable<LinkDto> CreateLinksForTrackables(
        TrackablesResourceParameters TrackablesResourceParameters,
        bool hasNext, bool hasPrevious)
    {
      var links = new List<LinkDto>
            {
                new LinkDto(CreateTrackablesResourceUri(TrackablesResourceParameters,
                        ResourceUriType.Current)
                    , "self", "GET")
            };

      if (hasNext)
      {
        links.Add(
            new LinkDto(CreateTrackablesResourceUri(TrackablesResourceParameters,
                    ResourceUriType.NextPage),
                "nextPage", "GET"));
      }

      if (hasPrevious)
      {
        links.Add(
            new LinkDto(CreateTrackablesResourceUri(TrackablesResourceParameters,
                    ResourceUriType.PreviousPage),
                "previousPage", "GET"));
      }

      return links;
    }

    private string CreateTrackablesResourceUri(
        TrackablesResourceParameters TrackablesResourceParameters,
        ResourceUriType type)
    {
      switch (type)
      {
        case ResourceUriType.PreviousPage:
          return _urlHelper.Link("GetTrackables",
              new
              {
                fields = TrackablesResourceParameters.Fields,
                orderBy = TrackablesResourceParameters.OrderBy,
                searchQuery = TrackablesResourceParameters.SearchQuery,
                pageNumber = TrackablesResourceParameters.PageIndex - 1,
                pageSize = TrackablesResourceParameters.PageSize
              });
        case ResourceUriType.NextPage:
          return _urlHelper.Link("GetTrackables",
              new
              {
                fields = TrackablesResourceParameters.Fields,
                orderBy = TrackablesResourceParameters.OrderBy,
                searchQuery = TrackablesResourceParameters.SearchQuery,
                pageNumber = TrackablesResourceParameters.PageIndex + 1,
                pageSize = TrackablesResourceParameters.PageSize
              });
        case ResourceUriType.Current:
        default:
          return _urlHelper.Link("GetTrackables",
              new
              {
                fields = TrackablesResourceParameters.Fields,
                orderBy = TrackablesResourceParameters.OrderBy,
                searchQuery = TrackablesResourceParameters.SearchQuery,
                pageNumber = TrackablesResourceParameters.PageIndex,
                pageSize = TrackablesResourceParameters.PageSize
              });
      }
    }

    #endregion
  }
}
