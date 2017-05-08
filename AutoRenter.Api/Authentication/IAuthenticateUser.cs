using AutoRenter.Api.Models;

namespace AutoRenter.Api.Authentication
{
    public interface IAuthenticateUser
    {
        ResultModel Execute(LoginModel loginModel);
    }
}
