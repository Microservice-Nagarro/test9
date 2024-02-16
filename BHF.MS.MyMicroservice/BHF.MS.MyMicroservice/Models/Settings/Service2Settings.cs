using System.ComponentModel.DataAnnotations;

namespace BHF.MS.MyMicroservice.Models.Settings
{
    public class Service2Settings
    {
        [Required(ErrorMessage = "Name required")]
        public string Name { get; set; } = string.Empty;
        [Url]
        public string BaseAddress { get; set; } = string.Empty;
        public string Parameters { get; set; } = string.Empty;
    }
}
