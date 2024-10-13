using eAppointmentServer.Domain.Entities;

namespace eAppointmentServer.Application.Features.Appointments.GetAllAppointment;

public sealed record GetAllAppointmentByDoctorIdQueryRespond(
    Guid Id,
    DateTime StartDate,
    DateTime EndDate,
    string Text,
    Patient Patient);
