using System.ComponentModel.DataAnnotations;
using BHF.MS.MyMicroservice.Database.Dto;

namespace BHF.MS.MyMicroservice.Database.Models
{
    public class DbItem
    {
        public Guid Id { get; set; }

        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        public DateTime LastUpdated { get; private set; } = DateTime.UtcNow;

        public void Update(DbItemDto dto)
        {
            Name = dto.Name;
            LastUpdated = DateTime.UtcNow;
        }
    }
}
