using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace BHF.MS.MyMicroservice.Models.Settings
{
    [ExcludeFromCodeCoverage(Justification = "It's a model with no logic")]
    public class HttpClientSettings
    {
        [Url]
        public string BaseAddress { get; set; } = string.Empty;
    }
}
