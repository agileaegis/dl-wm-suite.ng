using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using dl.wm.suite.cms.api.Controllers.API.Base;
using dl.wm.suite.cms.api.Validators;
using dl.wm.suite.cms.contracts.Containers;
using dl.wm.suite.cms.contracts.Users;
using dl.wm.suite.cms.contracts.V1;
using dl.wm.suite.cms.model.Containers;
using dl.wm.suite.common.dtos.Links;
using dl.wm.suite.common.dtos.Vms.Containers;
using dl.wm.suite.common.infrastructure.Extensions;
using dl.wm.suite.common.infrastructure.Helpers;
using dl.wm.suite.common.infrastructure.Helpers.ResourceParameters;
using dl.wm.suite.common.infrastructure.PropertyMappings;
using dl.wm.suite.common.infrastructure.PropertyMappings.TypeHelpers;
using AutoMapper;
using dl.wm.suite.cms.api.Helpers;
using dl.wm.suite.cms.api.Helpers.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Serilog;

namespace dl.wm.suite.cms.api.Controllers.API.V1
{
  [Produces("application/json")]
  [ApiVersion("1.0")]
  [ResponseCache(Duration = 0, NoStore = true, VaryByHeader = "*")]
  [Route("api/v{version:apiVersion}/[controller]")]
  [ApiController]
  //[Authorize]
  public class ContainersController : GeoBaseController
  {
    public IConfiguration Configuration { get; }

    private readonly IHostingEnvironment _environment;
    private readonly IUrlHelper _urlHelper;
    private readonly ITypeHelperService _typeHelperService;
    private readonly IPropertyMappingService _propertyMappingService;

    private readonly IInquiryAllContainersProcessor _inquiryAllContainersProcessor;

    private readonly IInquiryContainerProcessor _inquiryContainerProcessor;
    private readonly ICreateContainerProcessor _createContainerProcessor;
    private readonly IUpdateContainerProcessor _updateContainerProcessor;
    private readonly IDeleteContainerProcessor _deleteContainerProcessor;

    private readonly IInquiryUserProcessor _inquiryUserProcessor;
    private AzureStorageConfig _storageConfig = null;

    public ContainersController(IUrlHelper urlHelper, IConfiguration configuration, IHostingEnvironment environment,
      ITypeHelperService typeHelperService, IPropertyMappingService propertyMappingService,
      IInquiryAllContainersProcessor inquiryAllContainersProcessor,
      IInquiryContainerProcessor inquiryContainerProcessor,
      ICreateContainerProcessor createContainerProcessor,
      IUpdateContainerProcessor updateContainerProcessor, IDeleteContainerProcessor deleteContainerProcessor,
      IUsersControllerDependencyBlock userBlock)
    {
      _urlHelper = urlHelper;
      _typeHelperService = typeHelperService;
      _propertyMappingService = propertyMappingService;

      Configuration = configuration;

      _environment = environment ?? throw new ArgumentNullException(nameof(environment));

      _inquiryAllContainersProcessor = inquiryAllContainersProcessor;
      _inquiryContainerProcessor = inquiryContainerProcessor;
      _createContainerProcessor = createContainerProcessor;
      _updateContainerProcessor = updateContainerProcessor;
      _deleteContainerProcessor = deleteContainerProcessor;
      _deleteContainerProcessor = deleteContainerProcessor;

      _inquiryUserProcessor = userBlock.InquiryUserProcessor;
    }

