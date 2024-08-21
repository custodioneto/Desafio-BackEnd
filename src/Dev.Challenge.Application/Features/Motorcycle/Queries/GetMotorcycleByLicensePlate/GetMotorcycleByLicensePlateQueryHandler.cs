using Dev.Challenge.Application.Features.Motorcycle.Commands.DeleteMotorcycle;
using Dev.Challenge.Application.Service;
using Dev.Challenge.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dev.Challenge.Application.Features.Motorcycle.Queries.GetMotorcycleByLicensePlate
{
    public class GetMotorcycleByLicensePlateQueryHandler : IRequestHandler<GetMotorcycleByLicensePlateQuery, MotorcycleEntity>
    {
        private readonly IMotorcycleService _motorcycleService;

        public GetMotorcycleByLicensePlateQueryHandler(IMotorcycleService motorcycleService)
        {
            _motorcycleService = motorcycleService;
        }

        public async Task<MotorcycleEntity> Handle(GetMotorcycleByLicensePlateQuery request, CancellationToken cancellationToken)
        {
            return await _motorcycleService.GetMotorcycleByLicensePlateAsync(request.LicensePlate);
        }
    }
}
