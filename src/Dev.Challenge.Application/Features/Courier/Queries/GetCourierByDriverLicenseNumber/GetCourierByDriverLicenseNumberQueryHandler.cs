using Dev.Challenge.Application.Service;
using Dev.Challenge.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dev.Challenge.Application.Features.Courier.Queries.GetCourierByDriverLicenseNumber
{
    public class GetCourierByDriverLicenseNumberQueryHandler : IRequestHandler<GetCourierByDriverLicenseNumberQuery, CourierEntity>
    {
        private readonly ICourierService _courierService;

        public GetCourierByDriverLicenseNumberQueryHandler(ICourierService courierService)
        {
            _courierService = courierService;
        }

        public async Task<CourierEntity> Handle(GetCourierByDriverLicenseNumberQuery request, CancellationToken cancellationToken)
        {
            return await _courierService.GetCourierByDriverLicenseNumberAsync(request.DriverLicenseNumber);
        }
    }

}
