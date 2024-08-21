using Dev.Challenge.Application.Service;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dev.Challenge.Application.Features.Auth.Commands.Login
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginVm?>
    {
        private readonly IAuthService _authService;

        public LoginCommandHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<LoginVm?> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var token = _authService.Authenticate(request.Username, request.Password);

            if (token == null) return null;

            return new LoginVm { Token =  token };  
        }
    }
}
