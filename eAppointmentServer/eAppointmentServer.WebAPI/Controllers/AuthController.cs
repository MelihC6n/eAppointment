using eAppointmentServer.Application.Features.Auth.Login;
using eAppointmentServer.WebAPI.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace eAppointmentServer.WebAPI.Controllers;

public sealed class AuthController : ApiController
{
    public AuthController(IMediator mediator) : base(mediator)
    {
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginCommand loginCommand, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(loginCommand, cancellationToken);
        return StatusCode(response.StatusCode, response);
    }
}
