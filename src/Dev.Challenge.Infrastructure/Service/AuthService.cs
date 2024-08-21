using Dev.Challenge.Application.Models;
using Dev.Challenge.Application.Service;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Dev.Challenge.Infrastructure.Service
{
    public class AuthService : IAuthService
    {
        private readonly IMemoryCache _memoryCache;
        private readonly IConfiguration _configuration;

        public AuthService(IMemoryCache memoryCache, IConfiguration configuration)
        {
            _memoryCache = memoryCache;
            _configuration = configuration;
        }

        public string Authenticate(string username, string password)
        {
            if (_memoryCache.TryGetValue("Users", out List<User> users))
            {
                var user = users.SingleOrDefault(x => x.Username == username && x.Password == password);

                if (user == null)
                    return null;

                // Usar a chave diretamente sem conversão adicional
                var keyString = _configuration["Jwt:Key"];
                var keyBytes = Encoding.UTF8.GetBytes(keyString);
                var signingKey = new SymmetricSecurityKey(keyBytes);

                // Gerar JWT
                var tokenHandler = new JwtSecurityTokenHandler();
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[]
                    {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role)
            }),
                    Expires = DateTime.UtcNow.AddHours(1),
                    SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                return tokenHandler.WriteToken(token);
            }

            return null;
        }

    }
}
