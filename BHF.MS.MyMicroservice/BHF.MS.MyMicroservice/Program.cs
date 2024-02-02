using Azure.Identity;
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

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Enable Application Insights telemetry collection.
if (!builder.Environment.IsDevelopment())
{
    builder.Services.AddApplicationInsightsTelemetry();
    builder.Services.AddServiceProfiler();
}

// Add health check.
builder.Services.AddHealthChecks();

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