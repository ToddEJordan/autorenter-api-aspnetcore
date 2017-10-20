using System;
using System.Linq;
using System.Threading.Tasks;
using AutoRenter.Domain.Data;
using AutoRenter.Domain.Interfaces;
using AutoRenter.Domain.Models;

namespace AutoRenter.Domain.Services
{
    public class UserService : IUserService, IDomainService, IDisposable
    {
        private bool disposed = false;
        private readonly AutoRenterContext context;

        public UserService(AutoRenterContext context)
        {
            this.context = context;
        }

        public async Task<Result<User>> GetUserByUsernameAndPassword(string username, string password)
        {
            var user = context.Users.FirstOrDefault(x => x.Username == username);
            if (user != null && BCrypt.Net.BCrypt.Verify(password, user.Password))
            {
                return await Task.FromResult(new Result<User>(ResultCode.Success, user));
            }
            return await Task.FromResult(new Result<User>(ResultCode.NotFound));
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                    disposed = true;
                }
            }
        }
    }
}