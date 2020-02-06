using dl.wm.suite.cms.model.Vehicles;
using dl.wm.suite.common.dtos.Vms.Vehicles;
using AutoMapper;

namespace dl.wm.suite.cms.api.Configurations.AutoMappingProfiles.Vehicles
{
    public class VehicleUiModelToVehicleEntityAutoMapperProfile : Profile
    {
        public VehicleUiModelToVehicleEntityAutoMapperProfile()
        {
            ConfigureMapping();
        }

        public void ConfigureMapping()
        {
            CreateMap<VehicleUiModel, Vehicle>()
                            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
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