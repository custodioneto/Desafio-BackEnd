using Dev.Challenge.Domain.Enums;
using Dev.Challenge.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dev.Challenge.Domain.Extensions
{
    public static class EnumExtension
    {
        public static DriverLicenseType ParseToDriverLicenseType(this string enumStr)
        {
            if (!Enum.TryParse<DriverLicenseType>(enumStr, true, out var licenseType))
            {
                throw new DomainException("Categoria de Habilitação inválida.");
            }

            return licenseType;
        }
    }
}
