using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using BHF.MS.MyMicroservice.Database.Context.Entities;

namespace BHF.MS.MyMicroservice.Database.Context
{
    [ExcludeFromCodeCoverage]
    public class CustomDbContext(DbContextOptions<CustomDbContext> options) : DbContext(options)
    {
        protected CustomDbContext() : this(new DbContextOptions<CustomDbContext>())
        {
        }

        public virtual DbSet<DbItem> DbItems { get; set; }
    }
}
