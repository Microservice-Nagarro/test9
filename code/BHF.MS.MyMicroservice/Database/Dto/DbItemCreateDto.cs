using System.Text.Json.Serialization;
using BHF.MS.MyMicroservice.Database.Models;

namespace BHF.MS.MyMicroservice.Database.Dto
{
    public class DbItemCreateDto
    {
        [JsonRequired]
        public string Name { get; set; } = string.Empty;

        public DbItemCreateDto()
        {
        }

        public DbItemCreateDto(DbItem dbItem)
        {
            Name = dbItem.Name;
        }
    }
}
