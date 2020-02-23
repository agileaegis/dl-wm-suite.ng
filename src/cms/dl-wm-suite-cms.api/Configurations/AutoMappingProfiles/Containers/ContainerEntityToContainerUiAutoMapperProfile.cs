using dl.wm.suite.cms.model.Containers;
using dl.wm.suite.common.dtos.Vms.Containers;
using AutoMapper;
using GeoAPI.Geometries;

namespace dl.wm.suite.cms.api.Configurations.AutoMappingProfiles.Containers
{
    public class ContainerEntityToContainerUiAutoMapperProfile : Profile
    {
        public ContainerEntityToContainerUiAutoMapperProfile()
        {
            ConfigureMapping();
        }

        public void ConfigureMapping()
        {
            CreateMap<Container, ContainerUiModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.ContainerName, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.ContainerLevel, opt => opt.MapFrom(src => src.Level))
                .ForMember(dest => dest.ContainerFillLevel, opt => opt.MapFrom(src => src.FillLevel))
                .ForMember(dest => dest.ContainerLocationLat, opt => opt.MapFrom(src => src.Geo.Coordinate.X))
                .ForMember(dest => dest.ContainerLocationLong, opt => opt.MapFrom(src => src.Geo.Coordinate.Y))
                .ForMember(dest => dest.ContainerImageName, opt => opt.MapFrom(src => src.ImagePath))
                .ForMember(dest => dest.ContainerTimeFull, opt => opt.MapFrom(src => src.TimeFull))
                .ForMember(dest => dest.ContainerLastServicedDate, opt => opt.MapFrom(src => src.LastServicedDate))
                .ForMember(dest => dest.ContainerCreatedDate, opt => opt.MapFrom(src => src.CreatedDate))
                .ForMember(dest => dest.ContainerCreatedBy, opt => opt.MapFrom(src => src.CreatedBy))
                .ForMember(dest => dest.ContainerModifiedDate, opt => opt.MapFrom(src => src.ModifiedDate))
                .ForMember(dest => dest.ContainerModifiedBy, opt => opt.MapFrom(src => src.ModifiedBy))
                .ForMember(dest => dest.ContainerStatus, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.ContainerStatusValue, opt => opt.MapFrom(src => ((int)src.Status).ToString()))
                .ForMember(dest => dest.ContainerType, opt => opt.MapFrom(src => src.Type))
                .ForMember(dest => dest.ContainerTypeValue, opt => opt.MapFrom(src => ((int)src.Type).ToString()))
                .ForMember(dest => dest.ContainerStatus, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.ContainerAddress, opt => opt.MapFrom(src => src.Address))
                .ForMember(dest => dest.ContainerMandatoryPickupDate, opt => opt.MapFrom(src => src.MandatoryPickupDate))
                .ForMember(dest => dest.ContainerMandatoryPickupActive, opt => opt.MapFrom(src => src.MandatoryPickupActive))
                .ForMember(dest => dest.ContainerCapacity, opt => opt.MapFrom(src => src.Capacity))
                .ForMember(dest => dest.ContainerFixed, opt => opt.MapFrom(src => src.IsFixed))
                .ForMember(dest => dest.ContainerDescription, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.ContainerLoad, opt => opt.MapFrom(src => src.UsefulLoad))
                .ForMember(dest => dest.ContainerWasteType, opt => opt.MapFrom(src => src.WasteType))
                .ForMember(dest => dest.ContainerWasteTypeValue, opt => opt.MapFrom(src => ((int)src.WasteType).ToString()))
                .ForMember(dest => dest.ContainerMaterial, opt => opt.MapFrom(src => src.Material))
                .ForMember(dest => dest.ContainerMaterialValue, opt => opt.MapFrom(src => ((int)src.Material).ToString()))
                .MaxDepth(1)
                .PreserveReferences()
                ;
            
        }
    }
}