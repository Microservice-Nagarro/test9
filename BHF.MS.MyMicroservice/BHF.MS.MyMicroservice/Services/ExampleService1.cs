using BHF.MS.MyMicroservice.Models;
using BHF.MS.MyMicroservice.Models.Settings;
using Microsoft.Extensions.Options;

namespace BHF.MS.MyMicroservice.Services
{
    public class ExampleService1(IHttpClientFactory httpClientFactory, Service1Settings settings) : IExampleService1
    {
        public ExampleService1(IHttpClientFactory httpClientFactory, IOptions<Service1Settings> settings) : this(httpClientFactory, settings.Value) { }

        public async Task<HttpResponseMessage> DoSomethingAsync(ExampleModel model)
        {
            var client = httpClientFactory.CreateClient();
            var requestUri = settings.Uri;
            return await client.PostAsJsonAsync(requestUri, model);
        }
    }
}
