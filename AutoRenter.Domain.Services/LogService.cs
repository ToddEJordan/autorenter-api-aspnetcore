using Microsoft.Extensions.Logging;
using AutoRenter.Domain.Interfaces;
using AutoRenter.Domain.Models;

namespace AutoRenter.Domain.Services
{
    public class LogService : ILogService, IDomainService
    {
        private readonly ILogger<LogService> logger;

        public LogService(ILogger<LogService> logger)
        {
            this.logger = logger;
        }

        public Result<object> Log(string message, string level)
        {
            logger.LogInformation($"({level}): {message}");

            return new Result<object>(ResultCode.Success);
        }
    }
}
