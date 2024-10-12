using eAppointmentServer.Domain.Entities;
using eAppointmentServer.Domain.Repositories;
using GenericRepository;
using MediatR;
using TS.Result;

namespace eAppointmentServer.Application.Features.Patients.DeletePatient;
public sealed record DeletePatientCommand(
    Guid Id) : IRequest<Result<string>>;

internal sealed class DeletePatientCommandHandler(
    IPatientRepository patientRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<DeletePatientCommand, Result<string>>
{
    public async Task<Result<string>> Handle(DeletePatientCommand request, CancellationToken cancellationToken)
    {
        Patient? patient = await patientRepository.GetByExpressionWithTrackingAsync(x => x.Id == request.Id, cancellationToken);
        if (patient == null)
        {
            return Result<string>.Failure("Patient not found!");
        }

        patientRepository.Delete(patient);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return "Patient deleted successfully!";
    }
}
