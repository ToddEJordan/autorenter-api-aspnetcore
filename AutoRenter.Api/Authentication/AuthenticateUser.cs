using System.Threading.Tasks;
using AutoRenter.Api.Authorization;
using AutoRenter.Api.Models;
using AutoRenter.Api.Services;
using AutoRenter.Domain.Interfaces;
using AutoRenter.Domain.Models;

namespace AutoRenter.Api.Authentication
{
    public class AuthenticateUser : IAuthenticateUser
    {
        private readonly ITokenManager tokenManager;
        private readonly IUserService userService;
        private readonly IDataStructureConverter dataStructureConverter;

        public AuthenticateUser(ITokenManager tokenManager, IUserService userService, IDataStructureConverter dataStructureConverter)
        {
            this.tokenManager = tokenManager;
            this.userService = userService;
            this.dataStructureConverter = dataStructureConverter;
        }

        public async Task<Result<UserModel>> Execute(LoginModel loginModel)
        {
            var userResult = await userService.GetUserByUsernameAndPassword(loginModel.Username, loginModel.Password);

            if (userResult.ResultCode != ResultCode.Success)
            {
                return new Result<UserModel>(ResultCode.Unauthorized);
            }

            var userModel = dataStructureConverter.Map<UserModel, User>(userResult.Data);
            userModel.Token = tokenManager.CreateToken(userModel);

            return new Result<UserModel>(ResultCode.Success, userModel);
        }
    }
}
