using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dl.wm.suite.cms.api.Controllers.API.Base;
using dl.wm.suite.cms.api.Validators;
using dl.wm.suite.cms.contracts.Employees.Departments;
using dl.wm.suite.cms.contracts.Users;
using dl.wm.suite.cms.contracts.V1;
using dl.wm.suite.cms.model.Employees.Departments;
using dl.wm.suite.common.dtos.Links;
using dl.wm.suite.common.dtos.Vms.Employees.Departments;
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
    public class DepartmentsController : BaseController
    {
        private readonly IUrlHelper _urlHelper;
        private readonly ITypeHelperService _typeHelperService;
        private readonly IPropertyMappingService _propertyMappingService;

        private readonly IInquiryAllDepartmentsProcessor _inquiryAllDepartmentsProcessor;
        private readonly IInquiryDepartmentProcessor _inquiryDepartmentProcessor;
        private readonly ICreateDepartmentProcessor _createDepartmentProcessor;
        private readonly IUpdateDepartmentProcessor _updateDepartmentProcessor;

        private readonly IInquiryUserProcessor _inquiryUserProcessor;


        public DepartmentsController(IUrlHelper urlHelper,
            ITypeHelperService typeHelperService,
            IPropertyMappingService propertyMappingService,
            IDepartmentsControllerDependencyBlock blockDepartment, 
            IUsersControllerDependencyBlock blockUser)
        {
            _urlHelper = urlHelper;
            _typeHelperService = typeHelperService;
            _propertyMappingService = propertyMappingService;

            _inquiryAllDepartmentsProcessor = blockDepartment.InquiryAllDepartmentsProcessor;
            _inquiryDepartmentProcessor = blockDepartment.InquiryDepartmentProcessor;
            _createDepartmentProcessor = blockDepartment.CreateDepartmentProcessor;
            _updateDepartmentProcessor = blockDepartment.UpdateDepartmentProcessor;

            _inquiryUserProcessor = blockUser.InquiryUserProcessor;
        }

        /// <summary>
        /// POST : Create a New Department.
        /// </summary>
        /// <param name="departmentForCreationUiModel">DepartmentForCreationUiModel the Request Model for Creation</param>
        /// <remarks> return a ResponseEntity with status 201 (Created) if the new Department is created, 400 (Bad Request), 500 (Server Error) </remarks>
        /// <response code="201">Created (if the Department is created)</response>
        /// <response code="400">Bad Request</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPost(Name = "PostDepartmentRoute")]
        [ValidateModel]
        public async Task<IActionResult> PostDepartmentRouteAsync([FromBody] DepartmentForCreationUiModel departmentForCreationUiModel)
        {
            var userAudit = await _inquiryUserProcessor.GetUserByLoginAsync(GetEmailFromClaims());

            if (userAudit == null)
                return BadRequest();

            var newCreatedDepartment =
                await _createDepartmentProcessor.CreateDepartmentAsync(userAudit.Id, departmentForCreationUiModel);

            switch (newCreatedDepartment.Message)
            {
                case ("SUCCESS_CREATION"):
                {
                    Log.Information(
                        $"--Method:PostDepartmentRouteAsync -- Message:DEPARTMENT_CREATION_SUCCESSFULLY -- " +
                        $"Datetime:{DateTime.Now} -- DepartmentInfo:{departmentForCreationUiModel.DepartmentName}");
                    return Created(nameof(PostDepartmentRouteAsync), newCreatedDepartment);
                }
                case ("ERROR_ALREADY_EXISTS"):
                {
                    Log.Error(
                        $"--Method:PostDepartmentRouteAsync -- Message:ERROR_DEPARTMENT_ALREADY_EXISTS -- " +
                        $"Datetime:{DateTime.UtcNow} -- DepartmentInfo:{departmentForCreationUiModel.DepartmentName}");
                    return BadRequest(new {errorMessage = "DEPARTMENT_ALREADY_EXISTS"});
                }
                case ("ERROR_DEPARTMENT_NOT_MADE_PERSISTENT"):
                {
                    Log.Error(
                        $"--Method:PostDepartmentRouteAsync -- Message:ERROR_DEPARTMENT_NOT_MADE_PERSISTENT -- " +
                        $"Datetime:{DateTime.UtcNow} -- DepartmentInfo:{departmentForCreationUiModel.DepartmentName}");
                    return BadRequest(new {errorMessage = "ERROR_CREATION_NEW_DEPARTMENT"});
                }
                case ("UNKNOWN_ERROR"):
                {
                    Log.Error(
                        $"--Method:PostDepartmentRouteAsync -- Message:ERROR_CREATION_NEW_DEPARTMENT -- " +
                        $"Datetime:{DateTime.UtcNow} -- DepartmentInfo:{departmentForCreationUiModel.DepartmentName}");
                    return BadRequest(new {errorMessage = "ERROR_CREATION_NEW_DEPARTMENT"});
                }
            }

            return NotFound();
        }

        /// <summary>
        /// PUT : Update Department with New Department Name
        /// </summary>
        /// <param name="id">Department Id the Request Index for Retrieval</param>
        /// <param name="updatedDepartment">DepartmentForModification the Request Model with New Department Name</param>
        /// <remarks>Change Department providing DepartmentForModificationUiModel with Modified Department Name</remarks>
        /// <response code="200">Resource updated correctly.</response>
        /// <response code="400">The model is not in valid state.</response>
        /// <response code="403">You have not access for this action.</response>
        /// <response code="404">Wrong attributes provided.</response>
        /// <response code="500">Internal Server Error.</response>
        [HttpPut("{id}", Name = "UpdateDepartmentWithModifiedDepartment")]
        [ValidateModel]
        public async Task<IActionResult> UpdateDepartmentWithModifiedDepartmentAsync(Guid id,
            [FromBody] DepartmentForModificationUiModel updatedDepartment)
        {
            if (id == Guid.Empty || String.IsNullOrEmpty(updatedDepartment.DepartmentName))
                return BadRequest();

            var userAudit = await _inquiryUserProcessor.GetUserByLoginAsync(GetEmailFromClaims());

            if (userAudit == null)
                return BadRequest();

            await _updateDepartmentProcessor.UpdateDepartmentAsync(id, userAudit.Id, updatedDepartment);

            return Ok(await _inquiryDepartmentProcessor.GetDepartmentByIdAsync(id));
        }


        /// <summary>
        /// Get : Retrieve Stored Department providing Department Id
        /// </summary>
        /// <param name="id">Department Id the Request Index for Retrieval</param>
        /// <param name="fields">Fiends to be filtered with for the returned Department</param>
        /// <remarks>Retrieve Department providing Id and [Optional] fields</remarks>
        /// <response code="200">Resource retrieved correctly</response>
        /// <response code="404">Resource Not Found</response>
        /// <response code="500">Internal Server Error.</response>
        [HttpGet("{id}", Name = "GetDepartment")]
        public async Task<IActionResult> GetDepartmentAsync(Guid id, [FromQuery] string fields)
        {
            if (!_typeHelperService.TypeHasProperties<DepartmentUiModel>
                (fields))
            {
                return BadRequest();
            }

            var departmentFromRepo = await _inquiryDepartmentProcessor.GetDepartmentByIdAsync(id);

            if (departmentFromRepo == null)
            {
                return NotFound();
            }

            var department = Mapper.Map<DepartmentUiModel>(departmentFromRepo);

            var links = CreateLinksForDepartment(id, fields);

            var linkedResourceToReturn = department.ShapeData(fields)
                as IDictionary<string, object>;

            linkedResourceToReturn.Add("links", links);

            return Ok(linkedResourceToReturn);
        }

        /// <summary>
        /// Get : Retrieve All/or Partial Paged Stored Departments
        /// </summary>
        /// <remarks>Retrieve paged Departments providing Paging Query</remarks>
        /// <response code="200">Resource retrieved correctly.</response>
        /// <response code="500">Internal Server Error.</response>
        [HttpGet(Name = "GetDepartments")]
        public async Task<IActionResult> GetDepartmentsAsync([FromQuery] DepartmentsResourceParameters departmentsResourceParameters,
            [FromHeader(Name = "Accept")] string mediaType)
        {
            if (!_propertyMappingService.ValidMappingExistsFor<DepartmentUiModel, Department>
                (departmentsResourceParameters.OrderBy))
            {
                return BadRequest();
            }

            if (!_typeHelperService.TypeHasProperties<DepartmentUiModel>
                (departmentsResourceParameters.Fields))
            {
                return BadRequest();
            }

            var departmentsQueryable = await _inquiryAllDepartmentsProcessor.GetDepartmentsAsync(departmentsResourceParameters);

            var departments = Mapper.Map<IEnumerable<DepartmentUiModel>>(departmentsQueryable);

            if (mediaType.Contains("application/vnd.marvin.hateoas+json"))
            {
                var paginationMetadata = new
                {
                    totalCount = departmentsQueryable.TotalCount,
                    pageSize = departmentsQueryable.PageSize,
                    currentPage = departmentsQueryable.CurrentPage,
                    totalPages = departmentsQueryable.TotalPages,
                };

                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(paginationMetadata));

                var links = CreateLinksForDepartments(departmentsResourceParameters,
                    departmentsQueryable.HasNext, departmentsQueryable.HasPrevious);

                var shapedPersons = departments.ShapeData(departmentsResourceParameters.Fields);

                var shapedPersonsWithLinks = shapedPersons.Select(person =>
                {
                    var personAsDictionary = person as IDictionary<string, object>;
                    var personLinks =
                        CreateLinksForDepartment((Guid) personAsDictionary["Id"], departmentsResourceParameters.Fields);

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
                var previousPageLink = departmentsQueryable.HasPrevious
                    ? CreateDepartmentsResourceUri(departmentsResourceParameters,
                        ResourceUriType.PreviousPage)
                    : null;

                var nextPageLink = departmentsQueryable.HasNext
                    ? CreateDepartmentsResourceUri(departmentsResourceParameters,
                        ResourceUriType.NextPage)
                    : null;

                var paginationMetadata = new
                {
                    previousPageLink = previousPageLink,
                    nextPageLink = nextPageLink,
                    totalCount = departmentsQueryable.TotalCount,
                    pageSize = departmentsQueryable.PageSize,
                    currentPage = departmentsQueryable.CurrentPage,
                    totalPages = departmentsQueryable.TotalPages
                };

                Response.Headers.Add("X-Pagination",
                    JsonConvert.SerializeObject(paginationMetadata));

                return Ok(departments.ShapeData(departmentsResourceParameters.Fields));
            }
        }

        #region Link Builder

        private IEnumerable<LinkDto> CreateLinksForDepartment(Guid id, string fields)
        {
            var links = new List<LinkDto>();

            if (String.IsNullOrWhiteSpace(fields))
            {
                links.Add(
                    new LinkDto(_urlHelper.Link("GetDepartment", new {id = id}),
                        "self",
                        "GET"));
            }
            else
            {
                links.Add(
                    new LinkDto(_urlHelper.Link("GetDepartment", new {id = id, fields = fields}),
                        "self",
                        "GET"));
            }

            return links;
        }


        private IEnumerable<LinkDto> CreateLinksForDepartments(DepartmentsResourceParameters departmentsResourceParameters,
            bool hasNext, bool hasPrevious)
        {
            var links = new List<LinkDto>
            {
                new LinkDto(CreateDepartmentsResourceUri(departmentsResourceParameters,
                        ResourceUriType.Current)
                    , "self", "GET")
            };

            if (hasNext)
            {
                links.Add(
                    new LinkDto(CreateDepartmentsResourceUri(departmentsResourceParameters,
                            ResourceUriType.NextPage),
                        "nextPage", "GET"));
            }

            if (hasPrevious)
            {
                links.Add(
                    new LinkDto(CreateDepartmentsResourceUri(departmentsResourceParameters,
                            ResourceUriType.PreviousPage),
                        "previousPage", "GET"));
            }

            return links;
        }

        private string CreateDepartmentsResourceUri(DepartmentsResourceParameters departmentsResourceParameters,
            ResourceUriType type)
        {
            switch (type)
            {
                case ResourceUriType.PreviousPage:
                    return _urlHelper.Link("GetDepartments",
                        new
                        {
                            fields = departmentsResourceParameters.Fields,
                            orderBy = departmentsResourceParameters.OrderBy,
                            searchQuery = departmentsResourceParameters.SearchQuery,
                            pageNumber = departmentsResourceParameters.PageIndex - 1,
                            pageSize = departmentsResourceParameters.PageSize
                        });
                case ResourceUriType.NextPage:
                    return _urlHelper.Link("GetDepartments",
                        new
                        {
                            fields = departmentsResourceParameters.Fields,
                            orderBy = departmentsResourceParameters.OrderBy,
                            searchQuery = departmentsResourceParameters.SearchQuery,
                            pageNumber = departmentsResourceParameters.PageIndex + 1,
                            pageSize = departmentsResourceParameters.PageSize
                        });
                case ResourceUriType.Current:
                default:
                    return _urlHelper.Link("GetDepartments",
                        new
                        {
                            fields = departmentsResourceParameters.Fields,
                            orderBy = departmentsResourceParameters.OrderBy,
                            searchQuery = departmentsResourceParameters.SearchQuery,
                            pageNumber = departmentsResourceParameters.PageIndex,
                            pageSize = departmentsResourceParameters.PageSize
                        });
            }
        }

        #endregion
    }
}
