using FastEndpoints;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using TheHunt.Common.Constants;

namespace TheHunt.Places.Locations.Endpoints
{
    public class GetAllForUser(ILocationService locationService) :
        EndpointWithoutRequest<IEnumerable<LocationResponse>>
    {
        private readonly ILocationService _locationService = locationService;

        public override void Configure()
        {
            Get("/locations/me");
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
            var locations = await _locationService.GetAllLocationsForUserAsync(userId);

            await HttpContext.Response.SendAsync(locations, cancellation: ct);
        }
    }
}
