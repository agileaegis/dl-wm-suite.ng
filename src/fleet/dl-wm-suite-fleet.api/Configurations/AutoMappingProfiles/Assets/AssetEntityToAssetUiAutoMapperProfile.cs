using dl.wm.suite.common.dtos.Vms.Assets;
using dl.wm.suite.fleet.model.Assets;
using AutoMapper;

namespace dl.wm.suite.fleet.api.Configurations.AutoMappingProfiles.Assets
{
  public class AssetEntityToAssetUiAutoMapperProfile : Profile
  {
    public AssetEntityToAssetUiAutoMapperProfile()
    {
      ConfigureMapping();
    }

    public void ConfigureMapping()
    {
      CreateMap<Asset, AssetUiModel>()
          .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
          .ForMember(dest => dest.AssetName, opt => opt.MapFrom(src => src.Name))
          .ForMember(dest => dest.AssetNumPlate, opt => opt.MapFrom(src => src.NumPlate))
          .ForMember(dest => dest.AssetType, opt => opt.MapFrom(src => src.Type))
          .ForMember(dest => dest.AssetHeight, opt => opt.MapFrom(src => src.Height))
          .ForMember(dest => dest.AssetWidth, opt => opt.MapFrom(src => src.Width))
          .ForMember(dest => dest.AssetLength, opt => opt.MapFrom(src => src.Length))
          .ForMember(dest => dest.AssetAxels, opt => opt.MapFrom(src => src.Axels))
          .ForMember(dest => dest.AssetTrailers, opt => opt.MapFrom(src => src.Trailers))
          .ForMember(dest => dest.AssetIsSemi, opt => opt.MapFrom(src => src.IsSemi))
          .ForMember(dest => dest.AssetMaxGradient, opt => opt.MapFrom(src => src.MaxGradient))
          .ForMember(dest => dest.AssetMinTurnRadius, opt => opt.MapFrom(src => src.MinTurnRadius))
          .MaxDepth(1)
          .PreserveReferences()
          ;

    }
  }
}