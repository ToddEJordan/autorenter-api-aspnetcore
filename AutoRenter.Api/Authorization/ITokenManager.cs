using AutoRenter.Api.Features.User;

namespace AutoRenter.Api.Authorization
{
    public interface ITokenManager
    {
        string CreateToken(UserModel userModel);
    }
}
