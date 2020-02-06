using dl.wm.suite.cms.model.Vehicles;
using dl.wm.suite.common.dtos.Vms.Vehicles;
using AutoMapper;

namespace dl.wm.suite.cms.api.Configurations.AutoMappingProfiles.Vehicles
{
    public class VehicleForCreationUiModelToVehicleEntityAutoMapperProfile : Profile
    {
        public VehicleForCreationUiModelToVehicleEntityAutoMapperProfile()
        {
            ConfigureMapping();
        }

        public void ConfigureMapping()
        {
            CreateMap<VehicleForCreationUiModel, Vehicle>()
                .ForMember(dest => dest.Brand, opt => opt.MapFrom(src => src.VehicleBrand))
                .ForMember(dest => dest.NumPlate, opt => opt.MapFrom(src => src.VehicleNumPlate))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.VehicleActive))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.VehicleType))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.VehicleStatus))
                .ForMember(dest => dest.Gas, opt => opt.MapFrom(src => src.VehicleGas))
                .ForMember(dest => dest.Tours, opt => opt.Ignore())
                .MaxDepth(1)
                .PreserveReferences()
                ;
        }
    }
}