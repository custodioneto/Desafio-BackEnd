using Dev.Challenge.Application.Service;
using Dev.Challenge.Domain.Entities;
using MediatR;

namespace Dev.Challenge.Application.Features.Motorcycle.Commands.RegisterMotorcycle
{
    public class RegisterMotorcycleCommandHandler : IRequestHandler<RegisterMotorcycleCommand, Unit>
    {
        private readonly IMotorcycleService _motorcycleService;

        public RegisterMotorcycleCommandHandler(IMotorcycleService motorcycleService)
        {
            _motorcycleService = motorcycleService;
        }

        public async Task<Unit> Handle(RegisterMotorcycleCommand request, CancellationToken cancellationToken)
        {
            MotorcycleEntity motorcycle = new MotorcycleEntity(request.Id, request.Year,request.Model,request.LicensePlate);

            await _motorcycleService.RegisterMotorcycleAsync(motorcycle);
            return Unit.Value;
        }
    }
}
