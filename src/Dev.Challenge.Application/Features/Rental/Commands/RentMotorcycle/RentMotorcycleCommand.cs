using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dev.Challenge.Application.Features.Rental.Commands.RentMotorcycle
{

    public class RentMotorcycleCommand : IRequest<Unit>
    {
        public Guid MotorcycleId { get; set; }
        public Guid CourierId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime ExpectedEndDate { get; set; }
    }

}
