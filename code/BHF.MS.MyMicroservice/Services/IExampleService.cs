using BHF.MS.MyMicroservice.Models;

namespace BHF.MS.MyMicroservice.Services
{
    public interface IExampleService
    {
        Task<HttpResponseMessage> PostSomething(ExampleModel model);
        Task<HttpResponseMessage> GetSomething();
    }
}
