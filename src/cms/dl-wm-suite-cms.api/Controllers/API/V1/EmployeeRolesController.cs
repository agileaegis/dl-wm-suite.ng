using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dl.wm.suite.cms.api.Controllers.API.Base;
using dl.wm.suite.cms.api.Validators;
using dl.wm.suite.cms.contracts.Employees.EmployeeRoles;
using dl.wm.suite.cms.contracts.Users;
using dl.wm.suite.cms.contracts.V1;
using dl.wm.suite.cms.model.Employees.EmployeeRoles;
using dl.wm.suite.common.dtos.Links;
using dl.wm.suite.common.dtos.Vms.Employees.EmployeeRoles;
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
    public class EmployeeRolesController : BaseController
    {
        private readonly IUrlHelper _urlHelper;
        private readonly ITypeHelperService _typeHelperService;
        private readonly IPropertyMappingService _propertyMappingService;

        private readonly IInquiryAllEmployeeRolesProcessor _inquiryAllEmployeeRolesProcessor;
        private readonly IInquiryEmployeeRoleProcessor _inquiryEmployeeRoleProcessor;
        private readonly ICreateEmployeeRoleProcessor _createEmployeeRoleProcessor;
        private readonly IUpdateEmployeeRoleProcessor _updateEmployeeRoleProcessor;

        private readonly IInquiryUserProcessor _inquiryUserProcessor;


        public EmployeeRolesController(IUrlHelper urlHelper,
            ITypeHelperService typeHelperService,
            IPropertyMappingService propertyMappingService,
            IEmployeeRolesControllerDependencyBlock blockEmployeeRole,
            IUsersControllerDependencyBlock blockUser)
        {
            _urlHelper = urlHelper;
            _typeHelperService = typeHelperService;
            _propertyMappingService = propertyMappingService;

            _inquiryAllEmployeeRolesProcessor = blockEmployeeRole.InquiryAllEmployeeRolesProcessor;
            _inquiryEmployeeRoleProcessor = blockEmployeeRole.InquiryEmployeeRoleProcessor;
            _createEmployeeRoleProcessor = blockEmployeeRole.CreateEmployeeRoleProcessor;
            _updateEmployeeRoleProcessor = blockEmployeeRole.UpdateEmployeeRoleProcessor;

            _inquiryUserProcessor = blockUser.InquiryUserProcessor;
        }

        /// <summary>
        /// POST : Create a New Employee Role.
        /// </summary>
        /// <param name="employeeRoleForCreationUiModel">EmployeeRoleForCreationUiModel the Request Model for Creation</param>
        /// <remarks> return a ResponseEntity with status 201 (Created) if the new Employee Role is created, 400 (Bad Request), 500 (Server Error) </remarks>
        /// <response code="201">Created (if the Employee Role is created)</response>
        /// <response code="400">Bad Request</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPost(Name = "PostEmployeeRoleRoute")]
        [ValidateModel]
        public async Task<IActionResult> PostEmployeeRoleRouteAsync(
            [FromBody] EmployeeRoleForCreationUiModel employeeRoleForCreationUiModel)
        {
            var userAudit = await _inquiryUserProcessor.GetUserByLoginAsync(GetEmailFromClaims());

            if (userAudit == null)
                return BadRequest();

            var newCreatedRole =
                await _createEmployeeRoleProcessor.CreateEmployeeRoleAsync(userAudit.Id,
                    employeeRoleForCreationUiModel);

            switch (newCreatedRole.Message)
            {
                case ("SUCCESS_CREATION"):
                {
                    Log.Information(
                        $"--Method:PostRoleRouteAsync -- Message:ROLE_CREATION_SUCCESSFULLY -- " +
                        $"Datetime:{DateTime.Now} -- RoleInfo:{employeeRoleForCreationUiModel.EmployeeRoleName}");
                    return Created(nameof(PostEmployeeRoleRouteAsync), newCreatedRole);
                }
                case ("ERROR_ALREADY_EXISTS"):
                {
                    Log.Error(
                        $"--Method:PostRoleRouteAsync -- Message:ERROR_ROLE_ALREADY_EXISTS -- " +
                        $"Datetime:{DateTime.UtcNow} -- RoleInfo:{employeeRoleForCreationUiModel.EmployeeRoleName}");
                    return BadRequest(new {errorMessage = "ROLE_ALREADY_EXISTS"});
                }
                case ("ERROR_ROLE_NOT_MADE_PERSISTENT"):
                {
                    Log.Error(
                        $"--Method:PostRoleRouteAsync -- Message:ERROR_ROLE_NOT_MADE_PERSISTENT -- " +
                        $"Datetime:{DateTime.UtcNow} -- RoleInfo:{employeeRoleForCreationUiModel.EmployeeRoleName}");
                    return BadRequest(new {errorMessage = "ERROR_CREATION_NEW_ROLE"});
                }
                case ("UNKNOWN_ERROR"):
                {
                    Log.Error(
                        $"--Method:PostRoleRouteAsync -- Message:ERROR_CREATION_NEW_ROLE -- " +
                        $"Datetime:{DateTime.UtcNow} -- RoleInfo:{employeeRoleForCreationUiModel.EmployeeRoleName}");
                    return BadRequest(new {errorMessage = "ERROR_CREATION_NEW_ROLE"});
                }
            }

            return NotFound();
        }

        /// <summary>
        /// PUT : Update Employee Role with New Employee Role Name
        /// </summary>
        /// <param name="id">Employee Role Id the Request Index for Retrieval</param>
        /// <param name="updatedEmployeeRole">EmployeeRoleForModification the Request Model with New Employee Role Name</param>
        /// <remarks>Change EmployeeRole providing EmployeeRoleForModificationUiModel with Modified Employee Role Name</remarks>
        /// <response code="200">Resource updated correctly.</response>
        /// <response code="400">The model is not in valid state.</response>
        /// <response code="403">You have not access for this action.</response>
        /// <response code="404">Wrong attributes provided.</response>
        /// <response code="500">Internal Server Error.</response>
        [HttpPut("{id}", Name = "UpdateEmployeeRoleWithModifiedEmployeeRole")]
        [ValidateModel]
        public async Task<IActionResult> UpdateEmployeeRoleWithModifiedEmployeeRoleAsync(Guid id,
            [FromBody] EmployeeRoleForModificationUiModel updatedEmployeeRole)
        {
            if (id == Guid.Empty || String.IsNullOrEmpty(updatedEmployeeRole.EmployeeRoleName))
                return BadRequest();

            var userAudit = await _inquiryUserProcessor.GetUserByLoginAsync(GetEmailFromClaims());

            if (userAudit == null)
                return BadRequest();

            await _updateEmployeeRoleProcessor.UpdateEmployeeRoleAsync(id, userAudit.Id, updatedEmployeeRole);

            return Ok(await _inquiryEmployeeRoleProcessor.GetEmployeeRoleByIdAsync(id));
        }


        /// <summary>
        /// Get : Retrieve Stored Role providing Employee Role Id
        /// </summary>
        /// <param name="id">Employee Role Id the Request Index for Retrieval</param>
        /// <param name="fields">Fiends to be filtered with for the returned Employee Role</param>
        /// <remarks>Retrieve Employee Role providing Id and [Optional] fields</remarks>
        /// <response code="200">Resource retrieved correctly</response>
        /// <response code="404">Resource Not Found</response>
        /// <response code="500">Internal Server Error.</response>
        [HttpGet("{id}", Name = "GetEmployeeRole")]
        public async Task<IActionResult> GetEmployeeRoleAsync(Guid id, [FromQuery] string fields)
        {
            if (!_typeHelperService.TypeHasProperties<EmployeeRoleUiModel>
                (fields))
            {
                return BadRequest();
            }

            var employeeRoleFromRepo = await _inquiryEmployeeRoleProcessor.GetEmployeeRoleByIdAsync(id);

            if (employeeRoleFromRepo == null)
            {
                return NotFound();
            }

            var employeeRole = Mapper.Map<EmployeeRoleUiModel>(employeeRoleFromRepo);

            var links = CreateLinksForEmployeeRole(id, fields);

            var linkedResourceToReturn = employeeRole.ShapeData(fields)
                as IDictionary<string, object>;

            linkedResourceToReturn.Add("links", links);

            return Ok(linkedResourceToReturn);
        }

        /// <summary>
        /// Get : Retrieve All/or Partial Paged Stored Employee Roles
        /// </summary>
        /// <remarks>Retrieve paged Employee Roles providing Paging Query</remarks>
        /// <response code="200">Resource retrieved correctly.</response>
        /// <response code="500">Internal Server Error.</response>
        [HttpGet(Name = "GetEmployeeRoles")]
        public async Task<IActionResult> GetEmployeeRolesAsync(
            [FromQuery] EmployeeRolesResourceParameters employeeRolesResourceParameters,
            [FromHeader(Name = "Accept")] string mediaType)
        {
            if (!_propertyMappingService.ValidMappingExistsFor<EmployeeRoleUiModel, EmployeeRole>
                (employeeRolesResourceParameters.OrderBy))
            {
                return BadRequest();
            }

            if (!_typeHelperService.TypeHasProperties<EmployeeRoleUiModel>
                (employeeRolesResourceParameters.Fields))
            {
                return BadRequest();
            }

            var employeeRolesQueryable =
                await _inquiryAllEmployeeRolesProcessor.GetEmployeeRolesAsync(employeeRolesResourceParameters);

            var roles = Mapper.Map<IEnumerable<EmployeeRoleUiModel>>(employeeRolesQueryable);

            if (mediaType.Contains("application/vnd.marvin.hateoas+json"))
            {
                var paginationMetadata = new
                {
                    totalCount = employeeRolesQueryable.TotalCount,
                    pageSize = employeeRolesQueryable.PageSize,
                    currentPage = employeeRolesQueryable.CurrentPage,
                    totalPages = employeeRolesQueryable.TotalPages,
                };

                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(paginationMetadata));

                var links = CreateLinksForEmployeeRoles(employeeRolesResourceParameters,
                    employeeRolesQueryable.HasNext, employeeRolesQueryable.HasPrevious);

                var shapedPersons = roles.ShapeData(employeeRolesResourceParameters.Fields);

                var shapedPersonsWithLinks = shapedPersons.Select(person =>
                {
                    var personAsDictionary = person as IDictionary<string, object>;
                    var personLinks =
                        CreateLinksForEmployeeRole((Guid) personAsDictionary["Id"],
                            employeeRolesResourceParameters.Fields);

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
                var previousPageLink = employeeRolesQueryable.HasPrevious
                    ? CreateEmployeeRolesResourceUri(employeeRolesResourceParameters,
                        ResourceUriType.PreviousPage)
                    : null;

                var nextPageLink = employeeRolesQueryable.HasNext
                    ? CreateEmployeeRolesResourceUri(employeeRolesResourceParameters,
                        ResourceUriType.NextPage)
                    : null;

                var paginationMetadata = new
                {
                    previousPageLink = previousPageLink,
                    nextPageLink = nextPageLink,
                    totalCount = employeeRolesQueryable.TotalCount,
                    pageSize = employeeRolesQueryable.PageSize,
                    currentPage = employeeRolesQueryable.CurrentPage,
                    totalPages = employeeRolesQueryable.TotalPages
                };

                Response.Headers.Add("X-Pagination",
                    JsonConvert.SerializeObject(paginationMetadata));

                return Ok(roles.ShapeData(employeeRolesResourceParameters.Fields));
            }
        }

        #region Link Builder

        private IEnumerable<LinkDto> CreateLinksForEmployeeRole(Guid id, string fields)
        {
            var links = new List<LinkDto>();

            if (String.IsNullOrWhiteSpace(fields))
            {
                links.Add(
                    new LinkDto(_urlHelper.Link("GetEmployeeRole", new {id = id}),
                        "self",
                        "GET"));
            }
            else
            {
                links.Add(
                    new LinkDto(_urlHelper.Link("GetEmployeeRole", new {id = id, fields = fields}),
                        "self",
                        "GET"));
            }

            return links;
        }


        private IEnumerable<LinkDto> CreateLinksForEmployeeRoles(
            EmployeeRolesResourceParameters employeeRolesResourceParameters,
            bool hasNext, bool hasPrevious)
        {
            var links = new List<LinkDto>
            {
                new LinkDto(CreateEmployeeRolesResourceUri(employeeRolesResourceParameters,
                        ResourceUriType.Current)
                    , "self", "GET")
            };

            if (hasNext)
            {
                links.Add(
                    new LinkDto(CreateEmployeeRolesResourceUri(employeeRolesResourceParameters,
                            ResourceUriType.NextPage),
                        "nextPage", "GET"));
            }

            if (hasPrevious)
            {
                links.Add(
                    new LinkDto(CreateEmployeeRolesResourceUri(employeeRolesResourceParameters,
                            ResourceUriType.PreviousPage),
                        "previousPage", "GET"));
            }

            return links;
        }

        private string CreateEmployeeRolesResourceUri(EmployeeRolesResourceParameters employeeRolesResourceParameters,
            ResourceUriType type)
        {
            switch (type)
            {
                case ResourceUriType.PreviousPage:
                    return _urlHelper.Link("GetEmployeeRoles",
                        new
                        {
                            fields = employeeRolesResourceParameters.Fields,
                            orderBy = employeeRolesResourceParameters.OrderBy,
                            searchQuery = employeeRolesResourceParameters.SearchQuery,
                            pageNumber = employeeRolesResourceParameters.PageIndex - 1,
                            pageSize = employeeRolesResourceParameters.PageSize
                        });
                case ResourceUriType.NextPage:
                    return _urlHelper.Link("GetEmployeeRoles",
                        new
                        {
                            fields = employeeRolesResourceParameters.Fields,
                            orderBy = employeeRolesResourceParameters.OrderBy,
                            searchQuery = employeeRolesResourceParameters.SearchQuery,
                            pageNumber = employeeRolesResourceParameters.PageIndex + 1,
                            pageSize = employeeRolesResourceParameters.PageSize
                        });
                case ResourceUriType.Current:
                default:
                    return _urlHelper.Link("GetEmployeeRoles",
                        new
                        {
                            fields = employeeRolesResourceParameters.Fields,
                            orderBy = employeeRolesResourceParameters.OrderBy,
                            searchQuery = employeeRolesResourceParameters.SearchQuery,
                            pageNumber = employeeRolesResourceParameters.PageIndex,
                            pageSize = employeeRolesResourceParameters.PageSize
                        });
            }
        }

        #endregion
    }
}
