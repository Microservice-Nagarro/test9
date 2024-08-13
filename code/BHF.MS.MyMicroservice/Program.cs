using System.Diagnostics;
using Azure.Identity;
using BHF.MS.MyMicroservice.Database;
using BHF.MS.MyMicroservice.HealthCheck;
using BHF.MS.MyMicroservice.Models.HealthCheck;
using BHF.MS.MyMicroservice.Models.Settings;
using BHF.MS.MyMicroservice.Services;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;

namespace BHF.MS.MyMicroservice
{
    internal static class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            AddKeyVaultIntegration(builder);
            AddOptions(builder);
            AddDatabases(builder);
            AddCustomServices(builder);
            AddHttpClients(builder);

            builder.Services.AddControllers();
            builder.Services.AddSwaggerGen();

            // Enable Application Insights telemetry collection.
            if (!builder.Environment.IsDevelopment())
            {
                builder.Services.AddApplicationInsightsTelemetry();
                builder.Services.AddServiceProfiler();
                var logLevel = (LogLevel)Enum.Parse(typeof(LogLevel), builder.Configuration["Logging:ApplicationInsights:LogLevel:Default"]!);
                builder.Services.AddApplicationInsightsKubernetesEnricher(diagnosticLogLevel: logLevel);
            }

            // Add health check.
            AddHealthChecks(builder);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            app.UseSwagger();
            if (app.Environment.IsDevelopment())
            {
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            MapHealthChecks(app);

            await app.RunAsync();
        }

        private static void AddDatabases(WebApplicationBuilder builder)
        {
            builder.Services.AddDbContext<CustomDbContext>(options =>
            {
                options.UseInMemoryDatabase("Custom");
            });
        }

        private static void AddKeyVaultIntegration(WebApplicationBuilder builder)
        {
            if (builder.Environment.IsDevelopment())
            {
                return;
            }

            var credentials = new DefaultAzureCredential(new DefaultAzureCredentialOptions
            {
                ManagedIdentityClientId = builder.Configuration.GetConnectionString("ManagedIdentityClientId")
            });
            var connectionString = builder.Configuration.GetConnectionString("KeyVaultUri");
            if (string.IsNullOrEmpty(connectionString))
            {
                return;
            }

            var keyVaultUri = new Uri(connectionString);
            builder.Configuration.AddAzureKeyVault(
                keyVaultUri,
                credentials);
        }

        private static void AddHttpClients(WebApplicationBuilder builder)
        {
            builder.Services.AddHttpClient<IExampleService, ExampleService>((serviceProvider, client) =>
            {
                var options = serviceProvider.GetRequiredService<IOptions<ServiceSettings>>().Value;
                Debug.WriteLine($"url: {options.HttpClient.BaseAddress},{options.Name}");
                client.BaseAddress = new Uri(options.HttpClient.BaseAddress);
            });
        }

        private static void AddCustomServices(WebApplicationBuilder builder)
        {
            builder.Services.AddTransient<MicroserviceHealthCheck>();
            builder.Services.AddTransient<IExampleService, ExampleService>();
        }

        private static void AddOptions(WebApplicationBuilder builder)
        {
            builder.Services.AddOptions<ServiceSettings>()
                .Bind(builder.Configuration.GetSection("ServiceSettings"))
                .ValidateDataAnnotations()
                .ValidateOnStart();

            builder.Services.AddOptions<HealthCheckSettings>()
                .Bind(builder.Configuration.GetSection("HealthCheck"))
                .ValidateDataAnnotations()
                .ValidateOnStart();
        }

        private static void AddHealthChecks(WebApplicationBuilder builder)
        {
            builder.Services.AddHealthChecks()
                .AddCheck<MicroserviceHealthCheck>(
                    nameof(MicroserviceHealthCheck),
                    HealthStatus.Unhealthy,
                    ["ready"]);
        }

        private static void MapHealthChecks(WebApplication app)
        {
            app.MapHealthChecks("/healthz/ready", new HealthCheckOptions
            {
                Predicate = healthCheck => healthCheck.Tags.Contains("ready")
            });

            app.MapHealthChecks("/healthz/live", new HealthCheckOptions
            {
                Predicate = _ => false
            });
        }
    }
}
