using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dev.Challenge.Application.Features.Courier.Commands.DeleteCourier
{
    public class DeleteCourierCommand : IRequest<Unit>
    {
        public Guid Id { get; set; }
    }
}
