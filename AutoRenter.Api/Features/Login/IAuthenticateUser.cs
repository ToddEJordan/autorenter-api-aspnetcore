using AutoRenter.Api.Features.User;

namespace AutoRenter.Api.Features.Login
{
    public interface IAuthenticateUser
    {
        ResultModel Execute(LoginModel loginModel);
    }
}
