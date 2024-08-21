using Dev.Challenge.Application.Service;
using Dev.Challenge.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dev.Challenge.Application.Features.Courier.Queries.GetCourierByCnpj
{
    public class GetCourierByCnpjQueryHandler : IRequestHandler<GetCourierByCnpjQuery, CourierEntity>
    {
        private readonly ICourierService _courierService;

        public GetCourierByCnpjQueryHandler(ICourierService courierService)
        {
            _courierService = courierService;
        }

        public async Task<CourierEntity> Handle(GetCourierByCnpjQuery request, CancellationToken cancellationToken)
        {
            return await _courierService.GetCourierByCnpjAsync(request.Cnpj);
        }
    }
}
