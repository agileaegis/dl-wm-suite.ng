using System.Linq;
using dl.wm.suite.auth.api.Helpers.Models;
using dl.wm.suite.common.dtos.Vms.Users;
using AutoMapper;

namespace dl.wm.suite.auth.api.Configurations.AutoMappingProfiles
{
    public class UserEntityToUserForAllRetrievalUiAutoMapperProfile : Profile
    {
        public UserEntityToUserForAllRetrievalUiAutoMapperProfile()
        {
            ConfigureMapping();
        }

        public void ConfigureMapping()
        {
            CreateMap<User, UserForAllRetrievalUiModel>()
                .ForMember(dest => dest.Id, opt => opt
                    .MapFrom(src => src.Id))
                .ForMember(dest => dest.Login, opt => opt
                    .MapFrom(src => src.Login))
                .ForMember(dest => dest.Roles, opt => opt
                    .MapFrom(src => src.UsersRoles.Select(x=>x.Role).ToList()))
                .ForMember(dest => dest.IsActivated, opt => opt
                    .MapFrom(src => src.IsActivated))
                .ForMember(dest => dest.ActivationKey, opt => opt
                    .MapFrom(src => src.ActivationKey))
                .ForMember(dest => dest.CreatedBy, opt => opt
                    .MapFrom(src => src.CreatedBy))
                .ForMember(dest => dest.ModifiedBy, opt => opt
                    .MapFrom(src => src.ModifiedBy))
                .ForMember(dest => dest.CreateDate, opt => opt
                    .MapFrom(src => src.CreatedDate))
                .ForMember(dest => dest.LastModifiedDate, opt => opt
                    .MapFrom(src => src.ModifiedDate))
                .ForMember(dest => dest.ResetKey, opt => opt
                    .MapFrom(src => src.ResetKey))
                .ForMember(dest => dest.ResetDate, opt => opt
                    .MapFrom(src => src.ResetDate))
                .ForMember(dest => dest.LastModifiedBy, opt => opt
                    .MapFrom(src => src.ModifiedBy))
                .ForMember(dest => dest.Firstname, opt => opt
                    .MapFrom(src => src.Person.Firstname))
                .ForMember(dest => dest.Lastname, opt => opt
                    .MapFrom(src => src.Person.Lastname))
                .ForMember(dest => dest.Email, opt => opt
                    .MapFrom(src => src.Person.Email))
                .ForMember(dest => dest.LastModifiedBy, opt => opt
                    .MapFrom(src => src.Person.ModifiedBy))
                .ForMember(dest => dest.Gender, opt => opt
                    .MapFrom(src => src.Person.Gender == GenderType.Male ? "M" : "F"))
                .ForMember(dest => dest.Phone, opt => opt
                    .MapFrom(src => src.Person.Phone))
                .ForMember(dest => dest.ExtPhone, opt => opt
                    .MapFrom(src => src.Person.ExtPhone))
                .ForMember(dest => dest.Notes, opt => opt
                    .MapFrom(src => src.Person.Notes))
                .ForMember(dest => dest.AddressStreetOne, opt => opt
                    .MapFrom(src => src.Person.Address.StreetOne))
                .ForMember(dest => dest.AddressStreetTwo, opt => opt
                    .MapFrom(src => src.Person.Address.StreetTwo))
                .ForMember(dest => dest.AddressPostcode, opt => opt
                    .MapFrom(src => src.Person.Address.PostCode))
                .ForMember(dest => dest.AddressCity, opt => opt
                    .MapFrom(src => src.Person.Address.City))
                .ForMember(dest => dest.AddressRegion, opt => opt
                    .MapFrom(src => src.Person.Address.Region))
                .ForMember(dest => dest.EmployeeRoleId, opt => opt
                    .MapFrom(src => src.Person.EmployeeRole.Id))
                .ForMember(dest => dest.EmployeeRoleName, opt => opt
                    .MapFrom(src => src.Person.EmployeeRole.Name))
                .ForMember(dest => dest.DepartmentId, opt => opt
                    .MapFrom(src => src.Person.Department.Id))
                .ForMember(dest => dest.DepartmentName, opt => opt
                    .MapFrom(src => src.Person.Department.Name))
                .MaxDepth(1)
                ;
        }
    }
}