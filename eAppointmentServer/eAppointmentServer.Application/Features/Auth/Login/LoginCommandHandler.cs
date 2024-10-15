using eAppointmentServer.Application.Services;
using eAppointmentServer.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace eAppointmentServer.Application.Features.Auth.Login;

internal sealed class LoginCommandHandler(
    UserManager<AppUser> userManager, IJwtProvider jwtProvider) : IRequestHandler<LoginCommand, Result<LoginCommandResponse>>
{
    public async Task<Result<LoginCommandResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        AppUser? user = await userManager.Users.FirstOrDefaultAsync(
            p => p.UserName == request.UserNameOrEmail ||
            p.Email == request.UserNameOrEmail);

        if (user is null)
        {
            return Result<LoginCommandResponse>.Failure("User is not found");
        }

        bool isPasswordCorrect = await userManager.CheckPasswordAsync(user, request.password);
        if (!isPasswordCorrect)
        {
            return Result<LoginCommandResponse>.Failure("Wrong password!");
        }

        string token = await jwtProvider.CreateTokenAsync(user);
        LoginCommandResponse response = new(token);

        return Result<LoginCommandResponse>.Succeed(response);
    }
}
