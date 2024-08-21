using MediatR;

namespace Dev.Challenge.Application.Features.Motorcycle.Commands.RegisterMotorcycle
{
    public class RegisterMotorcycleCommand : IRequest<Unit>
    {
        public Guid Id { get; set; }
        public int Year { get; set; }
        public string Model { get; set; }
        public string LicensePlate { get; set; }
    }
}
