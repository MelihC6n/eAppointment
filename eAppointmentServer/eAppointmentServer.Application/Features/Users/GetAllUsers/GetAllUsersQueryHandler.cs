using eAppointmentServer.Domain.Entities;
using eAppointmentServer.Domain.Repositories;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace eAppointmentServer.Application.Features.Users.GetAllUsers;

internal sealed class GetAllUsersQueryHandler(
    UserManager<AppUser> userManager,
    IUserRoleRepository userRoleRepository,
    RoleManager<AppRole> roleManager) : IRequestHandler<GetAllUsersQuery, Result<List<GetAllUsersQueryResponse>>>
{
    public async Task<Result<List<GetAllUsersQueryResponse>>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        List<AppUser> users = await userManager.Users.OrderBy(p => p.FirstName).ToListAsync(cancellationToken);

        List<GetAllUsersQueryResponse> response =
            users.Select(u => new GetAllUsersQueryResponse()
            {
                Id = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                FullName = u.FullName,
                Email = u.Email,
                UserName = u.UserName
            }).ToList();

        foreach (var item in response)
        {
            List<AppUserRole> userRoles = await userRoleRepository.Where(p => p.UserId == item.Id).ToListAsync(cancellationToken);

            List<AppRole> roles = new();

            foreach (var userRole in userRoles)
            {
                AppRole? role = await roleManager
                    .Roles
                    .FirstOrDefaultAsync(r => r.Id == userRole.RoleId, cancellationToken);

                if (role is not null)
                {
                    roles.Add(role);
                }
            }

            List<Guid> stringRoles = roles.Select(s => s.Id).ToList();
            List<string?> stringRoleNames = roles.Select(s => s.Name).ToList();
            item.RoleIds = stringRoles;
            item.RoleNames = stringRoleNames;
        }
        return response;
    }
}
