using dl.wm.suite.common.dtos.Vms.Trackables;
using dl.wm.suite.fleet.model.Trackables;
using AutoMapper;

namespace dl.wm.suite.fleet.api.Configurations.AutoMappingProfiles.Trackables
{
  public class TrackableEntityToTrackableForCreationUiAutoMapperProfile : Profile
  {
    public TrackableEntityToTrackableForCreationUiAutoMapperProfile()
    {
      ConfigureMapping();
    }

    public void ConfigureMapping()
    {
      CreateMap<Trackable, TrackableForCreationUiModel>()
        .ForMember(dest => dest.TrackableName, opt => opt.MapFrom(src => src.Name))
        .ForMember(dest => dest.TrackableModel, opt => opt.MapFrom(src => src.Model))
        .ForMember(dest => dest.TrackableImei, opt => opt.MapFrom(src => src.VendorId))
        .ForMember(dest => dest.TrackablePhone, opt => opt.MapFrom(src => src.Phone))
        .ForMember(dest => dest.TrackableOs, opt => opt.MapFrom(src => src.Os))
        .ForMember(dest => dest.TrackableVersion, opt => opt.MapFrom(src => src.Version))
        .ForMember(dest => dest.TrackableNotes, opt => opt.MapFrom(src => src.Notes))
        .MaxDepth(1)
        .PreserveReferences()
        ;

    }
  }
}