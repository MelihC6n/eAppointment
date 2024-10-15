using eAppointmentServer.Domain.Entities;
using MediatR;
using TS.Result;

namespace eAppointmentServer.Application.Features.Users.GetRolesForUsers;
public sealed record GetRolesForUsersQuery() : IRequest<Result<List<AppRole>>>;
