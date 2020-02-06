using dl.wm.suite.cms.model.Vehicles;
using dl.wm.suite.common.dtos.Vms.Vehicles;
using AutoMapper;

namespace dl.wm.suite.cms.api.Configurations.AutoMappingProfiles.Vehicles
{
    public class VehicleEntityToVehicleUiAutoMapperProfile : Profile
    {
        public VehicleEntityToVehicleUiAutoMapperProfile()
        {
            ConfigureMapping();
        }

        public void ConfigureMapping()
        {
            CreateMap<Vehicle, VehicleUiModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.VehicleBrand, opt => opt.MapFrom(src => src.Brand))
                .ForMember(dest => dest.VehicleNumPlate, opt => opt.MapFrom(src => src.NumPlate))
                .ForMember(dest => dest.VehicleActive, opt => opt.MapFrom(src => src.IsActive))
                .ForMember(dest => dest.VehicleType, opt => opt.MapFrom(src => src.Type))
                .ForMember(dest => dest.VehicleStatus, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.VehicleGas, opt => opt.MapFrom(src => src.Gas))
                .ForMember(dest => dest.VehicleRegisteredDate, opt => opt.MapFrom(src => src.RegisteredDate))
                .ForMember(dest => dest.VehicleValue, opt => opt.Ignore())
                .MaxDepth(1)
                .PreserveReferences()
                ;
            
        }
    }
}