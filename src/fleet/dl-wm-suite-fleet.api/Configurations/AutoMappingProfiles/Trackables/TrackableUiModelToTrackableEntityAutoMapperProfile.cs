using dl.wm.suite.common.dtos.Vms.Trackables;
using dl.wm.suite.fleet.model.Trackables;
using AutoMapper;

namespace dl.wm.suite.fleet.api.Configurations.AutoMappingProfiles.Trackables
{
  public class TrackableUiModelToTrackableEntityAutoMapperProfile : Profile
  {
    public TrackableUiModelToTrackableEntityAutoMapperProfile()
    {
      ConfigureMapping();
    }

    public void ConfigureMapping()
    {
      CreateMap<TrackableUiModel, Trackable>()
        .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
        .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.TrackableName))
        .ForMember(dest => dest.Model, opt => opt.MapFrom(src => src.TrackableModel))
        .ForMember(dest => dest.VendorId, opt => opt.MapFrom(src => src.TrackableImei))
        .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.TrackablePhone))
        .ForMember(dest => dest.Os, opt => opt.MapFrom(src => src.TrackableOs))
        .ForMember(dest => dest.Version, opt => opt.MapFrom(src => src.TrackableVersion))
        .ForMember(dest => dest.Notes, opt => opt.MapFrom(src => src.TrackableNotes))
        .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => src.TrackableCreatedDate))
        .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
        .ForMember(dest => dest.IsActive, opt => opt.Ignore())
        .MaxDepth(1)
        .PreserveReferences()
        ;
    }
  }
}