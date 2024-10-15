using eAppointmentServer.Application.Services;
using eAppointmentServer.Domain.Entities;
using eAppointmentServer.Domain.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace eAppointmentServer.Infrastructure.Services;
internal sealed class JwtProvider(
    IConfiguration configuration,
    IUserRoleRepository userRoleRepository,
    RoleManager<AppRole> roleManager) : IJwtProvider
{
    public async Task<string> CreateTokenAsync(AppUser user)
    {
        List<AppUserRole> userRoles = await userRoleRepository.Where(u => u.UserId == user.Id).ToListAsync();

        List<AppRole> roles = new();

        foreach (var item in userRoles)
        {
            AppRole? role = await roleManager.Roles.Where(r => r.Id == item.RoleId).FirstOrDefaultAsync();
            if (role != null)
            {
                roles.Add(role);
            }
        }

        List<string?> stringRoles = roles.Select(s => s.Name).ToList();

        List<Claim> claims = new()
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.FullName),
            new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
            new Claim("UserName", user.UserName ?? string.Empty),
            new Claim(ClaimTypes.Role,JsonSerializer.Serialize(stringRoles))
        };

        DateTime expireDate = DateTime.Now.AddDays(1);

        SymmetricSecurityKey securityKey =
            new(Encoding.UTF8.GetBytes(configuration.GetSection("Jwt:SecretKey").Value ?? ""));

        SigningCredentials signingCredentials = new(securityKey, SecurityAlgorithms.HmacSha512);

        JwtSecurityToken securityToken = new(
            issuer: configuration.GetSection("Jwt:Issuer").Value,
            audience: configuration.GetSection("Jwt:Audience").Value,
            claims: claims,
            notBefore: DateTime.Now,
            expires: expireDate,
            signingCredentials: signingCredentials);

        JwtSecurityTokenHandler handler = new();

        string token = handler.WriteToken(securityToken);

        return token;
    }
}
