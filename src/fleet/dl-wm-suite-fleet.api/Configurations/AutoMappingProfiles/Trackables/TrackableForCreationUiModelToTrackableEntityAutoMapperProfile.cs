using dl.wm.suite.common.dtos.Vms.Trackables;
using dl.wm.suite.fleet.model.Trackables;
using AutoMapper;

namespace dl.wm.suite.fleet.api.Configurations.AutoMappingProfiles.Trackables
{
  public class TrackableForCreationUiModelToTrackableEntityAutoMapperProfile : Profile
  {
    public TrackableForCreationUiModelToTrackableEntityAutoMapperProfile()
    {
      ConfigureMapping();
    }

    public void ConfigureMapping()
    {
      CreateMap<TrackableForCreationUiModel, Trackable>()
        .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.TrackableName))
        .ForMember(dest => dest.Model, opt => opt.MapFrom(src => src.TrackableModel))
        .ForMember(dest => dest.VendorId, opt => opt.MapFrom(src => src.TrackableImei))
        .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.TrackablePhone))
        .ForMember(dest => dest.Os, opt => opt.MapFrom(src => src.TrackableOs))
        .ForMember(dest => dest.Version, opt => opt.MapFrom(src => src.TrackableVersion))
        .ForMember(dest => dest.Notes, opt => opt.MapFrom(src => src.TrackableNotes))
        .MaxDepth(1)
        .PreserveReferences()
        ;
    }
  }
}