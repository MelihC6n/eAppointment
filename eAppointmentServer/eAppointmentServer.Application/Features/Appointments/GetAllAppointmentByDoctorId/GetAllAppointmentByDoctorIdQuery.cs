using MediatR;
using TS.Result;

namespace eAppointmentServer.Application.Features.Appointments.GetAllAppointment;
public sealed record GetAllAppointmentByDoctorIdQuery(
    Guid DoctorId) : IRequest<Result<List<GetAllAppointmentByDoctorIdQueryRespond>>>;
