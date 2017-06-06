using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using AutoRenter.Api.Controllers;
using AutoRenter.Api.Services;
using AutoRenter.Domain.Interfaces;
using AutoRenter.Domain.Models;
using AutoRenter.Api.Models;

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

            var level = "warning";
            var message = "test message";
            var dataStructureConverterMoq = new Mock<IDataStructureConverter>();
            dataStructureConverterMoq.Setup(x => x.Map<LogEntry, LogEntryModel>(It.IsAny<LogEntryModel>()))
                .Returns(new LogEntry()
                {
                    Level = level,
                    Message = message
                });

            IErrorCodeConverter processor = new ErrorCodeConverter();
            var sut = new LogController(logService.Object, processor, dataStructureConverterMoq.Object);

            var logEntryModel = new LogEntryModel()
            {
                Level = level,
                Message = message
            };

            // act
            var result = await sut.Post(logEntryModel);
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

            IErrorCodeConverter processor = new ErrorCodeConverter();

            var level = "warning";
            var message = "test message";
            var dataStructureConverterMoq = new Mock<IDataStructureConverter>();
            dataStructureConverterMoq.Setup(x => x.Map<LogEntry, LogEntryModel>(It.IsAny<LogEntryModel>()))
                .Returns(new LogEntry()
                {
                    Level = level,
                    Message = message
                });

            var sut = new LogController(logService.Object, processor, dataStructureConverterMoq.Object);

            var logEntryModel = new LogEntryModel()
            {
                Level = level,
                Message = message
            };

            // act
            var result = await sut.Post(logEntryModel);
            var badRequestResult = result as BadRequestResult;

            // assert
            Assert.NotNull(badRequestResult);
        }

        [Fact]
        public async void Post_PassesMessageToService()
        {
            // arrange
            var logService = new Mock<ILogService>();
            logService.Setup(x => x.Log(It.IsAny<LogEntry>()))
                .ReturnsAsync(new Result<object>(ResultCode.Success));

            IErrorCodeConverter processor = new ErrorCodeConverter();

            var level = "warning";
            var message = "test message";
            var dataStructureConverterMoq = new Mock<IDataStructureConverter>();
            dataStructureConverterMoq.Setup(x => x.Map<LogEntry, LogEntryModel>(It.IsAny<LogEntryModel>()))
                .Returns(new LogEntry()
                {
                    Level = level,
                    Message = message
                });

            var sut = new LogController(logService.Object, processor, dataStructureConverterMoq.Object);

            var logEntryModel = new LogEntryModel()
            {
                Level = level,
                Message = message
            };

            // act
            var result = await sut.Post(logEntryModel);
            var createdResult = result as CreatedResult;

            // assert
            logService.Verify(x => x.Log(It.Is<LogEntry>(entry => entry.Message == message)));
        }

        [Fact]
        public async void Post_PassesLevelToService()
        {
            // arrange
            var logService = new Mock<ILogService>();
            logService.Setup(x => x.Log(It.IsAny<LogEntry>()))
                .ReturnsAsync(new Result<object>(ResultCode.Success));

            IErrorCodeConverter processor = new ErrorCodeConverter();

            var level = "info";
            var message = "test message";
            var dataStructureConverterMoq = new Mock<IDataStructureConverter>();
            dataStructureConverterMoq.Setup(x => x.Map<LogEntry, LogEntryModel>(It.IsAny<LogEntryModel>()))
                .Returns(new LogEntry()
                {
                    Level = level,
                    Message = message
                });

            var sut = new LogController(logService.Object, processor, dataStructureConverterMoq.Object);

            var logEntry = new LogEntryModel()
            {
                Level = level,
                Message = message
            };

            // act
            var result = await sut.Post(logEntry);
            var createdResult = result as CreatedResult;

            // assert
            logService.Verify(x => x.Log(It.Is<LogEntry>(entry => entry.Level == level)));
        }
    }
}
