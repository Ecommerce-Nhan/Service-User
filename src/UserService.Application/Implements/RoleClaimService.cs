using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SharedLibrary.Response.Identity;
using SharedLibrary.Wrappers;
using UserService.Application.Interfaces;
using UserService.Infrastructure;

namespace UserService.Application.Implements;

public class RoleClaimService : IRoleClaimService
{
    private readonly IMapper _mapper;
    private readonly UserDbContext _db;

    public RoleClaimService(
        IMapper mapper,
        UserDbContext db)
    {
        _mapper = mapper;
        _db = db;
    }
    public async Task<Response<List<RoleClaimResponse>>> GetAllAsync()
    {
        var roleClaims = await _db.RoleClaims.ToListAsync();
        var roleClaimsResponse = _mapper.Map<List<RoleClaimResponse>>(roleClaims);
        return await Response<List<RoleClaimResponse>>.SuccessAsync(roleClaimsResponse);
    }

    public async Task<int> GetCountAsync()
    {
        var count = await _db.RoleClaims.CountAsync();
        return count;
    }

    public async Task<Response<RoleClaimResponse>> GetByIdAsync(int id)
    {
        var roleClaim = await _db.RoleClaims
            .SingleOrDefaultAsync(x => x.Id == id);
        var roleClaimResponse = _mapper.Map<RoleClaimResponse>(roleClaim);
        return await Response<RoleClaimResponse>.SuccessAsync(roleClaimResponse);
    }

    public async Task<Response<List<RoleClaimResponse>>> GetAllByRoleIdAsync(string roleId)
    {
        var roleClaims = await _db.RoleClaims
            .Include(x => x.Role)
            .Where(x => x.RoleId == roleId)
            .ToListAsync();
        var roleClaimsResponse = _mapper.Map<List<RoleClaimResponse>>(roleClaims);
        return await Response<List<RoleClaimResponse>>.SuccessAsync(roleClaimsResponse);
    }
}