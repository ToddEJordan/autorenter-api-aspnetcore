using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using AutoRenter.Domain.Interfaces;
using AutoRenter.Domain.Models;

namespace AutoRenter.Domain.Services
{
    public class LogService : ILogService, IDomainService
    {
        private readonly ILogger<LogService> logger;
        private readonly IValidationService validationService;

        public LogService(ILogger<LogService> logger, IValidationService validationService)
        {
            this.logger = logger;
            this.validationService = validationService;
        }

        public async Task<Result<object>> Log(LogEntry logEntry)
        {
            if (! await validationService.IsValidInsert(logEntry))
            {
                return new Result<object>(ResultCode.BadRequest);
            }

            logger.LogInformation($"({logEntry.Level}): {logEntry.Message}");

            return new Result<object>(ResultCode.Success);
        }
    }
}
