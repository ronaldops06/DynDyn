using Api.CrossCutting.Mappings;
using AutoMapper;
using CrossCutting.DependencyInjection;
using CrossCutting.Mappings;
using Data.Context;
using Domain.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace Api.Integration.Test
{
    public class FakeStartup
    {
        private IWebHostEnvironment _environment;
        private string _dataBaseName = $"dbApiTest_{Guid.NewGuid().ToString().Replace("-", string.Empty)}";

        public FakeStartup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            _environment = environment;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            if (_environment.IsEnvironment("Testing"))
            {
                Environment.SetEnvironmentVariable("DB_CONNECTION", $"Data Source={_dataBaseName}.db");
                Environment.SetEnvironmentVariable("DATABASE", "SQLITE");
                Environment.SetEnvironmentVariable("MIGRATION", "APLICAR");
                Environment.SetEnvironmentVariable("Audience", "ExemploAudience");
                Environment.SetEnvironmentVariable("Issuer", "ExemploIssuer");
                Environment.SetEnvironmentVariable("Seconds", "28800");
            }

            ConfigureService.ConfigureDependenciesService(services);
            ConfigureRepository.ConfigureDependenciesRepository(services);
            services.AddDbContext<SomniaContext>(x => x.UseSqlite($"Data Source={_dataBaseName}.db"), ServiceLifetime.Transient);

            var config = new AutoMapper.MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new DtoToModelProfile());
                cfg.AddProfile(new EntityToModelProfile());
                cfg.AddProfile(new DictionaryToModelProfile());
            });

            IMapper mapper = config.CreateMapper();
            services.AddSingleton(mapper);

            var signingConfiguration = new SigningConfiguration();
            services.AddSingleton(signingConfiguration);

            services.AddAuthentication(authOptions =>
            {
                authOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                authOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(bearerOptions =>
            {
                var paramsValidation = bearerOptions.TokenValidationParameters;
                paramsValidation.IssuerSigningKey = signingConfiguration.Key;
                paramsValidation.ValidAudience = Environment.GetEnvironmentVariable("Audience");
                paramsValidation.ValidIssuer = Environment.GetEnvironmentVariable("Issuer");
                paramsValidation.ValidateIssuerSigningKey = true;
                paramsValidation.ClockSkew = TimeSpan.Zero;
            });

            services.AddAuthorization(auth =>
            {
                auth.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser().Build());
            });

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "application", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "Entre com o Token JWT",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Id = "Bearer",
                                Type = ReferenceType.SecurityScheme
                            }
                        }, new List<string>()
                    }
                });
            });

            services.AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            })
            .AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
            });

            services.AddAutoMapper(typeof(DtoToModelProfile), typeof(DictionaryToModelProfile)/*, typeof(ModelToEntityProfile)*/);
        }

        public static void Configure(IApplicationBuilder app, IWebHostEnvironment env, SomniaContext context)
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Test API v1");
                c.RoutePrefix = string.Empty;
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            app.UseAuthentication();

            app.UseCors(x =>
            {
                x.AllowAnyOrigin();
                x.AllowAnyMethod();
                x.AllowAnyHeader();
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
