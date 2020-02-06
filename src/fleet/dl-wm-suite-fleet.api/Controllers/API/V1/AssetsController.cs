using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dl.wm.suite.common.dtos.Links;
using dl.wm.suite.common.dtos.Vms.Assets;
using dl.wm.suite.common.infrastructure.Extensions;
using dl.wm.suite.common.infrastructure.Helpers;
using dl.wm.suite.common.infrastructure.Helpers.ResourceParameters;
using dl.wm.suite.common.infrastructure.PropertyMappings;
using dl.wm.suite.common.infrastructure.PropertyMappings.TypeHelpers;
using dl.wm.suite.fleet.api.Controllers.API.Base;
using dl.wm.suite.fleet.api.Validators;
using dl.wm.suite.fleet.contracts.Assets;
using dl.wm.suite.fleet.contracts.V1;
using dl.wm.suite.fleet.model.Assets;
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
  public class AssetsController : BaseController
  {
    private readonly IUrlHelper _urlHelper;
    private readonly ITypeHelperService _typeHelperService;
    private readonly IPropertyMappingService _propertyMappingService;

    private readonly IInquiryAllAssetsProcessor _inquiryAllAssetsProcessor;

    private readonly IInquiryAssetProcessor _inquiryAssetProcessor;
    private readonly ICreateAssetProcessor _createAssetProcessor;
    private readonly IUpdateAssetProcessor _updateAssetProcessor;

    public AssetsController(IUrlHelper urlHelper,
      ITypeHelperService typeHelperService, IPropertyMappingService propertyMappingService,
      IAssetsControllerDependencyBlock assetBlock)
    {
      _urlHelper = urlHelper;
      _typeHelperService = typeHelperService;
      _propertyMappingService = propertyMappingService;

      _inquiryAllAssetsProcessor = assetBlock.InquiryAllAssetsProcessor;
      _inquiryAssetProcessor = assetBlock.InquiryAssetProcessor;
      _createAssetProcessor = assetBlock.CreateAssetProcessor;
      _updateAssetProcessor = assetBlock.UpdateAssetProcessor;
    }


    /// <summary>
    /// POST : Create a New Asset.
    /// </summary>
    /// <param name="assetForCreationUiModel">AssetForCreationUiModel the Request Model for Creation</param>
    /// <remarks> return a ResponseEntity with status 201 (Created) if the new Asset is created, 400 (Bad Request), 500 (Server Error) </remarks>
    /// <response code="201">Created (if the Asset is created)</response>
    /// <response code="400">Bad Request</response>
    /// <response code="500">Internal Server Error</response>
    [HttpPost(Name = "PostAssetRoute")]
    [ValidateModel]
    [Authorize]
    public async Task<IActionResult> PostAssetRouteAsync([FromBody] AssetForCreationUiModel assetForCreationUiModel)
    {
      var userAudit = GetEmailFromClaims();

      if (userAudit == null)
        return BadRequest();

      var newCreatedAsset =
        await _createAssetProcessor.CreateAssetAsync(userAudit, assetForCreationUiModel);

      switch (newCreatedAsset.Message)
      {
        case ("SUCCESS_CREATION"):
          {
            Log.Information(
              $"--Method:PostAssetRouteAsync -- Message:ASSET_CREATION_SUCCESSFULLY -- Datetime:{DateTime.Now} -- " +
              $"AssetInfo:{assetForCreationUiModel.AssetNumPlate}");

            return Created("PostAssetRouteAsync",
              newCreatedAsset);
          }
        case ("ERROR_ASSET_ALREADY_EXISTS"):
          {
            Log.Error(
              $"--Method:PostAssetRouteAsync -- Message:ERROR_ASSET_ALREADY_EXISTS -- Datetime:{DateTime.UtcNow} -- " +
              $"AssetInfo:{assetForCreationUiModel.AssetNumPlate}");
            return BadRequest(new { errorMessage = "ASSET_ALREADY_EXISTS" });
          }
        case ("ERROR_ASSET_NOT_MADE_PERSISTENT"):
          {
            Log.Error(
              $"--Method:PostAssetRouteAsync -- Message:ERROR_ASSET_NOT_MADE_PERSISTENT -- Datetime:{DateTime.UtcNow} -- " +
              $"AssetInfo:{assetForCreationUiModel.AssetNumPlate}");
            return BadRequest(new { errorMessage = "ERROR_CREATION_NEW_ASSET" });
          }
        case ("UNKNOWN_ERROR"):
          {
            Log.Error(
              $"--Method:PostAssetRouteAsync -- Message:ERROR_CREATE_NEW_ASSET -- Datetime:{DateTime.UtcNow} -- " +
              $"AssetInfo:{assetForCreationUiModel.AssetNumPlate}");
            return BadRequest(new { errorMessage = "ERROR_CREATION_NEW_ASSET" });
          }
      }

      return BadRequest(new { errorMessage = "UNKNOWN_ERROR_CREATION_NEW_ASSET" });
    }


    /// <summary>
    /// Get - Retrieve Stored Asset providing Asset Id
    /// </summary>
    /// <param name="id">Asset Id the Request Index for Retrieval</param>
    /// <param name="fields">Fiends to be filtered with for the returned Asset</param>
    /// <remarks>Retrieve Assets providing Id and [Optional] fields</remarks>
    /// <response code="200">Resource retrieved correctly</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="404">Resource Not Found</response>
    /// <response code="500">Internal Server Error.</response>

    [HttpGet("{id}", Name = "GetAssetRoot")]
    public async Task<IActionResult> GetAssetAsync(int id, [FromQuery] string fields)
    {
      if (!_typeHelperService.TypeHasProperties<AssetUiModel>
        (fields))
      {
        return BadRequest();
      }

      var assetFromRepo = await _inquiryAssetProcessor.GetAssetAsync(id);

      if (assetFromRepo == null)
      {
        return NotFound();
      }

      var asset = Mapper.Map<AssetUiModel>(assetFromRepo);

      var links = CreateLinksForAsset(id, fields);

      var linkedResourceToReturn = asset.ShapeData(fields)
        as IDictionary<string, object>;

      linkedResourceToReturn.Add("links", links);

      return Ok(linkedResourceToReturn);
    }


    /// <summary>
    /// Get - Retrieve All/or Partial Paged Stored Assets
    /// </summary>
    /// <remarks>Retrieve paged Assets providing Paging Query</remarks>
    /// <response code="200">Resource retrieved correctly.</response>
    /// <response code="500">Internal Server Error.</response>
    [HttpGet(Name = "GetAssets")]
    public async Task<IActionResult> GetAssetsAsync(
      [FromQuery] AssetsResourceParameters assetsResourceParameters, [FromHeader(Name = "Accept")] string mediaType)
    {
      if (!_propertyMappingService.ValidMappingExistsFor<AssetUiModel, Asset>
        (assetsResourceParameters.OrderBy))
      {
        return BadRequest();
      }

      if (!_typeHelperService.TypeHasProperties<AssetUiModel>
        (assetsResourceParameters.Fields))
      {
        return BadRequest();
      }

      var assetsQueryable = await _inquiryAllAssetsProcessor.GetAllAssetsAsync(assetsResourceParameters);

      var assets = Mapper.Map<IEnumerable<AssetUiModel>>(assetsQueryable);

      if (mediaType.Contains("application/vnd.marvin.hateoas+json"))
      {
        var paginationMetadata = new
        {
          totalCount = assetsQueryable.TotalCount,
          pageSize = assetsQueryable.PageSize,
          currentPage = assetsQueryable.CurrentPage,
          totalPages = assetsQueryable.TotalPages,
        };

        Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(paginationMetadata));

        var links = CreateLinksForAssets(assetsResourceParameters,
          assetsQueryable.HasNext, assetsQueryable.HasPrevious);

        var shapedPersons = assets.ShapeData(assetsResourceParameters.Fields);

        var shapedPersonsWithLinks = shapedPersons.Select(person =>
        {
          var personAsDictionary = person as IDictionary<string, object>;
          var personLinks =
            CreateLinksForAsset((int)personAsDictionary["Id"],
              assetsResourceParameters.Fields);

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
        var previousPageLink = assetsQueryable.HasPrevious
          ? CreateAssetsResourceUri(assetsResourceParameters,
            ResourceUriType.PreviousPage)
          : null;

        var nextPageLink = assetsQueryable.HasNext
          ? CreateAssetsResourceUri(assetsResourceParameters,
            ResourceUriType.NextPage)
          : null;

        var paginationMetadata = new
        {
          previousPageLink = previousPageLink,
          nextPageLink = nextPageLink,
          totalCount = assetsQueryable.TotalCount,
          pageSize = assetsQueryable.PageSize,
          currentPage = assetsQueryable.CurrentPage,
          totalPages = assetsQueryable.TotalPages
        };

        Response.Headers.Add("X-Pagination",
          JsonConvert.SerializeObject(paginationMetadata));

        return Ok(assets.ShapeData(assetsResourceParameters.Fields));
      }
    }

    /// <summary>
    /// Get latest position of all assets
    /// </summary>
    /// <returns>Dictionary containing Asset Name vs last seen TrackingPoint</returns>
    // GET api/assets/positions
    [HttpGet("positions", Name = "GetAssetsLatestPositionsRoot")]
    [ProducesResponseType(typeof(string), 200)]
    public async Task<IActionResult> GetAssetsLatestPositionsAsync()
    {
      return Ok();
    }


    /// <summary>
    /// Get the TrackingPoints belonging to the specified asset
    /// </summary>
    /// <param name="assetId">The asset id</param>
    /// <returns>List of TrackingPoints</returns>
    // GET api/assets/321c0ef1-5430-4c10-815a-3e84e12104c4/points
    [HttpGet("{assetId}/points", Name = "GetAssetsTrackingPointsRoot")]
    [ProducesResponseType(typeof(string), 200)]
    public async Task<IActionResult> GetAssetsTrackingPointsAsync(Guid assetId)
    {
      return Ok();
    }

    /// <summary>
    /// Get the trips done by the asset 
    /// </summary>
    /// <param name="assetId">The asset id</param>
    /// <returns>List of Trips</returns>
    // GET api/assets/321c0ef1-5430-4c10-815a-3e84e12104c4/trips
    [HttpGet("{assetId}/trips")]
    [ProducesResponseType(typeof(string), 200)]
    public async Task<IActionResult> GetTrips(string assetId)
    {
      return Ok();
    }


    #region Link Builder

    private IEnumerable<LinkDto> CreateLinksForAsset(int id, string fields)
    {
      var links = new List<LinkDto>();

      if (String.IsNullOrWhiteSpace(fields))
      {
        links.Add(
          new LinkDto(_urlHelper.Link("GetAsset", new {id = id}),
            "self",
            "GET"));
      }
      else
      {
        links.Add(
          new LinkDto(_urlHelper.Link("GetAsset", new {id = id, fields = fields}),
            "self",
            "GET"));
      }

      return links;
    }

    private IEnumerable<LinkDto> CreateLinksForAssets(
    AssetsResourceParameters assetsResourceParameters,
    bool hasNext, bool hasPrevious)
    {
      var links = new List<LinkDto>
            {
                new LinkDto(CreateAssetsResourceUri(assetsResourceParameters,
                        ResourceUriType.Current)
                    , "self", "GET")
            };

      if (hasNext)
      {
        links.Add(
            new LinkDto(CreateAssetsResourceUri(assetsResourceParameters,
                    ResourceUriType.NextPage),
                "nextPage", "GET"));
      }

      if (hasPrevious)
      {
        links.Add(
            new LinkDto(CreateAssetsResourceUri(assetsResourceParameters,
                    ResourceUriType.PreviousPage),
                "previousPage", "GET"));
      }

      return links;
    }

    private string CreateAssetsResourceUri(
        AssetsResourceParameters assetsResourceParameters,
        ResourceUriType type)
    {
      switch (type)
      {
        case ResourceUriType.PreviousPage:
          return _urlHelper.Link("GetAssets",
              new
              {
                fields = assetsResourceParameters.Fields,
                orderBy = assetsResourceParameters.OrderBy,
                searchQuery = assetsResourceParameters.SearchQuery,
                pageNumber = assetsResourceParameters.PageIndex - 1,
                pageSize = assetsResourceParameters.PageSize
              });
        case ResourceUriType.NextPage:
          return _urlHelper.Link("GetAssets",
              new
              {
                fields = assetsResourceParameters.Fields,
                orderBy = assetsResourceParameters.OrderBy,
                searchQuery = assetsResourceParameters.SearchQuery,
                pageNumber = assetsResourceParameters.PageIndex + 1,
                pageSize = assetsResourceParameters.PageSize
              });
        case ResourceUriType.Current:
        default:
          return _urlHelper.Link("GetAssets",
              new
              {
                fields = assetsResourceParameters.Fields,
                orderBy = assetsResourceParameters.OrderBy,
                searchQuery = assetsResourceParameters.SearchQuery,
                pageNumber = assetsResourceParameters.PageIndex,
                pageSize = assetsResourceParameters.PageSize
              });
      }
    }

    #endregion
  }
}
