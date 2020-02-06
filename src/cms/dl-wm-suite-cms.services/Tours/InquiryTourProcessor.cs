using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dl.wm.suite.cms.contracts.Tours;
using dl.wm.suite.cms.repository.ContractRepositories;
using dl.wm.suite.common.dtos.Vms.Tours;
using dl.wm.suite.common.infrastructure.PropertyMappings;
using dl.wm.suite.common.infrastructure.TypeMappings;

namespace dl.wm.suite.cms.services.Tours
{
    public class InquiryTourProcessor : IInquiryTourProcessor
    {
        private readonly IAutoMapper _autoMapper;
        private readonly ITourRepository _tourRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IPropertyMappingService _propertyMappingService;

        public InquiryTourProcessor(IAutoMapper autoMapper,
            ITourRepository tourRepository, IEmployeeRepository employeeRepository,  IPropertyMappingService propertyMappingService)
        {
            _autoMapper = autoMapper;
            _tourRepository = tourRepository;
            _employeeRepository = employeeRepository;
            _propertyMappingService = propertyMappingService;
        }

        public Task<TourUiModel> GetTourAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<TourForAssignTrackableModel> GetTodayAssignedTourAsync(Guid userId)
        {
            var employeeToBeRetrieved = _employeeRepository.FindEmployeeByUserId(userId);
            if(employeeToBeRetrieved == null)
                throw new Exception();

            var toursToBeRetrieved = _tourRepository.FindTodayTourByEmployee(employeeToBeRetrieved.Id);

            if(toursToBeRetrieved == null || toursToBeRetrieved.Count > 1)
                throw new Exception();

            var tourForAssignedToRetrieved = _autoMapper.Map<TourForAssignTrackableModel>(toursToBeRetrieved.First());
            tourForAssignedToRetrieved.EmployeeId = employeeToBeRetrieved.Id;
            return Task.Run(() =>  tourForAssignedToRetrieved);
        }
    }
}