    /// <summary>
    /// POST : Create a New Container Image
    /// </summary>
    /// <param name="file">ContainerForCreationUiModel the Request Model for Creation</param>
    /// <remarks> return a ResponseEntity with status 200 (Ok) if the new Image Container is created, 400 (Bad Request), 500 (Server Error) </remarks>
    /// <response code="201">Created (if the Container is created)</response>
    /// <response code="400">Bad Request</response>
    /// <response code="500">Internal Server Error</response>
    [HttpPost("image-upload", Name = "PostContainerImageRoute")]
    public async Task<IActionResult> PostContainerImageAsync(IFormFile file)
    {
      if (string.IsNullOrWhiteSpace(_environment.WebRootPath))
      {
        _environment.WebRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
      }

      string imagePath = DateTime.UtcNow.ToString().Replace("/", "").Replace(" ", "").Replace(":", "");

      try
      {
        var folder = Path.Combine(_environment.WebRootPath, imagePath);

        try
        {
          if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);
        }
        catch (Exception e)
        {
          return Ok(new ImageContainerDto
          {
            IsStoredSuccessfully = false, Path = "",
            Message = $"Message:{e.Message} and inner massage:{e.InnerException?.Message}"
          });
        }

        if (file.Length > 0)
        {
          using (var fileStream = new FileStream(Path.Combine(folder, file.FileName), FileMode.Create))
          {
            await file.CopyToAsync(fileStream);
          }
        }
      }
      catch (Exception e)
      {
        return BadRequest(new ImageContainerDto
        {
          IsStoredSuccessfully = false, Path = "",
          Message = $"Message:{e.Message} and inner massage:{e.InnerException?.Message}"
        });
      }

      return Ok(new ImageContainerDto {IsStoredSuccessfully = true, Path = imagePath, Message = "All Set"});
    }

