using eAppointmentServer.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using TS.Result;

namespace eAppointmentServer.Application.Features.Users.DeleteUser;

internal sealed class DeleteUserCommandHandler(
    UserManager<AppUser> userManager) : IRequestHandler<DeleteUserCommand, Result<string>>
{
    public async Task<Result<string>> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        AppUser? user = await userManager.FindByIdAsync(request.Id.ToString());
        if (user == null)
        {
            return Result<string>.Failure("User is not found!");
        }

        IdentityResult result = await userManager.DeleteAsync(user);
        if (!result.Succeeded)
        {
            return Result<string>.Failure(result.Errors.Select(s => s.Description).ToList());
        }

        return "User delete is successful";
    }
}
