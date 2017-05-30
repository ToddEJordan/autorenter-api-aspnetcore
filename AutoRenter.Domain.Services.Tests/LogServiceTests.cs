using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using AutoRenter.Domain.Interfaces;
using AutoRenter.Domain.Models;

namespace AutoRenter.Domain.Services.Tests
{
    public class LogServiceTests
    {
        [Fact]
        public async void Log_Success()
        {
            // arrange
            var validationServiceMoq = new Mock<IValidationService>();
            validationServiceMoq.Setup(x => x.IsValidInsert(It.IsAny<LogEntry>()))
                .ReturnsAsync(true);

            var loggerMoq = new Mock<ILogger>();
            var loggerFactoryMoq = new Mock<ILoggerFactory>();
            loggerFactoryMoq.Setup(x => x.CreateLogger(It.IsAny<string>()))
                .Returns(loggerMoq.Object);

            var logEntry = new LogEntry
            {
                Level = "Error",
                Message = "Test error message"
            };

            var sut = new LogService(loggerFactoryMoq.Object, validationServiceMoq.Object);

            // act
            var result = await sut.Log(logEntry);

            // assert
            Assert.Equal(ResultCode.Success, result.ResultCode);
        }

        [Fact]
        public async void Log_WhenInvalid()
        {
            // arrange
            var validationServiceMoq = new Mock<IValidationService>();
            validationServiceMoq.Setup(x => x.IsValidInsert(It.IsAny<LogEntry>()))
                .ReturnsAsync(false);

            var loggerMoq = new Mock<ILogger>();
            var loggerFactoryMoq = new Mock<ILoggerFactory>();
            loggerFactoryMoq.Setup(x => x.CreateLogger(It.IsAny<string>()))
                .Returns(loggerMoq.Object);

            var logEntry = new LogEntry
            {
                Level = "Error",
                Message = "Test error message"
            };

            var sut = new LogService(loggerFactoryMoq.Object, validationServiceMoq.Object);

            // act
            var result = await sut.Log(logEntry);

            // assert
            Assert.Equal(ResultCode.BadRequest, result.ResultCode);
        }

        [Fact]
        public async void Log_WhenBadLevel()
        {
            // arrange
            var validationServiceMoq = new Mock<IValidationService>();
            validationServiceMoq.Setup(x => x.IsValidInsert(It.IsAny<LogEntry>()))
                .ReturnsAsync(true);

            var loggerMoq = new Mock<ILogger>();
            var loggerFactoryMoq = new Mock<ILoggerFactory>();
            loggerFactoryMoq.Setup(x => x.CreateLogger(It.IsAny<string>()))
                .Returns(loggerMoq.Object);

            var logEntry = new LogEntry
            {
                Level = "BadLevel",
                Message = "Test error message"
            };

            var sut = new LogService(loggerFactoryMoq.Object, validationServiceMoq.Object);

            // act
            var result = await sut.Log(logEntry);

            // assert
            Assert.Equal(ResultCode.BadRequest, result.ResultCode);
        }

        [Theory]
        [InlineData("Info")]
        [InlineData("Information")]
        [InlineData("Debug")]
        [InlineData("Warn")]
        [InlineData("Warning")]
        [InlineData("Error")]
        public async void Log_LevelsAreValid(string logLevel)
        {
            // arrange
            var message = "Test error message";
            var validationServiceMoq = new Mock<IValidationService>();
            validationServiceMoq.Setup(x => x.IsValidInsert(It.IsAny<LogEntry>()))
                .ReturnsAsync(true);

            var loggerMoq = new Mock<ILogger>();
            var loggerFactoryMoq = new Mock<ILoggerFactory>();
            loggerFactoryMoq.Setup(x => x.CreateLogger(It.IsAny<string>()))
                .Returns(loggerMoq.Object);

            var logEntry = new LogEntry
            {
                Level = logLevel,
                Message = message
            };

            var sut = new LogService(loggerFactoryMoq.Object, validationServiceMoq.Object);

            // act
            var result = await sut.Log(logEntry);

            // assert
            Assert.Equal(ResultCode.Success, result.ResultCode);
        }

        //TODO - We can not determine if the correct log level was interpretted by the service layer.
        // Service layer uses extensions for clean code. Cannot moq an extension.
    }
}
