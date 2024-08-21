using Dev.Challenge.Application.Queue;
using Dev.Challenge.Application.Repository;
using Dev.Challenge.Application.Service;
using Dev.Challenge.Application.Storage;
using Dev.Challenge.Infrastructure.Consumer;
using Dev.Challenge.Infrastructure.Queue;
using Dev.Challenge.Infrastructure.Repository;
using Dev.Challenge.Infrastructure.Service;
using Dev.Challenge.Infrastructure.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace Dev.Challenge.Crosscutting.DependencyInjector
{
    public static class Injector
    {
        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());
            });
            // Configuração do MongoDB
            services.AddSingleton<IMongoClient>(sp => new MongoClient(configuration.GetConnectionString("MongoDb")));
            services.AddScoped(sp => sp.GetRequiredService<IMongoClient>().GetDatabase(configuration["MongoDbDatabaseName"]));

            services.AddScoped<IAuthService, AuthService>();

            // Registro dos repositórios
            services.AddScoped<IMotorcycleRepository, MotorcycleRepository>();
            services.AddScoped<ICourierRepository, CourierRepository>();
            services.AddScoped<IRentalRepository, RentalRepository>();

            services.AddScoped<IMotorcycleService, MotorcycleService>();
            services.AddScoped<ICourierService, CourierService>();
            services.AddScoped<IRentalService, RentalService>();

            services.AddSingleton<IRabbitMQService, RabbitMQService>();
            services.AddHostedService<MotorcycleNotificationService>();


            services.AddScoped<IStorageService, GridFSStorageService>();
        }
    }
}