using Dev.Challenge.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dev.Challenge.Application.Features.Courier.Queries.GetCourierByCnpj
{
    public class GetCourierByCnpjQuery : IRequest<CourierEntity>
    {
        public string Cnpj { get; set; }
    }
}
