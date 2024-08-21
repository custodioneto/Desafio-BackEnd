using MediatR;
using Microsoft.AspNetCore.Http;


namespace Dev.Challenge.Application.Features.Courier.Commands.UpdateDriverLicenseImage
{
    public class UpdateDriverLicenseImageCommand : IRequest<Unit>
    {
        public Guid Id { get; set; }
        public IFormFile File { get; set; }
    }
}
