using System;
using System.Collections.Generic;
using System.Text;
using TheHunt.Common.Model;

namespace TheHunt.Users.Users
{
    public class UserService : IUserService
    {
        public Task<User?> GetUserByEmailAsync(string email)
        {
            throw new NotImplementedException();
        }

        public Task<User?> GetUserByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<User?> GetUserByRefreshTokenAsync(string refreshToken)
        {
            throw new NotImplementedException();
        }
    }
    public interface IUserService
    {
        Task<User?> GetUserByEmailAsync(string email);
        Task<User?> GetUserByRefreshTokenAsync(string refreshToken);
        Task<User?> GetUserByIdAsync(Guid id);
    }
}
