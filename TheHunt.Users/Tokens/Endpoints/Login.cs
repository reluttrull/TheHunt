using FastEndpoints;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TheHunt.Common.Model;
using TheHunt.Users.Users;

namespace TheHunt.Users.Tokens.Endpoints
{
    public class Login(ITokenService tokenService, IUserService userService,
        UserManager<User> userManager) :
        Endpoint<LoginRequest, TokenResponse>
    {
        private readonly ITokenService _tokenService = tokenService;
        private readonly IUserService _userService = userService;
        private readonly UserManager<User> _userManager = userManager;

        public override void Configure()
        {
            Post("/login");
            AllowAnonymous();
        }

        public override async Task HandleAsync(LoginRequest req, CancellationToken ct)
        {
            var user = await _userService.GetUserByEmailAsync(req.Email);

            // todo: send more useful errors
            if (user is null)
            {
                await HttpContext.Response.SendUnauthorizedAsync(cancellation: ct);
            }

            var passwordValid = await _userManager.CheckPasswordAsync(user!, req.Password);
            if (!passwordValid)
                await HttpContext.Response.SendUnauthorizedAsync(cancellation: ct);

            string accessToken = await _tokenService.GenerateAccessTokenAsync(user!);
            RefreshTokenResponse refreshToken = await _tokenService.GenerateRefreshTokenAsync();
            var updated = await _tokenService.UpdateRefreshTokenAsync(user!.Id, refreshToken.RefreshToken, refreshToken.Expires);

            await HttpContext.Response.SendOkAsync(new TokenResponse(accessToken, refreshToken.RefreshToken), cancellation: ct);
        }
    }
}
