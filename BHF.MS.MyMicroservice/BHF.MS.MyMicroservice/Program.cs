using Azure.Identity;
using BHF.MS.MyMicroservice.HealthCheck;
using BHF.MS.MyMicroservice.Models.HealthCheck;
using BHF.MS.MyMicroservice.Models.Settings;
using BHF.MS.MyMicroservice.Services;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

if (!builder.Environment.IsDevelopment())
{
    var credentials = new DefaultAzureCredential(new DefaultAzureCredentialOptions
    {
        ManagedIdentityClientId = builder.Configuration.GetConnectionString("ManagedIdentityClientId")
    });

    var connectionString = builder.Configuration.GetConnectionString("KeyVaultUri");
    if (!string.IsNullOrEmpty(connectionString))
    {
        var keyVaultUri = new Uri(connectionString);

        builder.Configuration.AddAzureKeyVault(
            keyVaultUri,
            credentials);
    }
}

// Add services to the container.
builder.Services.AddOptions<Service1Settings>()
    .Bind(builder.Configuration.GetSection("Service1"))
    .ValidateDataAnnotations()
    .ValidateOnStart();

builder.Services.AddOptions<Service2Settings>()
    .Bind(builder.Configuration.GetSection("Service2"))
    .ValidateDataAnnotations()
    .ValidateOnStart();

builder.Services.AddOptions<HealthCheckSettings>()
    .Bind(builder.Configuration.GetSection("HealthCheck"))
    .ValidateDataAnnotations()
    .ValidateOnStart();

builder.Services.AddHttpClient();

builder.Services.AddTransient<MicroserviceHealthCheck>();

builder.Services.AddControllers();

builder.Services.AddSwaggerGen();

builder.Services.AddTransient<IExampleService1, ExampleService1>();
builder.Services.AddTransient<IExampleService2, ExampleService2>();

// Enable Application Insights telemetry collection.
if (!builder.Environment.IsDevelopment())
{
    builder.Services.AddApplicationInsightsTelemetry();
    builder.Services.AddServiceProfiler();
    var logLevel = (LogLevel)Enum.Parse(typeof(LogLevel), builder.Configuration["Logging:ApplicationInsights:LogLevel:Default"]!);
    builder.Services.AddApplicationInsightsKubernetesEnricher(diagnosticLogLevel: logLevel);
}

// Add health check.
builder.Services.AddHealthChecks()
    .AddCheck<MicroserviceHealthCheck>(
    nameof(MicroserviceHealthCheck),
    HealthStatus.Unhealthy,
    new[] { "ready" });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapHealthChecks("/healthz/ready", new HealthCheckOptions
{
    Predicate = healthCheck => healthCheck.Tags.Contains("ready")
});

app.MapHealthChecks("/healthz/live", new HealthCheckOptions
{
    Predicate = _ => false
});

app.Run();