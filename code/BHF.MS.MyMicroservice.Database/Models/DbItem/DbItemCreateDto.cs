using System.Text.Json.Serialization;

namespace BHF.MS.MyMicroservice.Database.Models.DbItem
{
    public class DbItemCreateDto
    {
        [JsonRequired]
        public string Name { get; set; } = string.Empty;

        public DbItemCreateDto()
        {
        }

        public DbItemCreateDto(Context.Entities.DbItem dbItem)
        {
            Name = dbItem.Name;
        }
    }
}
