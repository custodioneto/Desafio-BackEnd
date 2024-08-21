using Dev.Challenge.Application.Features.Motorcycle.Commands.DeleteMotorcycle;
using Dev.Challenge.Application.Features.Motorcycle.Commands.RegisterMotorcycle;
using Dev.Challenge.Application.Features.Motorcycle.Commands.UpdateMotorcycle;
using Dev.Challenge.Application.Features.Motorcycle.Queries.GetMotorcycleByLicensePlate;
using Dev.Challenge.Application.Features.Motorcycle.Queries.GetMotorcycles;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace Dev.Challenge.Api.Controllers
{
    [Authorize(Roles = "ADMIN")]
    [Route("api/[controller]")]
    [ApiController]
    public class MotorcycleController : BaseController
    {
        public MotorcycleController(IMediator mediator) :base(mediator) { }

        [HttpPost]
        public async Task<IActionResult> RegisterMotorcycle([FromBody] RegisterMotorcycleCommand command)
        {
            return await ExecuteAsync(() => Mediator.Send(command));
        }

        [HttpGet]
        public async Task<IActionResult> GetAllMotorcycles()
        {
            return await ExecuteAsync(() => Mediator.Send(new GetMotorcyclesQuery()));
        }

        [HttpGet("{licensePlate}")]
        public async Task<IActionResult> GetMotorcycleByLicensePlate(string licensePlate)
        {
            return await ExecuteAsync(() => Mediator.Send(new GetMotorcycleByLicensePlateQuery { LicensePlate = licensePlate }));
        }

        [HttpPut]
        public async Task<IActionResult> UpdateMotorcycle([FromBody] UpdateMotorcycleCommand command)
        {
            return await ExecuteAsync(() => Mediator.Send(command));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMotorcycle(Guid id)
        {
            return await ExecuteAsync(() => Mediator.Send(new DeleteMotorcycleCommand { Id = id }));
        }
    }

}
