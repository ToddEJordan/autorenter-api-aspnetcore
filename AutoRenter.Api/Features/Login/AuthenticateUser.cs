using System;
using AutoRenter.Api.Authorization;
using AutoRenter.Api.Features.User;

namespace AutoRenter.Api.Features.Login
{
    public class AuthenticateUser : IAuthenticateUser
    {
        private readonly ITokenManager _tokenManager;

        public AuthenticateUser(ITokenManager tokenManager)
        {
            _tokenManager = tokenManager;
        }

        public ResultModel Execute(LoginModel loginModel)
        {
            var userModel = LookupUser(loginModel);

            return new ResultModel
            {
                Data = userModel,
                Success = userModel != null,
                Message = userModel == null ? "Login failed.  Please try again." : null
            };
        }

        public virtual UserModel LookupUser(LoginModel loginModel)
        {
            if (loginModel.Password != "secret") return null;
            var userModel = new UserModel
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@fusionalliance.com",
                Username = loginModel.Username,
                IsAdministrator = loginModel.Username.ToUpper().IndexOf("ADMIN", StringComparison.Ordinal) > -1
            };
            userModel.BearerToken = _tokenManager.CreateToken(userModel);
            return userModel;
        }
    }
}
