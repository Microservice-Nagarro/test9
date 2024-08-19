using BHF.MS.MyMicroservice.Models;
using BHF.MS.MyMicroservice.Services;
using Microsoft.AspNetCore.Mvc;

namespace BHF.MS.MyMicroservice.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MyMicroserviceController(ILogger<MyMicroserviceController> logger, IExampleService service) : ControllerBase
    {
        [HttpGet]
        [ResponseCache(VaryByQueryKeys = ["*"], Duration = 60)]
        public async Task<IActionResult> Get([FromQuery] ExampleModel requestModel)
        {
            var response = await service.DoSomethingAsync(requestModel);

            // Logger messages should be formatted like so to avoid CA2254 warnings. Please avoid concatenation and interpolation.
            logger.LogWarning("Responses {Response} are invalid!", response);

            return Ok();
        }
    }
}
