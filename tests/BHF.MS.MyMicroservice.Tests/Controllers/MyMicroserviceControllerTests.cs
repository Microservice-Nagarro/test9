using BHF.MS.MyMicroservice.Controllers;
using BHF.MS.MyMicroservice.Models;
using BHF.MS.MyMicroservice.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace BHF.MS.MyMicroservice.Tests.Controllers
{
    public class MyMicroserviceControllerTests
    {
        private readonly Mock<ILogger<MyMicroserviceController>> _loggerMock = new();
        private readonly Mock<IExampleService> _exampleServiceMock = new();
        private readonly MyMicroserviceController _sut;

        public MyMicroserviceControllerTests()
        {
            _sut = new MyMicroserviceController(_loggerMock.Object, _exampleServiceMock.Object);
        }

        [Fact]
        public async Task Get_LogsWarning()
        {
            // Arrange
            var message = new HttpResponseMessage();
            var model = new ExampleModel { Title = "title", Email = "abc@abc.com", Phone = "123123123" };
            _exampleServiceMock.Setup(x => x.DoSomethingAsync(model)).ReturnsAsync(message);

            // Act
            var result = await _sut.Get(model);

            // Assert
            _loggerMock.VerifyLog(x => x.LogWarning("Responses {Response} are invalid!", message), Times.Once);
            result.Should().BeOfType<OkResult>();
        }
    }
}
