using Dev.Challenge.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dev.Challenge.Application.Features.Motorcycle.Queries.GetMotorcycles
{
    public class GetMotorcyclesQuery : IRequest<IEnumerable<MotorcycleEntity>>
    {
    }
}
