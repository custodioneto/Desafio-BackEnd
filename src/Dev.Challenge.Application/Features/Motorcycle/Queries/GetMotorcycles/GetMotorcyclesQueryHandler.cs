using Dev.Challenge.Application.Service;
using Dev.Challenge.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dev.Challenge.Application.Features.Motorcycle.Queries.GetMotorcycles
{
    public class GetMotorcyclesQueryHandler : IRequestHandler<GetMotorcyclesQuery, IEnumerable<MotorcycleEntity>>
    {
        private readonly IMotorcycleService _motorcycleService;

        public GetMotorcyclesQueryHandler(IMotorcycleService motorcycleService)
        {
            _motorcycleService = motorcycleService;
        }

        public async Task<IEnumerable<MotorcycleEntity>> Handle(GetMotorcyclesQuery request, CancellationToken cancellationToken)
        {
            return await _motorcycleService.GetAllMotorcyclesAsync();
        }
    }
}
