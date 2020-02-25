using AutoMapper;
using dl.wm.suite.cms.model.Containers;
using dl.wm.suite.common.dtos.Vms.Containers;

namespace dl.wm.suite.cms.api.Configurations.AutoMappingProfiles.Containers
{
    public class ContainerUiModelToContainerEntityAutoMapperProfile : Profile
    {
        public ContainerUiModelToContainerEntityAutoMapperProfile()
        {
            ConfigureMapping();
        }

        public void ConfigureMapping()
        {
            CreateMap<ContainerForCreationModel, Container>()
                            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.ContainerName))
                            .ForMember(dest => dest.Level, opt => opt.MapFrom(src => src.ContainerLevel))
                            .ForMember(dest => dest.FillLevel, opt => opt.MapFrom(src => src.ContainerFillLevel))
                            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.ContainerType))
                            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.ContainerStatus))
                            .ForMember(dest => dest.ImagePath, opt => opt.MapFrom(src => src.ContainerImageName))
                            .ForMember(dest => dest.MandatoryPickupDate, opt => opt.MapFrom(src => src.ContainerPickupDate))
                            .ForMember(dest => dest.MandatoryPickupActive, opt => opt.MapFrom(src => src.ContainerPickupActive))
                            .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.ContainerAddress))
                            .ForMember(dest => dest.IsFixed, opt => opt.MapFrom(src => src.ContainerFixed))
                            .ForMember(dest => dest.Capacity, opt => opt.MapFrom(src => src.ContainerCapacity))
                            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.ContainerDescription))
                            .ForMember(dest => dest.UsefulLoad, opt => opt.MapFrom(src => src.ContainerLoad))
                            .ForMember(dest => dest.Material, opt => opt.MapFrom(src => src.ContainerMaterial))
                            .ForMember(dest => dest.WasteType, opt => opt.MapFrom(src => src.ContainerWasteType))
                            .MaxDepth(1)
                            .PreserveReferences()
                            ;
        }
    }
}