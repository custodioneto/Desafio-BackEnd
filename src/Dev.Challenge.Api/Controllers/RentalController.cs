using Dev.Challenge.Application.Features.Rental.Commands.DeleteRental;
using Dev.Challenge.Application.Features.Rental.Commands.RentMotorcycle;
using Dev.Challenge.Application.Features.Rental.Commands.UpdateRental;
using Dev.Challenge.Application.Features.Rental.Queries.CalculateRentalFee;
using Dev.Challenge.Application.Features.Rental.Queries.GetRentalById;
using Dev.Challenge.Application.Features.Rental.Queries.GetRentals;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace Dev.Challenge.Api.Controllers
{
    [Authorize(Roles = "ENTREGADOR")]
    [Route("api/[controller]")]
    [ApiController]
    public class RentalController : BaseController
    {
        public RentalController(IMediator mediator) : base(mediator) { }

        [HttpPost]
        public async Task<IActionResult> RentMotorcycle([FromBody] RentMotorcycleCommand command)
        {
            return await ExecuteAsync(() => Mediator.Send(command));
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRentals()
        {
            return await ExecuteAsync(() => Mediator.Send(new GetRentalsQuery()));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRentalById(Guid id)
        {
            return await ExecuteAsync(() => Mediator.Send(new GetRentalByIdQuery { Id = id }));
        }

        [HttpPut]
        public async Task<IActionResult> UpdateRental([FromBody] UpdateRentalCommand command)
        {
            return await ExecuteAsync(() => Mediator.Send(command));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRental(Guid id)
        {
            return await ExecuteAsync(() => Mediator.Send(new DeleteRentalCommand { Id = id }));
        }

        [HttpPost("{id}/calculateFee")]
        public async Task<IActionResult> CalculateRentalFee(Guid id, [FromBody] DateTime returnDate)
        {
            return await ExecuteAsync(() => Mediator.Send(new CalculateRentalFeeQuery { RentalId = id, ReturnDate = returnDate }));
        }
    }

}
