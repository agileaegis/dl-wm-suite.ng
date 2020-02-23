using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dl.wm.suite.cms.api.Controllers.API.Base;
using dl.wm.suite.cms.api.Validators;
using dl.wm.suite.cms.contracts.Tours;
using dl.wm.suite.cms.contracts.Users;
using dl.wm.suite.cms.contracts.V1;
using dl.wm.suite.cms.model.Tours;
using dl.wm.suite.common.dtos.Links;
using dl.wm.suite.common.dtos.Vms.Tours;
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
  public class ToursController : BaseController
  {
    private readonly IUrlHelper _urlHelper;
    private readonly ITypeHelperService _typeHelperService;
    private readonly IPropertyMappingService _propertyMappingService;

    private readonly IInquiryAllToursProcessor _inquiryAllToursProcessor;

    private readonly IInquiryTourProcessor _inquiryTourProcessor;
    private readonly ICreateTourProcessor _createTourProcessor;
    private readonly IUpdateTourProcessor _updateTourProcessor;

    private readonly IInquiryUserProcessor _inquiryUserProcessor;


    public ToursController(IUrlHelper urlHelper,
      ITypeHelperService typeHelperService, IPropertyMappingService propertyMappingService,
      IInquiryAllToursProcessor inquiryAllToursProcessor,
      IInquiryTourProcessor inquiryTourProcessor,
      ICreateTourProcessor createTourProcessor,
      IUpdateTourProcessor updateTourProcessor,
      IUsersControllerDependencyBlock blockUser
    )
    {
      _urlHelper = urlHelper;
      _typeHelperService = typeHelperService;
      _propertyMappingService = propertyMappingService;

      _inquiryAllToursProcessor = inquiryAllToursProcessor;
      _inquiryTourProcessor = inquiryTourProcessor;
      _createTourProcessor = createTourProcessor;
      _updateTourProcessor = updateTourProcessor;

      _inquiryUserProcessor = blockUser.InquiryUserProcessor;
    }

    /// <summary>
    /// POST : Create a New Tour.
    /// </summary>
    /// <param name="tourForCreationUiModel">TourForCreationUiModel the Request Model for Creation</param>
    /// <remarks> return a ResponseEntity with status 201 (Created) if the new Tour is created, 400 (Bad Request), 500 (Server Error) </remarks>
    /// <response code="201">Created (if the Tour is created)</response>
    /// <response code="400">Bad Request</response>
    /// <response code="500">Internal Server Error</response>
    [HttpPost(Name = "PostTourRoute")]
    [ValidateModel]
    public async Task<IActionResult> PostTourRouteAsync([FromBody] TourForCreationUiModel tourForCreationUiModel)
    {
      var userAudit = await _inquiryUserProcessor.GetUserByLoginAsync(GetEmailFromClaims());

      if (userAudit == null)
        return BadRequest();

      var newCreatedTour =
        await _createTourProcessor.CreateTourAsync(userAudit.Id, tourForCreationUiModel);

      switch (newCreatedTour.Message)
      {
        case ("SUCCESS_CREATION"):
        {
          Log.Information(
            $"--Method:PostTourRouteAsync -- Message:TOUR_CREATION_SUCCESSFULLY -- " +
            $"Datetime:{DateTime.Now} -- TourInfo:{tourForCreationUiModel.TourName}");
          return Created(nameof(PostTourRouteAsync), newCreatedTour);
        }
        case ("ERROR_ALREADY_EXISTS"):
        {
          Log.Error(
            $"--Method:PostTourRouteAsync -- Message:ERROR_TOUR_ALREADY_EXISTS -- " +
            $"Datetime:{DateTime.Now} -- TourInfo:{tourForCreationUiModel.TourName}");
          return BadRequest(new {errorMessage = "TOUR_ALREADY_EXISTS"});
        }
        case ("ERROR_TOUR_NOT_MADE_PERSISTENT"):
        {
          Log.Error(
            $"--Method:PostTourRouteAsync -- Message:ERROR_TOUR_NOT_MADE_PERSISTENT -- " +
            $"Datetime:{DateTime.Now} -- ContainerInfo:{tourForCreationUiModel.TourName}");
          return BadRequest(new {errorMessage = "ERROR_CREATION_NEW_TOUR"});
        }
        case ("UNKNOWN_ERROR"):
        {
          Log.Error(
            $"--Method:PostContainerRouteAsync -- Message:ERROR_CREATION_NEW_TOUR -- " +
            $"Datetime:{DateTime.Now} -- TourInfo:{tourForCreationUiModel.TourName}");
          return BadRequest(new {errorMessage = "ERROR_CREATION_NEW_TOUR"});
        }
      }

      return NotFound();
    }


    /// <summary>
    /// PUT : Update an Existing Tour.
    /// </summary>
    /// <param name="id">Tour Id for Modification</param>
    /// <param name="tourForModificationUiModel">TourForCreationUiModel the Request Model for Modification</param>
    /// <remarks> return a ResponseEntity with status 201 (Created) if the new Tour is created, 400 (Bad Request), 500 (Server Error) </remarks>
    /// <response code="200">Ok (if the Tour is updated)</response>
    /// <response code="400">Bad Request</response>
    /// <response code="500">Internal Server Error</response>
    [HttpPut("{id}", Name = "PutTourRoute")]
    [ValidateModel]
    public async Task<IActionResult> PutTourRouteAsync(Guid id,
      [FromBody] TourForModificationUiModel tourForModificationUiModel)
    {
      return BadRequest(new {errorMessage = "UNKNOWN_ERROR_UPDATE_TOUR"});
    }

    /// <summary>
    /// Get - Retrieve Stored Tour providing Tour Id
    /// </summary>
    /// <param name="id">Tour Id the Request Index for Retrieval</param>
    /// <param name="fields">Fiends to be filtered with for the returned Tour</param>
    /// <remarks>Retrieve Tours providing Id and [Optional] fields</remarks>
    /// <response code="200">Resource retrieved correctly</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="404">Resource Not Found</response>
    /// <response code="500">Internal Server Error.</response>

    [HttpGet("{id}", Name = "GetTourRoot")]
    public async Task<IActionResult> GetTourAsync(Guid id, [FromQuery] string fields)
    {
      if (!_typeHelperService.TypeHasProperties<TourUiModel>
        (fields))
      {
        return BadRequest();
      }

      var TourFromRepo = await _inquiryTourProcessor.GetTourAsync(id);

      if (TourFromRepo == null)
      {
        return NotFound();
      }

      var Tour = Mapper.Map<TourUiModel>(TourFromRepo);

      var links = CreateLinksForTour(id, fields);

      var linkedResourceToReturn = Tour.ShapeData(fields)
        as IDictionary<string, object>;

      linkedResourceToReturn.Add("links", links);

      return Ok(linkedResourceToReturn);
    }

    /// <summary>
    /// Get - Retrieve Stored Tour providing Assigned Tour
    /// </summary>
    /// <param name="id">Tour Id the Request Index for Retrieval</param>
    /// <param name="fields">Fiends to be filtered with for the returned Tour</param>
    /// <remarks>Retrieve Tours providing Id and [Optional] fields</remarks>
    /// <response code="200">Resource retrieved correctly</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="404">Resource Not Found</response>
    /// <response code="500">Internal Server Error.</response>

    [HttpGet("assign", Name = "GetTourAssignedRoot")]
    public async Task<IActionResult> GetTourAssignedAsync([FromQuery] string fields)
    {
      if (!_typeHelperService.TypeHasProperties<TourUiModel>
        (fields))
      {
        return BadRequest();
      }

      var userAudit = await _inquiryUserProcessor.GetUserByLoginAsync(GetEmailFromClaims());

      if (userAudit == null)
        return BadRequest();

      var tourFromRepo = await _inquiryTourProcessor.GetTodayAssignedTourAsync(userAudit.Id);

      if (tourFromRepo == null)
      {
        return NotFound();
      }


      return Ok(tourFromRepo);
    }

    /// <summary>
    /// Get - Retrieve All/or Partial Paged Stored Tours
    /// </summary>
    /// <remarks>Retrieve paged Tours providing Paging Query</remarks>
    /// <response code="200">Resource retrieved correctly.</response>
    /// <response code="500">Internal Server Error.</response>
    [HttpGet(Name = "GetTours")]
    public async Task<IActionResult> GetToursAsync(
      [FromQuery] ToursResourceParameters toursResourceParameters,
      [FromHeader(Name = "Accept")] string mediaType)
    {
      if (!_propertyMappingService.ValidMappingExistsFor<TourUiModel, Tour>
        (toursResourceParameters.OrderBy))
      {
        return BadRequest();
      }

      if (!_typeHelperService.TypeHasProperties<TourUiModel>
        (toursResourceParameters.Fields))
      {
        return BadRequest();
      }

      //return Ok(Tours.ShapeData(ToursResourceParameters.Fields));
      return Ok();
    }


    #region Link Builder

    private IEnumerable<LinkDto> CreateLinksForTour(Guid id, string fields)
    {
      var links = new List<LinkDto>();

      if (String.IsNullOrWhiteSpace(fields))
      {
        links.Add(
          new LinkDto(_urlHelper.Link("GetTour", new {id = id}),
            "self",
            "GET"));
      }
      else
      {
        links.Add(
          new LinkDto(_urlHelper.Link("GetTour", new {id = id, fields = fields}),
            "self",
            "GET"));
      }

      return links;
    }


    private IEnumerable<LinkDto> CreateLinksForTours(
      ToursResourceParameters toursResourceParameters,
      bool hasNext, bool hasPrevious)
    {
      var links = new List<LinkDto>
      {
        new LinkDto(CreateToursResourceUri(toursResourceParameters,
            ResourceUriType.Current)
          , "self", "GET")
      };

      if (hasNext)
      {
        links.Add(
          new LinkDto(CreateToursResourceUri(toursResourceParameters,
              ResourceUriType.NextPage),
            "nextPage", "GET"));
      }

      if (hasPrevious)
      {
        links.Add(
          new LinkDto(CreateToursResourceUri(toursResourceParameters,
              ResourceUriType.PreviousPage),
            "previousPage", "GET"));
      }

      return links;
    }

    private string CreateToursResourceUri(
      ToursResourceParameters toursResourceParameters,
      ResourceUriType type)
    {
      switch (type)
      {
        case ResourceUriType.PreviousPage:
          return _urlHelper.Link("GetTours",
            new
            {
              fields = toursResourceParameters.Fields,
              orderBy = toursResourceParameters.OrderBy,
              searchQuery = toursResourceParameters.SearchQuery,
              pageNumber = toursResourceParameters.PageIndex - 1,
              pageSize = toursResourceParameters.PageSize
            });
        case ResourceUriType.NextPage:
          return _urlHelper.Link("GetTours",
            new
            {
              fields = toursResourceParameters.Fields,
              orderBy = toursResourceParameters.OrderBy,
              searchQuery = toursResourceParameters.SearchQuery,
              pageNumber = toursResourceParameters.PageIndex + 1,
              pageSize = toursResourceParameters.PageSize
            });
        case ResourceUriType.Current:
        default:
          return _urlHelper.Link("GetTours",
            new
            {
              fields = toursResourceParameters.Fields,
              orderBy = toursResourceParameters.OrderBy,
              searchQuery = toursResourceParameters.SearchQuery,
              pageNumber = toursResourceParameters.PageIndex,
              pageSize = toursResourceParameters.PageSize
            });
      }
    }

    #endregion
  }
}
