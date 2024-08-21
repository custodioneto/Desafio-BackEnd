using Dev.Challenge.Application.Service;
using Dev.Challenge.Domain.Entities;
using Dev.Challenge.Domain.Extensions;
using MediatR;

namespace Dev.Challenge.Application.Features.Courier.Commands.RegisterCourier
{
    public class RegisterCourierCommandHandler : IRequestHandler<RegisterCourierCommand, Unit>
    {
        private readonly ICourierService _courierService;

        public RegisterCourierCommandHandler(ICourierService courierService)
        {
            _courierService = courierService;
        }

        public async Task<Unit> Handle(RegisterCourierCommand request, CancellationToken cancellationToken)
        {
            var courier = new CourierEntity(request.Id, request.Name,request.Cnpj, 
                request.DateOfBirth,request.DriverLicenseNumber, request.DriverLicenseType.ParseToDriverLicenseType());

            await _courierService.RegisterCourierAsync(courier);
            return Unit.Value;
        }
    }
}
