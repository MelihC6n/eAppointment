using AutoMapper;
using eAppointmentServer.Domain.Entities;
using eAppointmentServer.Domain.Repositories;
using GenericRepository;
using MediatR;
using TS.Result;

namespace eAppointmentServer.Application.Features.Patients.UpdatePatient;

internal sealed class UpdatePatientCommandHandler(
    IPatientRepository patientRepository,
    IUnitOfWork unitOfWork,
    IMapper mapper) : IRequestHandler<UpdatePatientCommand, Result<string>>
{
    public async Task<Result<string>> Handle(UpdatePatientCommand request, CancellationToken cancellationToken)
    {
        Patient? patient = await patientRepository.GetByExpressionWithTrackingAsync(x => x.Id == request.Id, cancellationToken);

        if (patient == null)
        {
            return Result<string>.Failure("Patient is not found!");
        }

        if (patient.IdentityNumber != request.IdentityNumber)
        {
            if(patientRepository.Any(x=>x.IdentityNumber == request.IdentityNumber))
            {
                return Result<string>.Failure("A patient with this identity number already exist!");
            }
        }

        mapper.Map(request, patient);

        patientRepository.Update(patient);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return "Patient information update is successful!";
    }
}
