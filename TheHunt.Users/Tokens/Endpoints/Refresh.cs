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
    public class Refresh(ITokenService tokenService, IUserService userService) :
        Endpoint<RefreshRequest, RefreshTokenResponse>
    {
        private readonly ITokenService _tokenService = tokenService;
        private readonly IUserService _userService = userService;

        public override void Configure()
        {
            Post("/refresh");
            AllowAnonymous();
        }

        public override async Task HandleAsync(RefreshRequest req, CancellationToken ct)
        {
            var user = await _userService.GetUserByRefreshTokenAsync(req.RefreshToken);

            if (user is null)
            {
                // todo: pass back specific error "Invalid refresh token" or "Expired refresh token"
                await HttpContext.Response.SendUnauthorizedAsync(ct);
                return;
            }
            if (user.RefreshTokenExpiry < DateTime.UtcNow)
            {
                await HttpContext.Response.SendUnauthorizedAsync(ct);
                return;
            }
            var newAccessToken = await _tokenService.GenerateAccessTokenAsync(user);
            var newRefreshToken = await _tokenService.GenerateRefreshTokenAsync();

            var updated = await _tokenService.UpdateRefreshTokenAsync(user.Id, newRefreshToken.RefreshToken, newRefreshToken.Expires);


            await HttpContext.Response.SendOkAsync(newRefreshToken, cancellation: ct);
        }
    }
}
