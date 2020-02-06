using dl.wm.suite.auth.api.Helpers.Models;
using dl.wm.suite.common.dtos.Vms.Users;
using AutoMapper;

namespace dl.wm.suite.auth.api.Configurations.AutoMappingProfiles
{
    public class UserEntityToUserActivationUiAutoMapperProfile : Profile
    {
        public UserEntityToUserActivationUiAutoMapperProfile()
        {
            ConfigureMapping();
        }

        public void ConfigureMapping()
        {
            CreateMap<User, UserActivationUiModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Login, opt => opt.MapFrom(src => src.Login))
                .ForMember(dest => dest.IsActivated, opt => opt.MapFrom(src => src.IsActivated))
                .ForMember(dest => dest.CreateDate, opt => opt.MapFrom(src => src.CreatedDate))
                .MaxDepth(1)
                ;
        }
    }
}