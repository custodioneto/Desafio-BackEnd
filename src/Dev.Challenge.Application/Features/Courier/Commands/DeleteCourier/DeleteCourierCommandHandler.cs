using Dev.Challenge.Application.Service;
using MediatR;

namespace Dev.Challenge.Application.Features.Courier.Commands.DeleteCourier
{
    public class DeleteCourierCommandHandler : IRequestHandler<DeleteCourierCommand, Unit>
    {
        private readonly ICourierService _courierService;

        public DeleteCourierCommandHandler(ICourierService courierService)
        {
            _courierService = courierService;
        }

        public async Task<Unit> Handle(DeleteCourierCommand request, CancellationToken cancellationToken)
        {
            await _courierService.DeleteCourierAsync(request.Id);
            return Unit.Value;
        }
    }
}
