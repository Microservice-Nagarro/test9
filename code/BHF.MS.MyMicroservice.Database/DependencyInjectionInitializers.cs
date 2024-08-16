using BHF.MS.MyMicroservice.Database.Context;
using BHF.MS.MyMicroservice.Database.HealthCheck;
using BHF.MS.MyMicroservice.Database.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace BHF.MS.MyMicroservice.Database
{
    [ExcludeFromCodeCoverage(Justification = "It's an IoC initializer")]
    public static class DependencyInjectionInitializers
    {
        public static void AddDatabases(IServiceCollection serviceCollection)
        {
            serviceCollection.AddDbContext<CustomDbContext>(options =>
            {
                options.UseInMemoryDatabase("Custom");
            });
        }

        public static void AddCustomServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IDbItemService, DbItemService>();
        }

        public static void AddHealthCheckConfiguration(IServiceCollection serviceCollection)
        {
            serviceCollection.AddHealthChecks()
                .AddCheck<DbHealthCheck>(
                    nameof(DbHealthCheck),
                    tags: ["ready"]);
        }
    }
}
