using AutoMapper;
using SharedLibrary.Response.Identity;
using UserService.Domains.Entities;

namespace UserService.Application.Mappers;

public class RoleProfile : Profile
{
    public RoleProfile()
    {
        CreateMap<Role, RoleResponse>();
    }
}