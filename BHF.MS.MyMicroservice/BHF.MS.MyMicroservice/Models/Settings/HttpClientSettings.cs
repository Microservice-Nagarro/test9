using System.ComponentModel.DataAnnotations;

namespace BHF.MS.MyMicroservice.Models.Settings
{
    public class HttpClientSettings
    {
        [Url]
        public string BaseAddress { get; set; } = string.Empty;
    }
}
