using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dev.Challenge.Application.Features.Auth.Commands.Login
{
    public class LoginCommand : IRequest<LoginVm>
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
