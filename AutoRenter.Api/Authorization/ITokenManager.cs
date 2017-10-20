using AutoRenter.Api.Models;

namespace AutoRenter.Api.Authorization
{
    public interface ITokenManager
    {
        string CreateToken(UserModel userModel);

        bool IsTokenValid(string token);
    }
}
