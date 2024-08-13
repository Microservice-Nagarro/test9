using BHF.MS.MyMicroservice.Models;
using BHF.MS.MyMicroservice.Models.Settings;
using Microsoft.Extensions.Options;

namespace BHF.MS.MyMicroservice.Services
{
    public class ExampleService(HttpClient httpClient, IOptions<ServiceSettings> settings) : IExampleService
    {
        public async Task<HttpResponseMessage> DoSomethingAsync(ExampleModel model)
        {
            return await httpClient.PostAsJsonAsync(settings.Value.Endpoint1Uri, model);
        }
    }
}
