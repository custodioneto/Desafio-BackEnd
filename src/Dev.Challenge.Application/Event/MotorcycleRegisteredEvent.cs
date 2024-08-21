
namespace Dev.Challenge.Application.Event
{
    public class MotorcycleRegisteredEvent
    {
        public Guid MotorcycleId { get; set; }
        public int Year { get; set; }
        public string Model { get; set; }
        public string LicensePlate { get; set; }
    }

}
