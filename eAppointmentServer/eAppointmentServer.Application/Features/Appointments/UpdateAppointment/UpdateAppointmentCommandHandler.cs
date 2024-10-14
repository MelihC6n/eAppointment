using eAppointmentServer.Domain.Entities;
using eAppointmentServer.Domain.Repositories;
using GenericRepository;
using MediatR;
using TS.Result;

namespace eAppointmentServer.Application.Features.Appointments.UpdateAppointment;

internal sealed class UpdateAppointmentCommandHandler(
    IAppointmentRepository appointmentRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<UpdateAppointmentCommand, Result<string>>
{
    public async Task<Result<string>> Handle(UpdateAppointmentCommand request, CancellationToken cancellationToken)
    {
        DateTime startDate = Convert.ToDateTime(request.StartDate);
        DateTime endDate = Convert.ToDateTime(request.EndDate);

        Appointment? appointment = await appointmentRepository.GetByExpressionWithTrackingAsync(a => a.Id == request.Id);

        if (appointment == null)
        {
            return Result<string>.Failure("Appointment is not found!");
        }

        bool isAppointmentDateNotAvailable =
            await appointmentRepository
            .AnyAsync(p =>
               p.Id != appointment.Id //Mevcut randevunun kendisini saymaması için.
            && p.DoctorId == appointment.DoctorId
            && ((p.StartDate < endDate && p.StartDate >= startDate)//Mevcut randevunun bitişi, diğer randevunun başlangıcıyla çakışıyor
            || (p.EndDate > startDate && p.EndDate <= endDate)//Mevcut randevunun başlangıcı, diğer randevunun bitişiyle çakışıyor
            || (p.StartDate >= startDate && p.StartDate <= startDate)//Mevcut randevu, diğer randevunun içerisinde kalıyor.
            || (p.StartDate <= startDate && p.EndDate >= endDate))//Mevcut randevu, diğer randevuyu kapsıyor.
            , cancellationToken);

        if (isAppointmentDateNotAvailable)
        {
            return Result<string>.Failure("Appointment date is not avaliable!");
        }

        appointment.StartDate = startDate;
        appointment.EndDate = endDate;

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return "Appointment successfully updated!";
    }
}
