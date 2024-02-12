using BHF.MS.MyMicroservice.Models;

namespace BHF.MS.MyMicroservice.Services
{
    public interface IExampleService2
    {
        Task<HttpResponseMessage> DoSomethingAsync(ExampleModel model);
    }
}
