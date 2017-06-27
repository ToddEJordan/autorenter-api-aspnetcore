using System.Collections.Generic;
using System.Linq;
using AutoRenter.Api.Models;

namespace AutoRenter.Api.Tests.Helpers
{
    internal static class UserModelHelper
    {
        internal static UserModel GetUser()
        {
            return User();
        }

        internal static UserModel GetAdministratorUser()
        {
            return AdministratorUser();
        }

        private static UserModel AdministratorUser(){
            return new UserModel
            {
                Username = "admin_jdoe",
                Email = "test@test.com",
                FirstName = "John",
                LastName = "Doe",
                IsAdministrator = true
            };
        }
        private static UserModel User()
        {
            return new UserModel
            {
                Username = "jdoe",
                Email = "test@test.com",
                FirstName = "John",
                LastName = "Doe",
                IsAdministrator = false
            };
        }
    }
}