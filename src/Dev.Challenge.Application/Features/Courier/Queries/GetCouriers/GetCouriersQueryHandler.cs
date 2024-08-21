using Dev.Challenge.Application.Service;
using Dev.Challenge.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dev.Challenge.Application.Features.Courier.Queries.GetCouriers
{

    public class GetCouriersQueryHandler : IRequestHandler<GetCouriersQuery, IEnumerable<CourierEntity>>
    {
        private readonly ICourierService _courierService;

        public GetCouriersQueryHandler(ICourierService courierService)
        {
            _courierService = courierService;
        }

        public async Task<IEnumerable<CourierEntity>> Handle(GetCouriersQuery request, CancellationToken cancellationToken)
        {
            return await _courierService.GetAllCouriersAsync();
        }
    }

}
