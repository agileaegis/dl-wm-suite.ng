using dl.wm.suite.cms.model.Containers;
using dl.wm.suite.common.dtos.Vms.Containers;
using AutoMapper;
using GeoAPI.Geometries;

namespace dl.wm.suite.cms.api.Configurations.AutoMappingProfiles.Containers
{
    public class ContainerEntityToContainerPointUiAutoMapperProfile : Profile
    {
        public ContainerEntityToContainerPointUiAutoMapperProfile()
        {
            ConfigureMapping();
        }

        public void ConfigureMapping()
        {
            CreateMap<Container, ContainerPointUiModel>()
                .ForMember(dest => dest.Id, opt => 
                  opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.ContainerLat, opt => 
                  opt.MapFrom(src => src.Location.Coordinate.X))
                .ForMember(dest => dest.ContainerLon, opt =>
                  opt.MapFrom(src => src.Location.Coordinate.Y))
                .ForMember(dest => dest.ContainerPointType, opt => 
                  opt.MapFrom(src => src.Location.GeometryType))
                .MaxDepth(1)
                .PreserveReferences()
                ;
            
        }
    }
}