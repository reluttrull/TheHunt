using FastEndpoints;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using TheHunt.Common.Constants;
using TheHunt.Places.Locations;

namespace TheHunt.Places.Places.Endpoints
{
    public class Create(IPlaceService placeService, ILocationService locationService) :
        Endpoint<CreatePlaceRequest, KnownPlaceResponse>
    {
        private readonly IPlaceService _placeService = placeService;
        private readonly ILocationService _locationService = locationService;

        public override void Configure()
        {
            Post("/places");
            Policies(AuthConstants.FreeMemberUserPolicyName);
        }

        public override async Task HandleAsync(CreatePlaceRequest req, CancellationToken ct)
        {
            var userIdClaim = User.FindFirst("userid")?.Value;

            if (!Guid.TryParse(userIdClaim, out var userId))
            {
                await HttpContext.Response.SendUnauthorizedAsync(cancellation: ct);
                return;
            }
            var newPlace = req with { Id = Guid.NewGuid(), AddedByUserId = userId };

            await _placeService.CreatePlaceAsync(newPlace);

            var createdPlace = await _placeService.GetPlaceByIdAsync(newPlace.Id!.Value);

            if (createdPlace is null)
            {
                await HttpContext.Response.SendNotFoundAsync(cancellation: ct);
                return;
            }

            await HttpContext.Response.SendCreatedAtAsync<GetById>(new { createdPlace.Id }, createdPlace.MapToResponse(), cancellation: ct);
        }
    }
}
