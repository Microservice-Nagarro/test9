using BHF.MS.MyMicroservice.Database.Context;
using BHF.MS.MyMicroservice.Database.Context.Entities;
using BHF.MS.MyMicroservice.Database.Models.DbItem;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace BHF.MS.MyMicroservice.Database.Services
{
    [ExcludeFromCodeCoverage(Justification = "There is no way to inject delay to DbSet mocks to make async state machine work as expected and calculate proper coverage results for any code calling await")]
    public class DbItemService(CustomDbContext context) : IDbItemService
    {
        public async Task<IList<DbItemDto>> GetAll()
        {
            return await context.DbItems
                .OrderByDescending(x => x.LastUpdated)
                .Take(100)
                .Select(x => new DbItemDto(x)).ToListAsync();
        }

        public async Task<DbItemDto?> GetById(Guid id)
        {
            return await context.DbItems.Where(x => x.Id == id).Select(x => new DbItemDto(x)).FirstOrDefaultAsync();
        }

        public async Task<bool> Update(DbItemDto model)
        {
            var dbItem = await context.DbItems.FindAsync(model.Id);
            if (dbItem == null)
            {
                return false;
            }

            dbItem.Update(model);

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await DbItemExists(model.Id))
                {
                    return false;
                }
            }

            return true;
        }

        public async Task<DbItemDto> Add(DbItemCreateDto model)
        {
            var dbItem = new DbItem { Name = model.Name };

            context.DbItems.Add(dbItem);
            await context.SaveChangesAsync();

            return new DbItemDto(dbItem);
        }

        public async Task<bool> Delete(Guid id)
        {
            var dbItem = await context.DbItems.FindAsync(id);
            if (dbItem == null)
            {
                return false;
            }

            context.DbItems.Remove(dbItem);
            await context.SaveChangesAsync();

            return true;
        }

        private async Task<bool> DbItemExists(Guid id)
        {
            return await context.DbItems.AnyAsync(x => x.Id == id);
        }
    }
}
