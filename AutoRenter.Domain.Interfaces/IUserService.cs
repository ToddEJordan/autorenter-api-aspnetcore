using System.Threading.Tasks;
using AutoRenter.Domain.Models;

namespace AutoRenter.Domain.Interfaces
{
    public interface IUserService
    {
        Task<Result<User>> GetUserByUsernameAndPassword(string username, string password);
    }
}
