using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BHF.MS.MyMicroservice.Database;
using BHF.MS.MyMicroservice.Database.Dto;
using BHF.MS.MyMicroservice.Database.Models;

namespace BHF.MS.MyMicroservice.Controllers
{
    /// <summary>
    /// Sample REST API controller. Please keep in mind, that controller itself should not contain business logic and any further implementation should move away use of EF context to a separate class.
    /// This should be treated as a sample, how a typical REST controller should look like and behave.
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    public class DbItemsController(CustomDbContext context) : ControllerBase
    {
        /// <summary>
        /// Consider adding paging for such endpoint
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DbItemDto>>> GetDbItems()
        {
            return await context.DbItems
                .OrderByDescending(x => x.LastUpdated)
                .Take(100)
                .Select(x => new DbItemDto(x)).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DbItemDto>> GetDbItem(Guid id)
        {
            var dbItem = await context.DbItems.Where(x => x.Id == id).Select(x => new DbItemDto(x)).FirstOrDefaultAsync();

            if (dbItem == null)
            {
                return NotFound();
            }

            return dbItem;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutDbItem(Guid id, DbItemDto dbItemDto)
        {
            if (id != dbItemDto.Id)
            {
                return BadRequest();
            }

            var dbItem = await context.DbItems.FindAsync(id);
            if (dbItem == null)
            {
                return NotFound();
            }

            dbItem.Update(dbItemDto);

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await DbItemExists(id))
                {
                    return NotFound();
                }
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<DbItemDto>> PostDbItem(DbItemCreateDto dbItemDto)
        {
            var dbItem = new DbItem { Name = dbItemDto.Name };

            context.DbItems.Add(dbItem);
            await context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetDbItem), new { id = dbItem.Id }, new DbItemDto(dbItem));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDbItem(Guid id)
        {
            var dbItem = await context.DbItems.FindAsync(id);
            if (dbItem == null)
            {
                return NotFound();
            }

            context.DbItems.Remove(dbItem);
            await context.SaveChangesAsync();

            return NoContent();
        }

        private async Task<bool> DbItemExists(Guid id)
        {
            return await context.DbItems.AnyAsync(x => x.Id == id);
        }
    }
}
