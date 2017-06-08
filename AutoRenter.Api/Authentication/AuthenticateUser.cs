using System;
using AutoRenter.Api.Authorization;
using AutoRenter.Api.Models;

namespace AutoRenter.Api.Authentication
{
    public class AuthenticateUser : IAuthenticateUser
    {
        private readonly ITokenManager tokenManager;

        public AuthenticateUser(ITokenManager tokenManager)
        {
            this.tokenManager = tokenManager;
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
            if (loginModel.Password != "admin" &&
                loginModel.Password != "user") 
                return null;
            var userModel = new UserModel
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@fusionalliance.com",
                Username = loginModel.Username,
                IsAdministrator = loginModel.Password.ToUpper().IndexOf("ADMIN", StringComparison.Ordinal) > -1
            };
            userModel.BearerToken = tokenManager.CreateToken(userModel);
            return userModel;
        }
    }
}
