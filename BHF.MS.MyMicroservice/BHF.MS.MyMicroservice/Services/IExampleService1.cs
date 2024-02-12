using BHF.MS.MyMicroservice.Models;

namespace BHF.MS.MyMicroservice.Services
{
    public interface IExampleService1
    {
        Task<HttpResponseMessage> DoSomethingAsync(ExampleModel model);
    }
}
