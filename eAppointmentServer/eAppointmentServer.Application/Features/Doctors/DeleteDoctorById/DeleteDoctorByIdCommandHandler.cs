using eAppointmentServer.Domain.Entities;
using eAppointmentServer.Domain.Repositories;
using GenericRepository;
using MediatR;
using TS.Result;

namespace eAppointmentServer.Application.Features.Doctors.DeleteDoctorById;

internal sealed class DeleteDoctorByIdCommandHandler(
    IDoctorRepository doctorRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<DeleteDoctorByIdCommand, Result<string>>
{
    public async Task<Result<string>> Handle(DeleteDoctorByIdCommand request, CancellationToken cancellationToken)
    {
        Doctor? doctor = await doctorRepository.GetByExpressionAsync(x => x.Id == request.Id);

        if (doctor is null)
        {
            return Result<string>.Failure("Doctor is not found");
        }

        doctorRepository.Delete(doctor);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return "Doctor delete is successful";
    }
}
