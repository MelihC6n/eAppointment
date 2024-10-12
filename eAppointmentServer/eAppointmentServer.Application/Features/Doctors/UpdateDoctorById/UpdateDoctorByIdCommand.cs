using MediatR;
using TS.Result;

namespace eAppointmentServer.Application.Features.Doctors.UpdateDoctorById;
public sealed record UpdateDoctorByIdCommand(
    Guid Id,
    string FirstName,
    string LastName,
    int DepartmentValue) : IRequest<Result<string>>;