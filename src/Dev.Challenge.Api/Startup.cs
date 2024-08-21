using Dev.Challenge.Crosscutting.DependencyInjector;
using Dev.Challenge.Crosscutting.MongoDbMapper;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Serilog.Formatting.Json;
using Serilog;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Dev.Challenge.Crosscutting.Migration;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.OpenApi.Models;

namespace Dev.Challenge.Api
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            //TODO: CONECTAR AO LOGZIO PARA LOGAR AS EXCEÇOES NO ELASTICSEARCH
            // Log.Logger = new LoggerConfiguration()
            //.WriteTo.Http("https://listener.logz.io:8071/?token=YOUR_LOGZIO_TOKEN&type=YOUR_LOG_TYPE",
            //              queueLimitBytes: null,
            //              textFormatter: new JsonFormatter())
            //.CreateLogger();

            // services.AddSingleton(Log.Logger);
            services.AddControllers();

            services.AddMemoryCache();

            ConfigureAuth(services);

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var errors = context.ModelState
                        .Where(e => e.Value.Errors.Count > 0)
                        .SelectMany(e => e.Value.Errors.Select(er => er.ErrorMessage))
                        .ToList();

                    var response = new
                    {
                        error = string.Join("; ", errors),
                        statusCode = (int)HttpStatusCode.BadRequest,
                        details = "One or more validation errors occurred."
                    };

                    return new JsonResult(response)
                    {
                        StatusCode = (int)HttpStatusCode.BadRequest
                    };
                };
            });

            // Configurar o Swagger para autenticação JWT
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Desafio Backend", Version = "v1" });

                // Definir esquema de segurança JWT
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                // Definir o requisito de segurança
                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    },
                    Scheme = "oauth2",
                    Name = "Bearer",
                    In = ParameterLocation.Header,
                },
                new List<string>()
            }
        });
            });

            DbMapper.Map();
            Injector.ConfigureServices(services, Configuration);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IMemoryCache memoryCache)
        {
            UserMigration.InitializeUsers(memoryCache);

            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private void ConfigureAuth(IServiceCollection services)
        {
            // Configurar JWT
            var keyString = Configuration["Jwt:Key"];
            if (string.IsNullOrEmpty(keyString))
            {
                throw new ArgumentNullException("Jwt:Key", "A chave JWT deve ser configurada.");
            }

            var key = Encoding.ASCII.GetBytes(keyString);

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            // Adicionar políticas de autorização baseadas no tipo de usuário
            services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminPolicy", policy => policy.RequireRole("ADMIN"));
                options.AddPolicy("EntregadorPolicy", policy => policy.RequireRole("ENTREGADOR"));
            });

            services.AddHttpContextAccessor();
        }
    }
}
