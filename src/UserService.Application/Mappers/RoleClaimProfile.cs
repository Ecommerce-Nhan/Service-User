using AutoMapper;
using SharedLibrary.Response.Identity;
using UserService.Domains.Entities;

namespace UserService.Application.Mappers;

public class RoleClaimProfile : Profile
{
    public RoleClaimProfile()
    {
        CreateMap<RoleClaim, RoleClaimResponse>();
    }
}