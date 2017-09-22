using System.Threading.Tasks;
using AutoRenter.Api.Models;
using AutoRenter.Domain.Models;

namespace AutoRenter.Api.Authentication
{
    public interface IAuthenticateUser
    {
        Task<Result<UserModel>> Execute(LoginModel loginModel);
    }
}
