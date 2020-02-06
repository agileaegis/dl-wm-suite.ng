using dl.wm.suite.common.dtos.Vms.Trips;
using dl.wm.suite.fleet.model.Trips;
using AutoMapper;

namespace dl.wm.suite.fleet.api.Configurations.AutoMappingProfiles.Trips
{
    public class TripForCreationUiModelToTripEntityAutoMapperProfile : Profile
    {
        public TripForCreationUiModelToTripEntityAutoMapperProfile()
        {
            ConfigureMapping();
        }

        public void ConfigureMapping()
        {
            CreateMap<TripForCreationUiModel, Trip>()
                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.TripCode))
                .MaxDepth(1)
                .PreserveReferences()
                ;
        }
    }
}