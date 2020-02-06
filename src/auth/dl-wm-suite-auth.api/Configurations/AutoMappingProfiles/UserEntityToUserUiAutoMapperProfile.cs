using System.Linq;
using dl.wm.suite.auth.api.Helpers.Models;
using dl.wm.suite.common.dtos.Vms.Users;
using AutoMapper;

namespace dl.wm.suite.auth.api.Configurations.AutoMappingProfiles
{
    public class UserEntityToUserUiAutoMapperProfile : Profile
    {
        public UserEntityToUserUiAutoMapperProfile()
        {
            ConfigureMapping();
        }

        public void ConfigureMapping()
        {
            CreateMap<User, UserUiModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Login, opt => opt.MapFrom(src => src.Login))
                .ForMember(dest => dest.Roles, opt => opt
                    .MapFrom(src => src.UsersRoles.Select(x=>x.Role).ToList()))
                .ForMember(dest => dest.IsActivated, opt => opt.MapFrom(src => src.IsActivated))
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy))
                .ForMember(dest => dest.CreateDate, opt => opt.MapFrom(src => src.CreatedDate))
                .ForMember(dest => dest.ResetDate, opt => opt.MapFrom(src => src.ResetDate))
                .ForMember(dest => dest.LastModifiedBy, opt => opt.MapFrom(src => src.ModifiedBy))
                .MaxDepth(1)
                ;
        }
    }
}