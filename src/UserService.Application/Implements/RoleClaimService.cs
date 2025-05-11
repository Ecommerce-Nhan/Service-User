using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SharedLibrary.Requests.Identity;
using SharedLibrary.Response.Identity;
using SharedLibrary.Wrappers;
using UserService.Application.Interfaces;
using UserService.Domains.Entities;
using UserService.Infrastructure;

namespace UserService.Application.Implements;

public class RoleClaimService : IRoleClaimService
{
    private readonly IMapper _mapper;
    private readonly UserDbContext _db;

    public RoleClaimService(IMapper mapper, UserDbContext db)
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

    public async Task<Response<string>> CreateAsync(RoleClaimRequest request)
    {
        var existingRoleClaim =
                    await _db.RoleClaims
                        .SingleOrDefaultAsync(x =>
                            x.RoleId == request.RoleId && x.ClaimType == request.Type && x.ClaimValue == request.Value);
        if (existingRoleClaim != null)
        {
            return await Response<string>.FailAsync("Similar Role Claim already exists.");
        }
        var roleClaim = _mapper.Map<RoleClaim>(request);
        await _db.RoleClaims.AddAsync(roleClaim);
        await _db.SaveChangesAsync();
        return await Response<string>.SuccessAsync(string.Format("Role Claim {0} created.", request.Value));
    }

    public async Task<Response<string>> UpdateAsync(int id, RoleClaimRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.RoleId))
        {
            return await Response<string>.FailAsync("Role is required.");
        }

        var existingRoleClaim =
                    await _db.RoleClaims
                        .Include(x => x.Role)
                        .SingleOrDefaultAsync(x => x.Id == request.Id);
        if (existingRoleClaim == null)
        {
            return await Response<string>.SuccessAsync("Role Claim does not exist.");
        }
        else
        {
            existingRoleClaim.ClaimType = request.Type;
            existingRoleClaim.ClaimValue = request.Value;
            existingRoleClaim.Group = request.Group;
            existingRoleClaim.Description = request.Description;
            existingRoleClaim.RoleId = request.RoleId;
            _db.RoleClaims.Update(existingRoleClaim);
            await _db.SaveChangesAsync();
            return await Response<string>.SuccessAsync(string.Format("Role Claim {0} for Role {1} updated.", request.Value, existingRoleClaim.Role.Name));
        }
    }

    public async Task<Response<string>> DeleteAsync(int id)
    {
        var existingRoleClaim = await _db.RoleClaims
                .Include(x => x.Role)
                .FirstOrDefaultAsync(x => x.Id == id);
        if (existingRoleClaim != null)
        {
            _db.RoleClaims.Remove(existingRoleClaim);
            await _db.SaveChangesAsync();
            return await Response<string>.SuccessAsync(string.Format("Role Claim {0} for {1} Role deleted.", existingRoleClaim.ClaimValue, existingRoleClaim.Role.Name));
        }
        else
        {
            return await Response<string>.FailAsync("Role Claim does not exist.");
        }
    }
}