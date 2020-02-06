using dl.wm.suite.cms.model.Containers;
using dl.wm.suite.common.dtos.Vms.Containers;
using AutoMapper;
using GeoAPI.Geometries;

namespace dl.wm.suite.cms.api.Configurations.AutoMappingProfiles.Containers
{
    public class ContainerEntityToContainerDeletionUiAutoMapperProfile : Profile
    {
        public ContainerEntityToContainerDeletionUiAutoMapperProfile()
        {
            ConfigureMapping();
        }

        public void ConfigureMapping()
        {
            CreateMap<Container, ContainerForDeletionUiModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive))
                .ForMember(dest => dest.Message, opt => opt.Ignore())
                .ForMember(dest => dest.DeletionStatus, opt => opt.Ignore())
                .MaxDepth(1)
                .PreserveReferences()
                ;
            
        }
    }
}