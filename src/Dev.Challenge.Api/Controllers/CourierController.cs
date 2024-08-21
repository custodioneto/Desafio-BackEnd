using Dev.Challenge.Application.Features.Courier.Commands.DeleteCourier;
using Dev.Challenge.Application.Features.Courier.Commands.RegisterCourier;
using Dev.Challenge.Application.Features.Courier.Commands.UpdateCourier;
using Dev.Challenge.Application.Features.Courier.Commands.UpdateDriverLicenseImage;
using Dev.Challenge.Application.Features.Courier.Queries.GetCourierByCnpj;
using Dev.Challenge.Application.Features.Courier.Queries.GetCourierByDriverLicenseNumber;
using Dev.Challenge.Application.Features.Courier.Queries.GetCouriers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace Dev.Challenge.Api.Controllers
{
    [Authorize(Roles = "ENTREGADOR")]
    [Route("api/[controller]")]
    [ApiController]
    public class CourierController : BaseController
    {
        public CourierController(IMediator mediator) : base(mediator) { }

        [HttpPost]
        public async Task<IActionResult> RegisterCourier([FromBody] RegisterCourierCommand command)
        {
            return await ExecuteAsync(() => Mediator.Send(command));
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCouriers()
        {
            return await ExecuteAsync(() => Mediator.Send(new GetCouriersQuery()));
        }

        [HttpGet("{cnpj}")]
        public async Task<IActionResult> GetCourierByCnpj(string cnpj)
        {
            return await ExecuteAsync(() => Mediator.Send(new GetCourierByCnpjQuery { Cnpj = cnpj }));
        }

        [HttpGet("driverLicense/{driverLicenseNumber}")]
        public async Task<IActionResult> GetCourierByDriverLicenseNumber(string driverLicenseNumber)
        {
            return await ExecuteAsync(() => Mediator.Send(new GetCourierByDriverLicenseNumberQuery { DriverLicenseNumber = driverLicenseNumber }));
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCourier([FromBody] UpdateCourierCommand command)
        {
            return await ExecuteAsync(() => Mediator.Send(command));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourier(Guid id)
        {
            return await ExecuteAsync(() => Mediator.Send(new DeleteCourierCommand { Id = id }));
        }

        [HttpPost("{id}/uploadLicenseImage")]
        public async Task<IActionResult> UploadDriverLicenseImage(Guid id, [FromForm] IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            return await ExecuteAsync(() => Mediator.Send(new UpdateDriverLicenseImageCommand { Id = id, File = file }));
        }
    }

}
