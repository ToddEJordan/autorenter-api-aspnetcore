using AutoRenter.Domain.Models;

namespace AutoRenter.Domain.Interfaces
{
    public interface ILogService
    {
        Result<object> Log(string message, string level);
    }
}
