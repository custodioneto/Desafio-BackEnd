using Dev.Challenge.Application.Service;
using MediatR;

namespace Dev.Challenge.Application.Features.Motorcycle.Commands.DeleteMotorcycle
{
    public class DeleteMotorcycleCommandHandler : IRequestHandler<DeleteMotorcycleCommand,Unit>
    {
        private readonly IMotorcycleService _motorcycleService;

        public DeleteMotorcycleCommandHandler(IMotorcycleService motorcycleService)
        {
            _motorcycleService = motorcycleService;
        }

        public async Task<Unit> Handle(DeleteMotorcycleCommand request, CancellationToken cancellationToken)
        {
            await _motorcycleService.DeleteMotorcycleAsync(request.Id);
            return Unit.Value;
        }
    }
}
