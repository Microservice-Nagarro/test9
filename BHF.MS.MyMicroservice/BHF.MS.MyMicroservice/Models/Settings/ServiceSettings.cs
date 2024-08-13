using System.ComponentModel.DataAnnotations;

namespace BHF.MS.MyMicroservice.Models.Settings
{
    public class ServiceSettings
    {
        [Required(ErrorMessage = "Name required")]
        public string Name { get; set; } = string.Empty;
        public string Endpoint1Uri { get; set; } = string.Empty;
        public string Parameters { get; set; } = string.Empty;
        public HttpClientSettings HttpClient { get; set; } = new();
    }
}
