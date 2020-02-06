using dl.wm.suite.cms.model.Employees.EmployeeRoles;
using dl.wm.suite.common.dtos.Vms.Employees.EmployeeRoles;
using AutoMapper;

namespace dl.wm.suite.cms.api.Configurations.AutoMappingProfiles.Employess
{
    public class EmployeeRoleEntityToEmployeeRolUiAutoMapperProfile : Profile
    {
        public EmployeeRoleEntityToEmployeeRolUiAutoMapperProfile()
        {
            ConfigureMapping();
        }

        public void ConfigureMapping()
        {
            CreateMap<EmployeeRole, EmployeeRoleUiModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => src.CreatedDate))
                .ForMember(dest => dest.ModifiedDate, opt => opt.MapFrom(src => src.ModifiedDate))
                .ForMember(dest => dest.Active, opt => opt.MapFrom(src => src.IsActive))
                .ForMember(dest => dest.Message, opt => opt.Ignore())
                .MaxDepth(1)
                ;
        }
    }
}