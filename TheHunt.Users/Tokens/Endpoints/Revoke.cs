using FastEndpoints;
using Microsoft.AspNetCore.Identity.Data;
using System;
using System.Collections.Generic;
using System.Text;
using TheHunt.Users.Users;

namespace TheHunt.Users.Tokens.Endpoints
{
    public class Revoke(ITokenService tokenService, IUserService userService) : 
        EndpointWithoutRequest
    {
        private readonly ITokenService _tokenService = tokenService;
        private readonly IUserService _userService = userService;

        public override void Configure()
        {
            Post("/revoke");
            AllowAnonymous();
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            var refreshToken = HttpContext.Request.Cookies["refresh_token"];
            if (refreshToken is null)
            {
                await HttpContext.Response.SendUnauthorizedAsync(ct);
                return;
            }
            var user = await _userService.GetUserByRefreshTokenAsync(refreshToken);
            if (user is null)
            {
                await HttpContext.Response.SendUnauthorizedAsync(cancellation: ct);
                return;
            }

            _tokenService.ClearAuthCookies(HttpContext);
            await HttpContext.Response.SendOkAsync(ct);
        }
    }
}
