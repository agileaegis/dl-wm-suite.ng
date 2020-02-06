using dl.wm.suite.cms.model.Tours;
using dl.wm.suite.common.dtos.Vms.Tours;
using AutoMapper;

namespace dl.wm.suite.cms.api.Configurations.AutoMappingProfiles.Tours
{
    public class TourEntityToTourForCreationUiAutoMapperProfile : Profile
    {
        public TourEntityToTourForCreationUiAutoMapperProfile()
        {
            ConfigureMapping();
        }

        public void ConfigureMapping()
        {
            CreateMap<Tour, TourForAssignTrackableModel>()
                .ForMember(dest => dest.TourId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.ScheduledDate, opt => opt.MapFrom(src => src.ScheduledDate))
                .ForMember(dest => dest.AssetNumplate, opt => opt.MapFrom(src => src.Vehicle.NumPlate))
                .ForMember(dest => dest.EmployeeId, opt => opt.Ignore())
                .MaxDepth(1)
                .PreserveReferences()
                ;
            
        }
    }
}