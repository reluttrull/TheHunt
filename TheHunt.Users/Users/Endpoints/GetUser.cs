using FastEndpoints;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using TheHunt.Common.Constants;

namespace TheHunt.Users.Users.Endpoints
{
    public class GetUser(IUserService userService) :
        EndpointWithoutRequest<UserResponse>
    {
        private readonly IUserService _userService = userService;

        public override void Configure()
        {
            Get("/users/me");
            Policies(AuthConstants.FreeMemberUserPolicyName);
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            var userIdClaim = User.FindFirst("userid")?.Value;

            if (!Guid.TryParse(userIdClaim, out var userId))
            {
                await HttpContext.Response.SendUnauthorizedAsync(cancellation: ct);
                return;
            }
            var user = await _userService.GetUserByIdAsync(userId);

            if (user is null)
            {
                await HttpContext.Response.SendNotFoundAsync(cancellation: ct);
                return;
            }

            await HttpContext.Response.SendAsync(user, cancellation: ct);
        }
    }
}
