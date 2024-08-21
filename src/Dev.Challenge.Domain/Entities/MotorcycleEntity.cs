using Dev.Challenge.Domain.Contracts;
using Dev.Challenge.Domain.Exceptions;


namespace Dev.Challenge.Domain.Entities
{
    public class MotorcycleEntity : IEntity
    {
        public MotorcycleEntity(int year, string model, string licensePlate)
        {
            Year = year;
            Model = model;
            LicensePlate = licensePlate;
        }

        public MotorcycleEntity(Guid id, int year, string model, string licensePlate)
        {
            Id = id;
            Year = year;
            Model = model;
            LicensePlate = licensePlate;
        }

        public Guid Id { get; private set; }
        private int _year;
        public int Year
        {
            get => _year;
            private set
            {
                if (value < 2000)
                    throw new DomainException("\r\nO ano deve ser maior ou igual a 2000.");
                _year = value;
            }
        }

        private string _model;
        public string Model
        {
            get => _model;
            private set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new DomainException("O modelo é obrigatório.");
                if (value.Length > 50)
                    throw new DomainException("O modelo não pode ter mais de 50 caracteres.");
                _model = value;
            }
        }

        private string _licensePlate;
        public string LicensePlate
        {
            get => _licensePlate;
            private set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new DomainException("A placa é obrigatória.");
                if (!System.Text.RegularExpressions.Regex.IsMatch(value, "^[A-Z0-9]{7}$"))
                    throw new DomainException("A placa deve ter 7 caracteres, contendo apenas letras maiúsculas e números.");
                _licensePlate = value;
            }
        }

        public void UpdateLicensePlate(string newLicensePlate)
        {
            LicensePlate = newLicensePlate;
        }
    }

}
