using eAppointmentServer.Domain.Entities;
using eAppointmentServer.Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace eAppointmentServer.Application.Features.Appointments.GetAllDoctorByDepartment;

internal sealed class GetAllDoctorByDepartmentQueryHandler(
    IDoctorRepository doctorRepository) : IRequestHandler<GetAllDoctorByDepartmentQuery, Result<List<Doctor>>>
{
    public async Task<Result<List<Doctor>>> Handle(GetAllDoctorByDepartmentQuery request, CancellationToken cancellationToken)
    {
        List<Doctor> doctors = await doctorRepository
            .Where(d => d.Department == request.DepartmentValue)
            .OrderBy(d => d.FirstName)
            .ToListAsync(cancellationToken);

        return doctors;
    }
}
