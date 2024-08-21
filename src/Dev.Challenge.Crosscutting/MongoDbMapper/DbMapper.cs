using Dev.Challenge.Infrastructure.Mapping.MongoDb;


namespace Dev.Challenge.Crosscutting.MongoDbMapper
{
    public static class DbMapper
    {
        public static void Map()
        {
            MongoDbMappings.RegisterClassMaps();
        }
    }
}
