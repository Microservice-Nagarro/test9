using Azure.Identity;
using BHF.MS.MyMicroservice.Models.Settings;
using BHF.MS.MyMicroservice.Services;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using System.Diagnostics.CodeAnalysis;
using BHF.MS.MyMicroservice.Database.HealthCheck;
using BHF.MS.MyMicroservice.Database.Models.HealthCheck;

namespace BHF.MS.MyMicroservice
{
    [ExcludeFromCodeCoverage]
    public static class DependencyInjectionInitializers
    {
        public static void AddOptionsConfiguration(WebApplicationBuilder builder)
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

        public static void AddCustomServices(IServiceCollection serviceCollection)
        {
            Database.DependencyInjectionInitializers.AddDatabases(serviceCollection);
            Database.DependencyInjectionInitializers.AddCustomServices(serviceCollection);

            serviceCollection.AddTransient<DbHealthCheck>();
            serviceCollection.AddTransient<IExampleService, ExampleService>();

            AddHttpClients(serviceCollection);
        }

        private static void AddHttpClients(IServiceCollection serviceCollection)
        {
            serviceCollection.AddHttpClient<IExampleService, ExampleService>((serviceProvider, client) =>
            {
                var options = serviceProvider.GetRequiredService<IOptions<ServiceSettings>>().Value;
                client.BaseAddress = new Uri(options.HttpClient.BaseAddress);
            });
        }

        public static void MapHealthChecks(WebApplication app)
        {
            app.MapHealthChecks("/healthz/ready",
                new HealthCheckOptions { Predicate = healthCheck => healthCheck.Tags.Contains("ready") });

            app.MapHealthChecks("/healthz/live", new HealthCheckOptions { Predicate = _ => false });
        }

        public static void AddKeyVaultIntegration(WebApplicationBuilder builder)
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

        public static void AddApplicationInsights(WebApplicationBuilder builder)
        {
            if (builder.Environment.IsDevelopment())
            {
                return;
            }

            builder.Services.AddApplicationInsightsTelemetry();
            builder.Services.AddServiceProfiler();
            var logLevel = (LogLevel)Enum.Parse(typeof(LogLevel),
                builder.Configuration["Logging:ApplicationInsights:LogLevel:Default"]!);
            builder.Services.AddApplicationInsightsKubernetesEnricher(logLevel);
        }
    }
}