    /// <summary>
    /// POST : Create a New Container.
    /// </summary>
    /// <param name="containerForCreationUiModel">ContainerForCreationUiModel the Request Model for Creation</param>
    /// <remarks> return a ResponseEntity with status 201 (Created) if the new Container is created, 400 (Bad Request), 500 (Server Error) </remarks>
    /// <response code="201">Created (if the Container is created)</response>
    /// <response code="400">Bad Request</response>
    /// <response code="500">Internal Server Error</response>
    [HttpPost(Name = "PostContainerRoute")]
    [ValidateModel]
    public async Task<IActionResult> PostContainerRouteAsync(
      [FromBody] ContainerForCreationUiModel containerForCreationUiModel)
    {
      var userAudit = await _inquiryUserProcessor.GetUserByLoginAsync(GetEmailFromClaims());

      if (userAudit == null)
        return BadRequest();

      if (string.IsNullOrWhiteSpace(_environment.WebRootPath))
      {
        _environment.WebRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
      }

      string imageFolder = Path.Combine(_environment.WebRootPath, containerForCreationUiModel.ContainerImagePath);

      _storageConfig = new AzureStorageConfig
      {
        AccountName = Configuration.GetSection("AzureStorageConfig:AccountName").Value,
        AccountKey = Configuration.GetSection("AzureStorageConfig:AccountKey").Value,
        ImageContainer = Configuration.GetSection("AzureStorageConfig:ImageContainer").Value,
        ThumbnailContainer = Configuration.GetSection("AzureStorageConfig:ThumbnailContainer").Value
      };

      string imageBlobName = Guid.NewGuid().ToString();

      try
      {
        string imagePathFolder = string.Empty;
        
        imagePathFolder = $"{imageFolder}//{containerForCreationUiModel.ContainerImageName}"; 
        //imagePathFolder = $"{imageFolder}\\{containerForCreationUiModel.ContainerImageName}"; 
        
        using (FileStream fs = new FileStream(imagePathFolder, FileMode.Open, FileAccess.Read, FileShare.Read, 8,
          true))
        {
          try
          {
            var isUploaded = await StorageHelper.UploadFileToStorage(fs, imageBlobName, _storageConfig);

            if(!isUploaded)
              return BadRequest(new { errorMessage = "ERROR_STORING_CONTAINER_IMAGE" });
          }
          catch (Exception e)
          {
            return BadRequest(new {errorMessage = "ERROR_STORING_CONTAINER_IMAGE"});
          }
        }
      }
      catch (Exception e)
      {
        return BadRequest(new { errorMessage = $"ERROR_RETRIEVE_CONTAINER_IMAGE. Details:{e.Message} inner: {e?.InnerException?.Message}" });
      }


      ContainerForCreationModel newContainerForCreationModel = new ContainerForCreationModel()
      {
        ContainerAddress = GetLocationByCoordinates(Configuration["BingMapsAPIKey"],
          containerForCreationUiModel.ContainerLat, containerForCreationUiModel.ContainerLong),
        ContainerName = containerForCreationUiModel.ContainerName,
        ContainerLevel = containerForCreationUiModel.ContainerLevel,
        ContainerFillLevel = containerForCreationUiModel.ContainerFillLevel,
        ContainerLat = containerForCreationUiModel.ContainerLat,
        ContainerLong = containerForCreationUiModel.ContainerLong,
        ContainerType = containerForCreationUiModel.ContainerType,
        ContainerStatus = containerForCreationUiModel.ContainerStatus,
        ContainerPickupDate = containerForCreationUiModel.ContainerPickupDate,
        ContainerPickupActive = containerForCreationUiModel.ContainerPickupActive,
        ContainerImageName = imageBlobName
      };

      var newCreatedContainer =
        await _createContainerProcessor.CreateContainerAsync(userAudit.Id, newContainerForCreationModel);

      switch (newCreatedContainer.Message)
      {
        case ("SUCCESS_CREATION"):
        {
          Log.Information(
            $"--Method:PostContainerRouteAsync -- Message:CONTAINER_CREATION_SUCCESSFULLY -- " +
            $"Datetime:{DateTime.Now} -- ContainerInfo:{containerForCreationUiModel.ContainerName}");
          return Created(nameof(PostContainerRouteAsync), newCreatedContainer);
        }
        case ("ERROR_ALREADY_EXISTS"):
        {
          Log.Error(
            $"--Method:PostContainerRouteAsync -- Message:ERROR_CONTAINER_ALREADY_EXISTS -- " +
            $"Datetime:{DateTime.Now} -- ContainerInfo:{containerForCreationUiModel.ContainerName}");
          return BadRequest(new {errorMessage = "CONTAINER_ALREADY_EXISTS"});
        }
        case ("ERROR_CONTAINER_NOT_MADE_PERSISTENT"):
        {
          Log.Error(
            $"--Method:PostContainerRouteAsync -- Message:ERROR_CONTAINER_NOT_MADE_PERSISTENT -- " +
            $"Datetime:{DateTime.Now} -- ContainerInfo:{containerForCreationUiModel.ContainerName}");
          return BadRequest(new {errorMessage = "ERROR_CREATION_NEW_CONTAINER"});
        }
        case ("UNKNOWN_ERROR"):
        {
          Log.Error(
            $"--Method:PostContainerRouteAsync -- Message:ERROR_CREATION_NEW_CONTAINER -- " +
            $"Datetime:{DateTime.Now} -- ContainerInfo:{containerForCreationUiModel.ContainerName}");
          return BadRequest(new {errorMessage = "ERROR_CREATION_NEW_CONTAINER"});
        }
      }

      return NotFound();
    }

