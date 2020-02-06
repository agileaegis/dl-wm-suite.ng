using System.Linq;
using System.Threading.Tasks;
using dl.wm.suite.auth.api.Helpers.Models;
using dl.wm.suite.auth.api.Helpers.Repositories.Users;
using dl.wm.suite.auth.api.Helpers.Services.Users.Contracts;
using dl.wm.suite.common.dtos.Vms.Users;
using dl.wm.suite.common.infrastructure.Extensions;
using dl.wm.suite.common.infrastructure.Helpers.ResourceParameters;
using dl.wm.suite.common.infrastructure.Paging;
using dl.wm.suite.common.infrastructure.PropertyMappings;
using dl.wm.suite.common.infrastructure.TypeMappings;

namespace dl.wm.suite.auth.api.Helpers.Services.Users.Impls
{
    public class InquiryAllUsersProcessor : IInquiryAllUsersProcessor
    {
        private readonly IAutoMapper _autoMapper;
        private readonly IUserRepository _personRepository;
        private readonly IPropertyMappingService _propertyMappingService;

        public InquiryAllUsersProcessor(IAutoMapper autoMapper,
            IUserRepository personRepository, IPropertyMappingService propertyMappingService)
        {
            _autoMapper = autoMapper;
            _personRepository = personRepository;
            _propertyMappingService = propertyMappingService;
        }

        public Task<PagedList<User>> GetUsersAsync(UsersResourceParameters personsResourceParameters)
        {
            var collectionBeforePaging =
                QueryableExtensions.ApplySort(_personRepository
                        .GetUsersPagedAsync(personsResourceParameters.PageIndex,
                            personsResourceParameters.PageSize),
                    personsResourceParameters.OrderBy,
                    _propertyMappingService.GetPropertyMapping<UserUiModel, User>());


            if (!string.IsNullOrEmpty(personsResourceParameters.SearchQuery))
            {
                // trim & ignore casing
                var searchQueryForWhereClause = personsResourceParameters.SearchQuery
                    .Trim().ToLowerInvariant();

                collectionBeforePaging.QueriedItems = collectionBeforePaging.QueriedItems
                    .Where(a => a.Login.ToLowerInvariant().Contains(searchQueryForWhereClause));
            }

            return Task.Run(() => PagedList<User>.Create(collectionBeforePaging,
                personsResourceParameters.PageIndex,
                personsResourceParameters.PageSize));
        }
    }
}