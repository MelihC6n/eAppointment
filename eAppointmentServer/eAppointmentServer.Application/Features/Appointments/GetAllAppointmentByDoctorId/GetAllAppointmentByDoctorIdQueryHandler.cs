using eAppointmentServer.Domain.Entities;
using eAppointmentServer.Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace eAppointmentServer.Application.Features.Appointments.GetAllAppointment;

internal sealed class GetAllAppointmentByDoctorIdQueryHandler(
    IAppointmentRepository appointmentRepository) : IRequestHandler<GetAllAppointmentByDoctorIdQuery, Result<List<GetAllAppointmentByDoctorIdQueryRespond>>>
{
    public async Task<Result<List<GetAllAppointmentByDoctorIdQueryRespond>>> Handle(GetAllAppointmentByDoctorIdQuery request, CancellationToken cancellationToken)
    {
        List<Appointment> appointments = await appointmentRepository
            .Where(a => a.DoctorId == request.DoctorId)
            .Include(a => a.Patient)
            .ToListAsync(cancellationToken);

        List<GetAllAppointmentByDoctorIdQueryRespond> result =
            appointments.Select(s =>
            new GetAllAppointmentByDoctorIdQueryRespond(
                s.Id,
                s.StartDate,
                s.EndDate,
                s.Patient!.FullName,
                s.Patient))
            .ToList();

        return result;
    }
}
