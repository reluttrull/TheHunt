using FastEndpoints;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using TheHunt.Places.Locations.Endpoints;
using TheHunt.Places.Locations;
using Microsoft.AspNetCore.Authentication.OAuth;
using TheHunt.Common.Constants;

namespace TheHunt.Places.Locations.Endpoints
{
    public class Delete(ILocationService locationService) :
        Endpoint<DeleteLocationRequest, LocationResponse>
    {
        private readonly ILocationService _locationService = locationService;

        public override void Configure()
        {
            Post("/locations");
            Policies(AuthConstants.FreeMemberUserPolicyName);
        }

        public override async Task HandleAsync(DeleteLocationRequest req, CancellationToken ct)
        {
            var userIdClaim = User.FindFirst("userid")?.Value;

            if (!Guid.TryParse(userIdClaim, out var userId))
            {
                await HttpContext.Response.SendUnauthorizedAsync(cancellation: ct);
                return;
            }

            var locationToDelete = await _locationService.GetLocationByIdAsync(req.Id);
            if (locationToDelete is null) return;
            if (locationToDelete.RecordedByUser != userId)
            {
                await HttpContext.Response.SendUnauthorizedAsync(cancellation: ct);
                return;
            }

            var success = await _locationService.DeleteLocationAsync(req.Id, ct);

            if (!success)
            {
                await HttpContext.Response.SendNotFoundAsync(cancellation: ct);
                return;
            }
            await HttpContext.Response.SendOkAsync(cancellation: ct);
        }
    }
}
