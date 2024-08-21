using Dev.Challenge.Domain.Entities;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson;
using Dev.Challenge.Domain.Enums;

namespace Dev.Challenge.Infrastructure.Mapping.MongoDb
{
    public static class MongoDbMappings
    {
        public static void RegisterClassMaps()
        {
            if (!BsonClassMap.IsClassMapRegistered(typeof(MotorcycleEntity)))
            {
                BsonClassMap.RegisterClassMap<MotorcycleEntity>(cm =>
                {
                    cm.AutoMap();
                    cm.SetIgnoreExtraElements(true);
                    cm.MapMember(c => c.Id).SetSerializer(new GuidSerializer(BsonType.String)).SetElementName("id");
                    cm.MapMember(c => c.Year).SetElementName("year");
                    cm.MapMember(c => c.Model).SetElementName("model");
                    cm.MapMember(c => c.LicensePlate).SetElementName("licensePlate");
                });
            }

            if (!BsonClassMap.IsClassMapRegistered(typeof(CourierEntity)))
            {
                BsonClassMap.RegisterClassMap<CourierEntity>(cm =>
                {
                    cm.AutoMap();
                    cm.SetIgnoreExtraElements(true);
                    cm.MapMember(c => c.Id).SetSerializer(new GuidSerializer(BsonType.String)).SetElementName("id");
                    cm.MapMember(c => c.Name).SetElementName("name");
                    cm.MapMember(c => c.Cnpj).SetElementName("cnpj");
                    cm.MapMember(c => c.DateOfBirth).SetElementName("dateOfBirth");
                    cm.MapMember(c => c.DriverLicenseNumber).SetElementName("driverLicenseNumber");

                    cm.MapMember(c => c.DriverLicenseType)
                        .SetSerializer(new EnumSerializer<DriverLicenseType>(BsonType.String))
                        .SetElementName("driverLicenseType");

                    cm.MapMember(c => c.DriverLicenseImageUrl).SetElementName("driverLicenseImageUrl");
                });
            }

            if (!BsonClassMap.IsClassMapRegistered(typeof(RentalEntity)))
            {
                BsonClassMap.RegisterClassMap<RentalEntity>(cm =>
                {
                    cm.AutoMap();
                    cm.SetIgnoreExtraElements(true);
                    cm.MapMember(c => c.Id).SetSerializer(new GuidSerializer(BsonType.String)).SetElementName("id");
                    cm.MapMember(c => c.MotorcycleId).SetSerializer(new GuidSerializer(BsonType.String)).SetElementName("motorcycleId");
                    cm.MapMember(c => c.CourierId).SetSerializer(new GuidSerializer(BsonType.String)).SetElementName("courierId");
                    cm.MapMember(c => c.StartDate).SetElementName("startDate");
                    cm.MapMember(c => c.EndDate).SetElementName("endDate");
                    cm.MapMember(c => c.ExpectedEndDate).SetElementName("expectedEndDate");
                    cm.MapMember(c => c.TotalAmount).SetElementName("totalAmount");
                    cm.MapMember(c => c.PenaltyFee).SetElementName("penaltyFee");
                });
            }
        }
    }
}

