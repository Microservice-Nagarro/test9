using BHF.MS.MyMicroservice.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace BHF.MS.MyMicroservice.Database
{
    public class CustomDbContext(DbContextOptions<CustomDbContext> options) : DbContext(options)
    {
        public DbSet<DbItem> DbItems { get; set; }
    }
}
