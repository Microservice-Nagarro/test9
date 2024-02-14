using System.ComponentModel.DataAnnotations;

namespace BHF.MS.MyMicroservice.Models.Settings
{
    public class Service1Settings
    {
        [StringLength(128)]
        public string Category { get; set; } = string.Empty;
        [Url]
        public string Uri { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }
}
