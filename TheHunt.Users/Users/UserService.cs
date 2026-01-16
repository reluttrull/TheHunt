using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TheHunt.Common.Data;
using TheHunt.Common.Model;

namespace TheHunt.Users.Users
{
    public class UserService : IUserService
    {
        private readonly GameContext _gameContext;

        public UserService(GameContext gameContext)
        {
            _gameContext = gameContext;
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            var user = await _gameContext.Users.FirstOrDefaultAsync<User>(u => u.Email == email);
            return user;
        }

        public async Task<User?> GetUserByIdAsync(Guid id)
        {
            var user = await _gameContext.Users.FindAsync(id);
            return user;
        }

        public async Task<User?> GetUserByRefreshTokenAsync(string refreshToken)
        {
            var user = await _gameContext.Users.FirstOrDefaultAsync<User>(u => u.RefreshToken == refreshToken);
            return user;
        }

        public async Task<User?> GetUserByUserNameAsync(string userName)
        {
            var user = await _gameContext.Users.FirstOrDefaultAsync<User>(u => u.UserName == userName);
            return user;
        }
    }
    public interface IUserService
    {
        Task<User?> GetUserByEmailAsync(string email);
        Task<User?> GetUserByUserNameAsync(string userName);
        Task<User?> GetUserByRefreshTokenAsync(string refreshToken);
        Task<User?> GetUserByIdAsync(Guid id);
    }
}
