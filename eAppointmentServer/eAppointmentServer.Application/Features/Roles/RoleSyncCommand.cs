using MediatR;
using TS.Result;

namespace eAppointmentServer.Application.Features.Roles;
public sealed record RoleSyncCommand() : IRequest<Result<string>>;
