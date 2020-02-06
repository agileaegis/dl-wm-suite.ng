using dl.wm.suite.common.dtos.Vms.Trips;
using dl.wm.suite.fleet.model.Trips;
using AutoMapper;

namespace dl.wm.suite.fleet.api.Configurations.AutoMappingProfiles.Trips
{
  public class TripUiModelToTripEntityAutoMapperProfile : Profile
  {
    public TripUiModelToTripEntityAutoMapperProfile()
    {
      ConfigureMapping();
    }

    public void ConfigureMapping()
    {
      CreateMap<TripUiModel, Trip>()
        .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
        .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.TripCode))
        .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
        .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
        .MaxDepth(1)
        .PreserveReferences()
        ;
    }
  }
}