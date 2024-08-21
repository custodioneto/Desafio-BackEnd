using Dev.Challenge.Domain.Contracts;
using Dev.Challenge.Domain.Enums;

namespace Dev.Challenge.Domain.Entities
{
    public class CourierEntity : IEntity
    {
        public CourierEntity(string name, string cnpj, DateTime dateOfBirth, string driverLicenseNumber, DriverLicenseType driverLicenseType)
        {
            Name = name;
            Cnpj = cnpj;
            DateOfBirth = dateOfBirth;
            DriverLicenseNumber = driverLicenseNumber;
            DriverLicenseType = driverLicenseType;
        }

        public CourierEntity(Guid id, string name, string cnpj, DateTime dateOfBirth, string driverLicenseNumber, DriverLicenseType driverLicenseType)
        {
            Id = id;
            Name = name;
            Cnpj = cnpj;
            DateOfBirth = dateOfBirth;
            DriverLicenseNumber = driverLicenseNumber;
            DriverLicenseType = driverLicenseType;
        }

        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Cnpj { get; private set; }
        public DateTime DateOfBirth { get; private set; }
        public string DriverLicenseNumber { get; private set; }
        public DriverLicenseType DriverLicenseType { get; private set; } 
        public string? DriverLicenseImageUrl { get; private set; }

        public void UpdateDriverLicenseImage(string driverLicenseImagePath)
        {
            DriverLicenseImageUrl = driverLicenseImagePath;
        }
    }

}
