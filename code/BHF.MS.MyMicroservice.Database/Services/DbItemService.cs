using BHF.MS.MyMicroservice.Database.Context;
using BHF.MS.MyMicroservice.Database.Context.Entities;
using BHF.MS.MyMicroservice.Database.Models.DbItem;
using Microsoft.EntityFrameworkCore;

namespace BHF.MS.MyMicroservice.Database.Services
{
    public class DbItemService(CustomDbContext context) : IDbItemService
    {
        public async Task<IList<DbItemDto>> GetAll(CancellationToken token = default)
        {
            return await context.DbItems
                .OrderByDescending(x => x.LastUpdated)
                .Take(100)
                .Select(x => new DbItemDto(x)).ToListAsync(token);
        }

        public async Task<DbItemDto?> GetById(Guid id, CancellationToken token = default)
        {
            return await context.DbItems.Where(x => x.Id == id).Select(x => new DbItemDto(x)).FirstOrDefaultAsync(token);
        }

        public async Task<bool> Update(DbItemDto model, CancellationToken token = default)
        {
            var dbItem = await context.DbItems.FindAsync([model.Id], token);
            if (dbItem == null)
            {
                return false;
            }

            dbItem.Update(model);

            try
            {
                await context.SaveChangesAsync(token);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await DbItemExists(model.Id, token))
                {
                    return false;
                }
            }

            return true;
        }

        public async Task<DbItemDto> Add(DbItemCreateDto model, CancellationToken token = default)
        {
            var dbItem = new DbItem { Name = model.Name };

            context.DbItems.Add(dbItem);
            await context.SaveChangesAsync(token);

            return new DbItemDto(dbItem);
        }

        public async Task<bool> Delete(Guid id, CancellationToken token = default)
        {
            var dbItem = await context.DbItems.FindAsync([id], token);
            if (dbItem == null)
            {
                return false;
            }

            context.DbItems.Remove(dbItem);
            await context.SaveChangesAsync(token);

            return true;
        }

        private async Task<bool> DbItemExists(Guid id, CancellationToken token = default)
        {
            return await context.DbItems.AnyAsync(x => x.Id == id, token);
        }
    }
}
