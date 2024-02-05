using Microsoft.AspNetCore.Mvc;

namespace BHF.MS.MyMicroservice.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MyMicroserviceController : ControllerBase
    {
        private readonly ILogger<MyMicroserviceController> _logger;

        public MyMicroserviceController(ILogger<MyMicroserviceController> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger)); ;
        }

        [HttpGet]
        public void Get()
        {

        }
    }
}