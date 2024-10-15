using AutoMapper;
using eAppointmentServer.Domain.Entities;
using eAppointmentServer.Domain.Repositories;
using GenericRepository;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace eAppointmentServer.Application.Features.Users.UpdateUser;

internal sealed class UpdateUserCommandHandler(
    UserManager<AppUser> userManager,
    IMapper mapper,
    IUserRoleRepository userRoleRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<UpdateUserCommand, Result<string>>
{
    public async Task<Result<string>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        AppUser? user = await userManager.FindByIdAsync(request.Id.ToString());
        if (user == null)
        {
            return Result<string>.Failure("User is not found!");
        }

        if (user.Email != request.Email)
        {
            if (await userManager.Users.AnyAsync(u => u.Email == request.Email))
            {
                return Result<string>.Failure("Email already exist!");
            }
        }

        if (user.UserName != request.UserName)
        {
            if (await userManager.Users.AnyAsync(u => u.UserName == request.UserName))
            {
                return Result<string>.Failure("Username already exist!");
            }
        }

        mapper.Map(request, user);

        IdentityResult result = await userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            return Result<string>.Failure(result.Errors.Select(s => s.Description).ToList());
        }

        if (request.RoleIds.Any())
        {
            List<AppUserRole> userRoles = await userRoleRepository.Where(u => u.UserId == user.Id).ToListAsync(cancellationToken);

            userRoleRepository.DeleteRange(userRoles);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            userRoles = new();

            foreach (var roleId in request.RoleIds)
            {
                AppUserRole appUserRole = new()
                {
                    RoleId = roleId,
                    UserId = user.Id
                };
                userRoles.Add(appUserRole);
            }

            await userRoleRepository.AddRangeAsync(userRoles, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }

        return "User update is successful!";
    }
}
