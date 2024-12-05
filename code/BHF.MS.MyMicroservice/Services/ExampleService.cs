using BHF.MS.test9.Models;
using BHF.MS.test9.Models.Settings;
using Microsoft.Extensions.Options;

namespace BHF.MS.test9.Services
{
    public class ExampleService(HttpClient httpClient, IOptions<ServiceSettings> settings) : IExampleService
    {
        public async Task<HttpResponseMessage> PostSomething(ExampleModel model)
        {
            return await httpClient.PostAsJsonAsync(settings.Value.Endpoint1Uri, model);
        }

        public async Task<HttpResponseMessage> GetSomething()
        {
            return await httpClient.GetAsync(settings.Value.Endpoint1Uri);
        }
    }
}

