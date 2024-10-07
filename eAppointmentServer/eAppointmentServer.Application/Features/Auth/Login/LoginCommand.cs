using MediatR;
using TS.Result;

namespace eAppointmentServer.Application.Features.Auth.Login;
public sealed record LoginCommand(
    string UserNameOrEmail,
    string password) : IRequest<Result<LoginCommandResponse>>;
