using AutoMapper;
using eAppointmentServer.Domain.Entities;
using eAppointmentServer.Domain.Repositories;
using GenericRepository;
using MediatR;
using TS.Result;

namespace eAppointmentServer.Application.Features.Patients.CreatePatient;

internal sealed class CreatePatientCommandHandler(
    IPatientRepository patientRepository,
    IUnitOfWork unitOfWork,
    IMapper mapper) : IRequestHandler<CreatePatientCommand, Result<string>>
{
    public async Task<Result<string>> Handle(CreatePatientCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (patientRepository.Any(x => x.IdentityNumber == request.IdentityNumber))
            {
                return Result<string>.Failure("Patient already recorded!");
            }

            Patient patient = mapper.Map<Patient>(request);

            await patientRepository.AddAsync(patient);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return "Patient successfully added";
        }
        catch (Exception ex)
        {
            return Result<string>.Failure("Something went wrong while adding patient! \n Details : " + ex.Message);
        }
    }
}
