using FastEndpoints;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using TheHunt.Common.Constants;
using TheHunt.Common.Data;
using TheHunt.Common.Model;
using TheHunt.Users.Tokens.Endpoints;
using TheHunt.Users.Users;

namespace TheHunt.Users.Tokens
{
    public class TokenService : ITokenService
    {
        private readonly GameContext _gameContext;
        private readonly UserManager<User> _userManager;
        private string tokenSecret = string.Empty;
        private static readonly TimeSpan tokenLifetime = TimeSpan.FromMinutes(2); // todo: make longer after testing
        private readonly IConfiguration _config;
        public TokenService(GameContext gameContext, UserManager<User> userManager, IConfiguration config)
        {
            _gameContext = gameContext;
            _userManager = userManager;
            _config = config;
            tokenSecret = Environment.GetEnvironmentVariable("TOKEN_SECRET") ?? config.GetValue<string>("TOKEN_SECRET")!;
        }

        public async Task<string> GenerateAccessTokenAsync(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(tokenSecret);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName!),
                new("userid", user.Id.ToString())
            };

            // for now, everyone is a free member
            claims.Add(new Claim(AuthConstants.FreeMemberUserClaimName, "true"));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.Add(tokenLifetime),
                Issuer = Environment.GetEnvironmentVariable("JWT_ISSUER") ?? _config.GetValue<string>("JWT_ISSUER"),
                Audience = Environment.GetEnvironmentVariable("JWT_AUDIENCE") ?? _config.GetValue<string>("JWT_AUDIENCE"),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwt = tokenHandler.WriteToken(token);
            return jwt;
        }

        public async Task<RefreshTokenResponse> GenerateRefreshTokenAsync()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return new RefreshTokenResponse(Convert.ToBase64String(randomNumber), DateTime.UtcNow.AddDays(14));
        }

        public async Task<bool> RevokeTokenAsync(RevokeTokenRequest request)
        {
            var user = await _gameContext.Users.FirstOrDefaultAsync(u => u.RefreshToken == request.RefreshToken);
            if (user is null) return false;

            user.RefreshToken = string.Empty;
            user.RefreshTokenExpiry = new DateTime();
            await _gameContext.SaveChangesAsync();

            var result = await _userManager.RemoveAuthenticationTokenAsync(user, "Default", "RefreshToken");
            return result.Succeeded;
        }

        public async Task<bool> UpdateRefreshTokenAsync(Guid userId, string refreshToken, DateTime expires)
        {
            var user = await _gameContext.Users.FindAsync(userId);
            if (user is null) return false;
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiry = expires;
            var result = await _gameContext.SaveChangesAsync();
            return result > 0;
        }
    }
    public interface ITokenService
    {
        Task<string> GenerateAccessTokenAsync(User user);
        Task<RefreshTokenResponse> GenerateRefreshTokenAsync();
        Task<bool> UpdateRefreshTokenAsync(Guid userId, string refreshToken, DateTime expires);
        Task<bool> RevokeTokenAsync(RevokeTokenRequest request);
    }
}
