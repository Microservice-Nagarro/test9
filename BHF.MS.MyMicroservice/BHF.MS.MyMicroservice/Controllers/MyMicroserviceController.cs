using BHF.MS.MyMicroservice.Models;
using BHF.MS.MyMicroservice.Services;
using Microsoft.AspNetCore.Mvc;

namespace BHF.MS.MyMicroservice.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MyMicroserviceController(ILogger<MyMicroserviceController> logger,
        IExampleService1 service1,
        IExampleService2 service2) : ControllerBase
    {
        [HttpGet]
        public async Task Get(ExampleModel requestModel)
        {
            var response = await service1.DoSomethingAsync(requestModel);
            var anotherResponse = await service2.DoSomethingAsync(requestModel);

            // Logger messages should be formatted like so to avoid CA2254 warnings. Please avoid concatenation and interpolation.
            logger.LogWarning("Responses {response} and {anotherResponse} are invalid!", response, anotherResponse);
        }
    }
}