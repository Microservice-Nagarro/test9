using System.Text.Json.Serialization;
using BHF.MS.MyMicroservice.Database.Context.Models;

namespace BHF.MS.MyMicroservice.Database.Dto
{
    public class DbItemDto : DbItemCreateDto
    {
        [JsonRequired]
        public Guid Id { get; set; }

        public DbItemDto()
        {
        }

        public DbItemDto(DbItem dbItem) : base(dbItem)
        {
            Id = dbItem.Id;
        }
    }
}
