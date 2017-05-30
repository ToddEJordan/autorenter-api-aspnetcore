using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using AutoRenter.Domain.Interfaces;
using AutoRenter.Domain.Models;

namespace AutoRenter.Domain.Services
{
    public class LogService : ILogService, IDomainService
    {
        private readonly ILogger logger;
        private readonly IValidationService validationService;

        public LogService(ILoggerFactory loggerFactory, IValidationService validationService)
        {
            this.validationService = validationService;
            logger = loggerFactory.CreateLogger("AutoRenter");
        }

        public async Task<Result<object>> Log(LogEntry logEntry)
        {
            if (! await validationService.IsValidInsert(logEntry))
            {
                return new Result<object>(ResultCode.BadRequest);
            }

            switch (logEntry.Level.ToLower())
            {
                case "info":
                case "information":
                    logger.LogInformation(logEntry.Message);
                    break;
                case "debug":
                    logger.LogDebug(logEntry.Message);
                    break;
                case "warn":
                case "warning":
                    logger.LogWarning(logEntry.Message);
                    break;
                case "error":
                    logger.LogError(logEntry.Message);
                    break;
                default:
                    return new Result<object>(ResultCode.BadRequest);
            }

            return new Result<object>(ResultCode.Success);
        }
    }
}
