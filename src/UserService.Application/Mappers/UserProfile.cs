using AutoMapper;
using SharedLibrary.Dtos.Users;
using UserService.Domains.Entities;

namespace UserService.Application.Mappers;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserDto>();
    }
}
