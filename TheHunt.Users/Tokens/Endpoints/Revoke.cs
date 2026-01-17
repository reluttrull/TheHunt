using FastEndpoints;
using Microsoft.AspNetCore.Identity.Data;
using System;
using System.Collections.Generic;
using System.Text;
using TheHunt.Users.Users;

namespace TheHunt.Users.Tokens.Endpoints
{
    public class Revoke(ITokenService tokenService, IUserService userService) : 
        Endpoint<RevokeTokenRequest>
    {
        private readonly ITokenService _tokenService = tokenService;
        private readonly IUserService _userService = userService;

        public override void Configure()
        {
            Post("/revoke");
            AllowAnonymous();
        }

        public override async Task HandleAsync(RevokeTokenRequest req, CancellationToken ct)
        {
            var user = await _userService.GetUserByRefreshTokenAsync(req.RefreshToken);

            if (user is null)
            {
                await HttpContext.Response.SendUnauthorizedAsync(cancellation: ct);
                return;
            }
            var success = await _tokenService.RevokeTokenAsync(req);
            if (!success)
            {
                await HttpContext.Response.SendNotFoundAsync(cancellation: ct);
                return;
            }
            await HttpContext.Response.SendOkAsync(cancellation: ct);

        }
    }
}
