using eAppointmentServer.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace eAppointmentServer.Application.Features.Users.GetRolesForUsers;

internal sealed class GetRolesForUsersQueryHandler(
    RoleManager<AppRole> roleManager) : IRequestHandler<GetRolesForUsersQuery, Result<List<AppRole>>>
{
    public async Task<Result<List<AppRole>>> Handle(GetRolesForUsersQuery request, CancellationToken cancellationToken)
    {
        List<AppRole> roles = await roleManager.Roles.OrderBy(r => r.Name).ToListAsync(cancellationToken);
        return roles;
    }
}
