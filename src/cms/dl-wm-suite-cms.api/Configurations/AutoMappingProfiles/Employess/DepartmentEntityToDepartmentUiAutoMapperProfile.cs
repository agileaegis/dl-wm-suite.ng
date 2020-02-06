using dl.wm.suite.cms.model.Employees.Departments;
using dl.wm.suite.common.dtos.Vms.Employees.Departments;
using AutoMapper;

namespace dl.wm.suite.cms.api.Configurations.AutoMappingProfiles.Employess
{
    public class DepartmentEntityToDepartmentUiAutoMapperProfile : Profile
    {
        public DepartmentEntityToDepartmentUiAutoMapperProfile()
        {
            ConfigureMapping();
        }

        public void ConfigureMapping()
        {
            CreateMap<Department, DepartmentUiModel>()
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