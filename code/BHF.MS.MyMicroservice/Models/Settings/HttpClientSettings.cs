using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace BHF.MS.MyMicroservice.Models.Settings
{
    [ExcludeFromCodeCoverage]
    public class HttpClientSettings
    {
        [Url]
        public string BaseAddress { get; set; } = string.Empty;
    }
}
