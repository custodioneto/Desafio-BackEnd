using Dev.Challenge.Application.Service;
using MediatR;

namespace Dev.Challenge.Application.Features.Courier.Commands.UpdateDriverLicenseImage
{

    public class UpdateDriverLicenseImageCommandHandler : IRequestHandler<UpdateDriverLicenseImageCommand, Unit>
    {
        private readonly ICourierService _courierService;

        public UpdateDriverLicenseImageCommandHandler(ICourierService courierService)
        {
            _courierService = courierService;
        }

        public async Task<Unit> Handle(UpdateDriverLicenseImageCommand request, CancellationToken cancellationToken)
        {
            using (var memoryStream = new MemoryStream())
            {
                await request.File.CopyToAsync(memoryStream);

                // Redefinir a posição do MemoryStream para o início
                memoryStream.Position = 0;

                await _courierService.UpdateDriverLicenseImageAsync(request.Id, memoryStream, request.File.FileName);
            }

            return Unit.Value;
        }
    }

}
