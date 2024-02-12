using BHF.MS.MyMicroservice.Models;
using BHF.MS.MyMicroservice.Models.Settings;
using Microsoft.Extensions.Options;
using System.Net.Http;

namespace BHF.MS.MyMicroservice.Services
{
    public class ExampleService1(IHttpClientFactory httpClientFactory, IOptions<Service1Settings>? settings) : IExampleService1
    {
        public async Task<HttpResponseMessage> DoSomethingAsync(ExampleModel model)
        {
            var client = httpClientFactory.CreateClient();
            var requestUri = settings?.Value.Uri;
            return await client.PostAsJsonAsync(requestUri, model);
        }
    }
}
