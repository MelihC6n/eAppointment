using MediatR;
using TS.Result;

namespace eAppointmentServer.Application.Features.Users.DeleteUser;
public sealed record DeleteUserCommand(
    Guid Id) : IRequest<Result<string>>;