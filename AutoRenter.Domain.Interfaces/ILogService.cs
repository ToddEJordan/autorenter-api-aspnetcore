using System.Threading.Tasks;
using AutoRenter.Domain.Models;

namespace AutoRenter.Domain.Interfaces
{
    public interface ILogService
    {
        Task<Result<object>> Log(LogEntry logEntry);
    }
}
