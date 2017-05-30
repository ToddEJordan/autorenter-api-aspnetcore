using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using AutoRenter.Api.Controllers;
using AutoRenter.Api.Services;
using AutoRenter.Domain.Interfaces;
using AutoRenter.Domain.Models;

namespace AutoRenter.Api.Tests
{
    public class LogControllerTests
    {
        [Fact]
        public async void Post_WhenValid()
        {
            // arrange
            var logService = new Mock<ILogService>();
            logService.Setup(x => x.Log(It.IsAny<LogEntry>()))
                .ReturnsAsync(new Result<object>(ResultCode.Success));

            IResultCodeProcessor processor = new ResultCodeProcessor();
            var sut = new LogController(logService.Object, processor);
            var logEntry = new LogEntry()
            {
                Level = "warning",
                Message = "test message"
            };

            // act
            var result = await sut.Post(logEntry);
            var createdResult = result as CreatedResult;

            // assert
            Assert.NotNull(createdResult);
        }

        [Fact]
        public async void Post_WhenNotValid()
        {
            // arrange
            var logService = new Mock<ILogService>();
            logService.Setup(x => x.Log(It.IsAny<LogEntry>()))
                .ReturnsAsync(new Result<object>(ResultCode.BadRequest));

            IResultCodeProcessor processor = new ResultCodeProcessor();
            var sut = new LogController(logService.Object, processor);
            var logEntry = new LogEntry()
            {
                Level = "warning",
                Message = "test message"
            };

            // act
            var result = await sut.Post(logEntry);
            var badRequestResult = result as BadRequestResult;

            // assert
            Assert.NotNull(badRequestResult);
        }

        [Fact]
        public async void Post_PassesMessageToService()
        {
            // arrange
            var message = "Test message";
            var logService = new Mock<ILogService>();
            logService.Setup(x => x.Log(It.IsAny<LogEntry>()))
                .ReturnsAsync(new Result<object>(ResultCode.Success));

            IResultCodeProcessor processor = new ResultCodeProcessor();
            var sut = new LogController(logService.Object, processor);
            var logEntry = new LogEntry()
            {
                Level = "warning",
                Message = message
            };

            // act
            var result = await sut.Post(logEntry);
            var createdResult = result as CreatedResult;

            // assert
            logService.Verify(x => x.Log(It.Is<LogEntry>(entry => entry.Message == message)));
        }

        [Fact]
        public async void Post_PassesLevelToService()
        {
            // arrange
            var level = "info";
            var logService = new Mock<ILogService>();
            logService.Setup(x => x.Log(It.IsAny<LogEntry>()))
                .ReturnsAsync(new Result<object>(ResultCode.Success));

            IResultCodeProcessor processor = new ResultCodeProcessor();
            var sut = new LogController(logService.Object, processor);
            var logEntry = new LogEntry()
            {
                Level = level,
                Message = "Test message"
            };

            // act
            var result = await sut.Post(logEntry);
            var createdResult = result as CreatedResult;

            // assert
            logService.Verify(x => x.Log(It.Is<LogEntry>(entry => entry.Level == level)));
        }
    }
}