    /// <summary>
    /// PUT : Provisioning an Existing Container with an Existing Device.
    /// </summary>
    /// <param name="id">Container Id for Modification</param>
    /// <param name="deviceId">Device Id for Provisioning</param>
    /// <param name="containerForModificationProvisioningModel">ContainerForModificationProvisioningModel the Request Model for Modification</param>
    /// <remarks> return a ResponseEntity with status 200 (Ok) if the new Container provisioning with Device Correctly, 400 (Bad Request), 500 (Server Error) </remarks>
    /// <response code="200">Ok (if the Container is updated)</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="400">Bad Request</response>
    /// <response code="500">Internal Server Error</response>
    [HttpPut("{id}/provisioning-device/{deviceId}", Name = "PutContainerProvisioningDeviceRoute")]
    [ValidateModel]
    public async Task<IActionResult> PutContainerProvisioningDeviceAsync(Guid id, Guid deviceId,
      [FromBody] ContainerForModificationProvisioningModel containerForModificationProvisioningModel)
    {
      var userAudit = await _inquiryUserProcessor.GetUserByLoginAsync(GetEmailFromClaims());

      if (userAudit == null)
        return BadRequest(new {errorMessage = "AUDIT_USER_NOT_EXIST"});

      var provisioningDeviceToContainer =
        await _updateContainerProcessor.ProvisioningDeviceToContainerAsync(userAudit.Id, id, deviceId, containerForModificationProvisioningModel);

      switch (provisioningDeviceToContainer.Message)
      {
        case ("SUCCESS_PROVISIONING"):
        {
          Log.Information(
            $"--Method:PutContainerProvisioningDeviceAsync -- Message:DEVICE_CONTAINER_PROVISIONING_SUCCESSFULLY -- " +
            $"Datetime:{DateTime.Now} -- ContainerInfo:{id} -- DeviceInfo:{deviceId}");
          return Ok(provisioningDeviceToContainer);
        }
        case ("ERROR_DEVICE_NOT_EXISTS"):
        {
          return BadRequest(new {errorMessage = "ERROR_DEVICE_NOT_EXISTS"});
        }
        case ("ERROR_CONTAINER_NOT_EXISTS"):
        {
          return BadRequest(new {errorMessage = "ERROR_CONTAINER_NOT_EXISTS"});
        }
        case ("ERROR_PROVISIONING_FAILED"):
        {
          return BadRequest(new {errorMessage = "ERROR_PROVISIONING_FAILED"});
        }
        case ("UNKNOWN_ERROR"):
        {
          return BadRequest(new {errorMessage = "ERROR_PROVISIONING_DEVICE"});
        }
      }

      return NotFound();
    }



    /// <summary>
    /// PUT : Update an Existing Container.
    /// </summary>
    /// <param name="id">Container Id for Modification</param>
    /// <param name="containerForModificationUiModel">ContainerForCreationUiModel the Request Model for Modification</param>
    /// <remarks> return a ResponseEntity with status 201 (Created) if the new Container is created, 400 (Bad Request), 500 (Server Error) </remarks>
    /// <response code="200">Ok (if the Container is updated)</response>
    /// <response code="400">Bad Request</response>
    /// <response code="500">Internal Server Error</response>
    [HttpPut("{id}", Name = "PutContainerRoute")]
    [ValidateModel]
    public async Task<IActionResult> PutContainerRouteAsync(Guid id,
      [FromBody] ContainerForModificationUiModel containerForModificationUiModel)
    {
      return BadRequest(new {errorMessage = "UNKNOWN_ERROR_UPDATE_CONTAINER"});
    }


    /// <summary>
    /// Delete - Delete an existing Container 
    /// </summary>
    /// <param name="id">Container Id for Deletion</param>
    /// <remarks>Delete Existing Container </remarks>
    /// <response code="200">Resource retrieved correctly</response>
    /// <response code="200">Resource Not Found</response>
    /// <response code="500">Internal Server Error.</response>
    [HttpDelete("{id}", Name = "DeleteContainerRoot")]
    public async Task<IActionResult> DeleteContainerRoot(Guid id)
    {
      var userAudit = await _inquiryUserProcessor.GetUserByLoginAsync(GetEmailFromClaims());

      if (userAudit == null)
        return BadRequest();

      var containerToBeSoftDeleted = await _deleteContainerProcessor.SoftDeleteContainerAsync(userAudit.Id, id);

      return Ok(containerToBeSoftDeleted);
    }

