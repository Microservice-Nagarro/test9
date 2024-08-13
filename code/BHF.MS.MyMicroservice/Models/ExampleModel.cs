using System.ComponentModel.DataAnnotations;

namespace BHF.MS.MyMicroservice.Models
{
    public class ExampleModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Title length can't be more than 100 characters")]
        public required string Title { get; set; }

        [Range(0, 999.99)]
        public decimal Price { get; set; }

        [EmailAddress]
        public required string Email { get; set; }

        [Phone]
        public required string Phone { get; set; }

        public Dictionary<string, string>? Customisations { get; set; }
    }
}
