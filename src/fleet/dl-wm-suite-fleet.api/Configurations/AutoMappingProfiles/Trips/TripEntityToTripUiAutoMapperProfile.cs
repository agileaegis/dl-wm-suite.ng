using dl.wm.suite.common.dtos.Vms.Trips;
using dl.wm.suite.fleet.model.Trips;
using AutoMapper;

namespace dl.wm.suite.fleet.api.Configurations.AutoMappingProfiles.Trips
{
  public class TripEntityToTripUiAutoMapperProfile : Profile
  {
    public TripEntityToTripUiAutoMapperProfile()
    {
      ConfigureMapping();
    }

    public void ConfigureMapping()
    {
      CreateMap<Trip, TripUiModel>()
          .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
          .ForMember(dest => dest.TripCode, opt => opt.MapFrom(src => src.Code))
          .ForMember(dest => dest.TripStartTime, opt => opt.MapFrom(src => src.StartTime))
          .ForMember(dest => dest.TripEndTime, opt => opt.MapFrom(src => src.EndTime))
          .ForMember(dest => dest.TripAssetId, opt => opt.MapFrom(src => src.DeviceAsset.Asset.Id))
          .ForMember(dest => dest.TripDeviceId, opt => opt.MapFrom(src => src.DeviceAsset.Device.Id))
          .ForMember(dest => dest.Message, opt => opt.Ignore())
          .MaxDepth(1)
          .PreserveReferences()
          ;

    }
  }
}