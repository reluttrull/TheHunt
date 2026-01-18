using FastEndpoints;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using TheHunt.Common.Constants;
using TheHunt.Places.Places;
using TheHunt.Places.Places.Endpoints;

namespace TheHunt.Places.PLaces.Endpoints
{
    public class GetAllForUser(IPlaceService placeService) :
        EndpointWithoutRequest<IEnumerable<PlaceResponse>>
    {
        private readonly IPlaceService _placeService = placeService;

        public override void Configure()
        {
            Get("/places/me");
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
            var places = await _placeService.GetAllPlacesForUserAsync(userId, ct);

            await HttpContext.Response.SendAsync(places.Select(p => p.MapToResponse()), cancellation: ct);
        }
    }
}
