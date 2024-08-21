using Dev.Challenge.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dev.Challenge.Application.Features.Motorcycle.Queries.GetMotorcycleByLicensePlate
{
    public class GetMotorcycleByLicensePlateQuery : IRequest<MotorcycleEntity>
    {
        public string LicensePlate { get; set; }
    }
}
