using FastEndpoints;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace TheHunt.Users.Users.Endpoints
{
    public class GetById(IUserService userService) :
        Endpoint<GetUserByIdRequest, UserResponse>
    {
        private readonly IUserService _userService = userService;

        public override void Configure()
        {
            Get("/users/{Id}");
            AllowAnonymous();
        }

        public override async Task HandleAsync(GetUserByIdRequest req, CancellationToken ct)
        {
            var user = await _userService.GetUserByIdAsync(req.Id);

            if (user is null)
            {
                await HttpContext.Response.SendNotFoundAsync(cancellation: ct);
                return;
            }

            await HttpContext.Response.SendAsync(user, cancellation: ct);
        }
    }
}