    /// <summary>
    /// Delete - Delete an existing Container 
    /// </summary>
    /// <param name="id">Container Id for Deletion</param>
    /// <remarks>Delete Existing Container </remarks>
    /// <response code="200">Resource retrieved correctly</response>
    /// <response code="200">Resource Not Found</response>
    /// <response code="500">Internal Server Error.</response>
    [HttpDelete("hard/{id}", Name = "DeleteHardContainerRoot")]
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "SU")]
    public async Task<IActionResult> DeleteHardContainerRoot(Guid id)
    {
      var userAudit = await _inquiryUserProcessor.GetUserByLoginAsync(GetEmailFromClaims());

      if (userAudit == null)
        return BadRequest();

      var containerToBeDeleted = await _deleteContainerProcessor.HardDeleteContainerAsync(userAudit.Id, id);

      return Ok(containerToBeDeleted.DeletionStatus);
    }

    /// <summary>
    /// Get - Retrieve Stored Container providing Container Id
    /// </summary>
    /// <param name="id">Container Id the Request Index for Retrieval</param>
    /// <param name="fields">Fiends to be filtered with for the returned Container</param>
    /// <remarks>Retrieve Containers providing Id and [Optional] fields</remarks>
    /// <response code="200">Resource retrieved correctly</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="404">Resource Not Found</response>
    /// <response code="500">Internal Server Error.</response>

    [HttpGet("{id}", Name = "GetContainerRoot")]
    public async Task<IActionResult> GetContainerAsync(Guid id, [FromQuery] string fields)
    {
      if (!_typeHelperService.TypeHasProperties<ContainerUiModel>
        (fields))
      {
        return BadRequest();
      }

      var containerFromRepo = await _inquiryContainerProcessor.GetContainerAsync(id);

      if (containerFromRepo == null)
      {
        return NotFound();
      }

      var container = Mapper.Map<ContainerUiModel>(containerFromRepo);

      var links = CreateLinksForContainer(id, fields);

      var linkedResourceToReturn = container.ShapeData(fields)
        as IDictionary<string, object>;

      linkedResourceToReturn.Add("links", links);

      return Ok(linkedResourceToReturn);
    }

    /// <summary>
    /// Get - Retrieve All Containers Points
    /// </summary>
    /// <remarks>Retrieve Containers Points</remarks>
    /// <response code="200">Resource retrieved correctly.</response>
    /// <response code="500">Internal Server Error.</response>
    [HttpGet("points", Name = "GetContainersPointsRoot")]
    public async Task<IActionResult> GetContainersPointsAsync()
    {
      var containersPoints = await _inquiryAllContainersProcessor.GetContainersPointsAsync();

      return Ok(containersPoints);
    }

    /// <summary>
    /// Get - Retrieve All/or Partial Paged Stored Containers
    /// </summary>
    /// <remarks>Retrieve paged Containers providing Paging Query</remarks>
    /// <response code="200">Resource retrieved correctly.</response>
    /// <response code="500">Internal Server Error.</response>
    [HttpGet(Name = "GetContainers")]
    public async Task<IActionResult> GetContainersAsync(
      [FromQuery] ContainersResourceParameters containersResourceParameters,
      [FromHeader(Name = "Accept")] string mediaType)
    {
      if (!_propertyMappingService.ValidMappingExistsFor<ContainerUiModel, Container>
        (containersResourceParameters.OrderBy))
      {
        return BadRequest();
      }

      if (!_typeHelperService.TypeHasProperties<ContainerUiModel>
        (containersResourceParameters.Fields))
      {
        return BadRequest();
      }

      var containersQueryable = await _inquiryAllContainersProcessor.GetContainersAsync(containersResourceParameters);

      var containers = Mapper.Map<IEnumerable<ContainerUiModel>>(containersQueryable);


      if (mediaType.Contains("application/vnd.marvin.hateoas+json"))
      {
        var paginationMetadata = new
        {
          totalCount = containersQueryable.TotalCount,
          pageSize = containersQueryable.PageSize,
          currentPage = containersQueryable.CurrentPage,
          totalPages = containersQueryable.TotalPages,
        };

        Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(paginationMetadata));

        var links = CreateLinksForContainers(containersResourceParameters,
          containersQueryable.HasNext, containersQueryable.HasPrevious);

        var shapedContainers = containers.ShapeData(containersResourceParameters.Fields);

        var shapedContainersWithLinks = shapedContainers.Select(container =>
        {
          var containerAsDictionary = container as IDictionary<string, object>;
          var containerLinks =
            CreateLinksForContainer((Guid) containerAsDictionary["Id"], containersResourceParameters.Fields);

          containerAsDictionary.Add("links", containerLinks);

          return containerAsDictionary;
        });

        var linkedCollectionResource = new
        {
          value = shapedContainersWithLinks,
          links = links
        };

        return Ok(linkedCollectionResource);
      }
      else
      {
        var previousPageLink = containersQueryable.HasPrevious
          ? CreateContainersResourceUri(containersResourceParameters,
            ResourceUriType.PreviousPage)
          : null;

        var nextPageLink = containersQueryable.HasNext
          ? CreateContainersResourceUri(containersResourceParameters,
            ResourceUriType.NextPage)
          : null;

        var paginationMetadata = new
        {
          previousPageLink = previousPageLink,
          nextPageLink = nextPageLink,
          totalCount = containersQueryable.TotalCount,
          pageSize = containersQueryable.PageSize,
          currentPage = containersQueryable.CurrentPage,
          totalPages = containersQueryable.TotalPages
        };

        Response.Headers.Add("X-Pagination",
          JsonConvert.SerializeObject(paginationMetadata));

        return Ok(containers.ShapeData(containersResourceParameters.Fields));
      }
    }

    #region Link Builder

    private IEnumerable<LinkDto> CreateLinksForContainer(Guid id, string fields)
    {
      var links = new List<LinkDto>();

      if (String.IsNullOrWhiteSpace(fields))
      {
        links.Add(
          new LinkDto(_urlHelper.Link("GetContainer", new {id = id}),
            "self",
            "GET"));
      }
      else
      {
        links.Add(
          new LinkDto(_urlHelper.Link("GetContainer", new {id = id, fields = fields}),
            "self",
            "GET"));
      }

      return links;
    }


    private IEnumerable<LinkDto> CreateLinksForContainers(
      ContainersResourceParameters containersResourceParameters,
      bool hasNext, bool hasPrevious)
    {
      var links = new List<LinkDto>
      {
        new LinkDto(CreateContainersResourceUri(containersResourceParameters,
            ResourceUriType.Current)
          , "self", "GET")
      };

      if (hasNext)
      {
        links.Add(
          new LinkDto(CreateContainersResourceUri(containersResourceParameters,
              ResourceUriType.NextPage),
            "nextPage", "GET"));
      }

      if (hasPrevious)
      {
        links.Add(
          new LinkDto(CreateContainersResourceUri(containersResourceParameters,
              ResourceUriType.PreviousPage),
            "previousPage", "GET"));
      }

      return links;
    }

    private string CreateContainersResourceUri(
      ContainersResourceParameters containersResourceParameters,
      ResourceUriType type)
    {
      switch (type)
      {
        case ResourceUriType.PreviousPage:
          return _urlHelper.Link("GetContainers",
            new
            {
              fields = containersResourceParameters.Fields,
              orderBy = containersResourceParameters.OrderBy,
              searchQuery = containersResourceParameters.SearchQuery,
              pageNumber = containersResourceParameters.PageIndex - 1,
              pageSize = containersResourceParameters.PageSize
            });
        case ResourceUriType.NextPage:
          return _urlHelper.Link("GetContainers",
            new
            {
              fields = containersResourceParameters.Fields,
              orderBy = containersResourceParameters.OrderBy,
              searchQuery = containersResourceParameters.SearchQuery,
              pageNumber = containersResourceParameters.PageIndex + 1,
              pageSize = containersResourceParameters.PageSize
            });
        case ResourceUriType.Current:
        default:
          return _urlHelper.Link("GetContainers",
            new
            {
              fields = containersResourceParameters.Fields,
              orderBy = containersResourceParameters.OrderBy,
              searchQuery = containersResourceParameters.SearchQuery,
              pageNumber = containersResourceParameters.PageIndex,
              pageSize = containersResourceParameters.PageSize
            });
      }
    }

    #endregion
  }
}
