using Dev.Challenge.Application.Service;
using MediatR;

namespace Dev.Challenge.Application.Features.Motorcycle.Commands.UpdateMotorcycle
{
    public class UpdateMotorcycleCommandHandler : IRequestHandler<UpdateMotorcycleCommand, Unit>
    {
        private readonly IMotorcycleService _motorcycleService;

        public UpdateMotorcycleCommandHandler(IMotorcycleService motorcycleService)
        {
            _motorcycleService = motorcycleService;
        }

        public async Task<Unit> Handle(UpdateMotorcycleCommand request, CancellationToken cancellationToken)
        {
            var motorcycle = await _motorcycleService.GetMotorcycleByIdAsync(request.Id);
            if (motorcycle != null)
            {
                await _motorcycleService.UpdateMotorcycleLicensePlateAsync(request.Id, request.LicensePlate);
            }

            return Unit.Value;
        }
    }
}
