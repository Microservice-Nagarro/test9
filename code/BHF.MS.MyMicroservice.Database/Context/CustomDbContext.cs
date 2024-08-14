using BHF.MS.MyMicroservice.Database.Context.Models;
using Microsoft.EntityFrameworkCore;

namespace BHF.MS.MyMicroservice.Database.Context
{
    public class CustomDbContext(DbContextOptions<CustomDbContext> options) : DbContext(options)
    {
        public DbSet<DbItem> DbItems { get; set; }
    }
}
