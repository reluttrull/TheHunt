using FastEndpoints;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
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

            Throttle(hitLimit: 10, durationSeconds: 300);
        }

        public override async Task HandleAsync(LoginRequest req, CancellationToken ct)
        {
            var user = await _userService.GetUserByEmailAsync(req.Email);
            var isValidLogin = user is null ? false : await _userManager.CheckPasswordAsync(user!, req.Password);
            if (!isValidLogin)
            {
                AddError("Invalid email or password.");
                await HttpContext.Response.SendErrorsAsync(ValidationFailures, cancellation: ct);
                return;
            }

            string accessToken = await _tokenService.GenerateAccessTokenAsync(user!);
            RefreshTokenResponse refreshToken = await _tokenService.GenerateRefreshTokenAsync();
            var updated = await _tokenService.UpdateRefreshTokenAsync(user!.Id, refreshToken.RefreshToken, refreshToken.Expires);

            if (!updated)
            {
                await HttpContext.Response.SendNotFoundAsync(cancellation: ct);
                return;
            }

            _tokenService.SetAuthCookies(HttpContext, accessToken, refreshToken.RefreshToken);

            await HttpContext.Response.SendOkAsync(cancellation: ct);
        }
    }
}
