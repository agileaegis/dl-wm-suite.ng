using dl.wm.suite.common.dtos.Vms.Trips;
using dl.wm.suite.fleet.model.Trips;
using AutoMapper;

namespace dl.wm.suite.fleet.api.Configurations.AutoMappingProfiles.Trips
{
    public class TripEntityToTripForCreationUiAutoMapperProfile : Profile
    {
        public TripEntityToTripForCreationUiAutoMapperProfile()
        {
            ConfigureMapping();
        }

        public void ConfigureMapping()
        {
            CreateMap<Trip, TripForCreationUiModel>()
                .ForMember(dest => dest.TripCode, opt => opt.MapFrom(src => src.Code))
                .MaxDepth(1)
                .PreserveReferences()
                ;

        }
    }
}