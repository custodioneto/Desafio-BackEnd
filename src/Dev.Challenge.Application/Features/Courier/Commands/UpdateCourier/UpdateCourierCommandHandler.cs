using Dev.Challenge.Application.Service;
using Dev.Challenge.Domain.Entities;
using Dev.Challenge.Domain.Extensions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dev.Challenge.Application.Features.Courier.Commands.UpdateCourier
{

    public class UpdateCourierCommandHandler : IRequestHandler<UpdateCourierCommand, Unit>
    {
        private readonly ICourierService _courierService;

        public UpdateCourierCommandHandler(ICourierService courierService)
        {
            _courierService = courierService;
        }

        public async Task<Unit> Handle(UpdateCourierCommand request, CancellationToken cancellationToken)
        {
            var courier = await _courierService.GetCourierByDriverLicenseNumberAsync(request.DriverLicenseNumber);
            if (courier != null)
            {
                var updatedCourier = new CourierEntity(request.Id, request.Name, request.Cnpj, request.DateOfBirth,
                    request.DriverLicenseNumber, request.DriverLicenseType.ParseToDriverLicenseType());

                updatedCourier.UpdateDriverLicenseImage(courier.DriverLicenseImageUrl);

                await _courierService.UpdateCourierAsync(updatedCourier);
            }

            return Unit.Value;
        }
    }

}
