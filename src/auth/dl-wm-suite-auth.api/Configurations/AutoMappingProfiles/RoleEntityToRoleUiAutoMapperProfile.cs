using dl.wm.suite.auth.api.Helpers.Models;
using dl.wm.suite.common.dtos.Vms.Roles;
using AutoMapper;

namespace dl.wm.suite.auth.api.Configurations.AutoMappingProfiles
{
    public class RoleEntityToRoleUiAutoMapperProfile : Profile
    {
        public RoleEntityToRoleUiAutoMapperProfile()
        {
            ConfigureMapping();
        }

        public void ConfigureMapping()
        {
            CreateMap<Role, RoleUiModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => src.CreatedDate))
                .ForMember(dest => dest.ModifiedDate, opt => opt.MapFrom(src => src.ModifiedDate))
                .ForMember(dest => dest.Active, opt => opt.MapFrom(src => src.IsActive))
                .MaxDepth(1)
                ;
        }
    }
}