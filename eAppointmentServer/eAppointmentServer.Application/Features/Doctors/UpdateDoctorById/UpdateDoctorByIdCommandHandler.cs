using AutoMapper;
using eAppointmentServer.Domain.Entities;
using eAppointmentServer.Domain.Repositories;
using GenericRepository;
using MediatR;
using TS.Result;

namespace eAppointmentServer.Application.Features.Doctors.UpdateDoctorById;

internal sealed class UpdateDoctorByIdCommandHandler(
    IDoctorRepository doctorRepository,
    IUnitOfWork unitOfWork,
    IMapper mapper) : IRequestHandler<UpdateDoctorByIdCommand, Result<string>>
{
    public async Task<Result<string>> Handle(UpdateDoctorByIdCommand request, CancellationToken cancellationToken)
    {
        Doctor doctor = await doctorRepository.GetByExpressionAsync(x => x.Id == request.Id);
        if (doctor is null)
        {
            return Result<string>.Failure("Doctor is not found");
        }

        mapper.Map(request, doctor);

        doctorRepository.Update(doctor);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return "Doctor info is succesfly updated!";
    }
}
