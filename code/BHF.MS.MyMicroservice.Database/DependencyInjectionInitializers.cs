using BHF.MS.MyMicroservice.Database.Context;
using BHF.MS.MyMicroservice.Database.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BHF.MS.MyMicroservice.Database
{
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
    }
}
