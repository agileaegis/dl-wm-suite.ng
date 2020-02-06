using dl.wm.suite.common.dtos.Vms.Assets;
using dl.wm.suite.fleet.model.Assets;
using AutoMapper;

namespace dl.wm.suite.fleet.api.Configurations.AutoMappingProfiles.Assets
{
  public class AssetUiModelToAssetEntityAutoMapperProfile : Profile
  {
    public AssetUiModelToAssetEntityAutoMapperProfile()
    {
      ConfigureMapping();
    }

    public void ConfigureMapping()
    {
      CreateMap<AssetUiModel, Asset>()
        .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
        .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.AssetName))
        .ForMember(dest => dest.NumPlate, opt => opt.MapFrom(src => src.AssetNumPlate))
        .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.AssetType))
        .ForMember(dest => dest.Height, opt => opt.MapFrom(src => src.AssetHeight))
        .ForMember(dest => dest.Width, opt => opt.MapFrom(src => src.AssetWidth))
        .ForMember(dest => dest.Length, opt => opt.MapFrom(src => src.AssetLength))
        .ForMember(dest => dest.Width, opt => opt.MapFrom(src => src.AssetWeight))
        .ForMember(dest => dest.Axels, opt => opt.MapFrom(src => src.AssetAxels))
        .ForMember(dest => dest.Trailers, opt => opt.MapFrom(src => src.AssetTrailers))
        .ForMember(dest => dest.IsSemi, opt => opt.MapFrom(src => src.AssetIsSemi))
        .ForMember(dest => dest.MaxGradient, opt => opt.MapFrom(src => src.AssetMaxGradient))
        .ForMember(dest => dest.MinTurnRadius, opt => opt.MapFrom(src => src.AssetMinTurnRadius))
        .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
        .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
        .ForMember(dest => dest.IsActive, opt => opt.Ignore())
        .MaxDepth(1)
        .PreserveReferences()
        ;
    }
  }
}