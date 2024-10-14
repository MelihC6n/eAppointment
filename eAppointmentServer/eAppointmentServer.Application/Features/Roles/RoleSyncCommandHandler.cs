using eAppointmentServer.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace eAppointmentServer.Application.Features.Roles;

internal sealed class RoleSyncCommandHandler(
    RoleManager<AppRole> roleManager) : IRequestHandler<RoleSyncCommand, Result<string>>
{
    public async Task<Result<string>> Handle(RoleSyncCommand request, CancellationToken cancellationToken)
    {
        List<AppRole> currentRoles = await roleManager.Roles.ToListAsync(cancellationToken);

        List<AppRole> staticRoles = Constants.GetRoles();

        foreach (var role in currentRoles)
        {
            if (!staticRoles.Any(s => s.Name == role.Name))
            {
                await roleManager.DeleteAsync(role);
            }
        }

        foreach (var role in staticRoles)
        {
            if (!currentRoles.Any(s => s.Name == role.Name))
            {
                await roleManager.CreateAsync(role);
            }
        }

        return "Sync is successfull!";
    }
}
