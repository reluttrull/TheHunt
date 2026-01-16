using FastEndpoints;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using TheHunt.Common.Constants;

namespace TheHunt.Places.Places.Endpoints
{
    public class Create(IPlaceService placeService) :
        Endpoint<CreatePlaceRequest, PlaceResponse>
    {
        private readonly IPlaceService _placeService = placeService;

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

            var createdPlaceResponse = await _placeService.GetPlaceByIdAsync(newPlace.Id!.Value);

            if (createdPlaceResponse is null)
            {
                await HttpContext.Response.SendNotFoundAsync(cancellation: ct);
                return;
            }

            await HttpContext.Response.SendCreatedAtAsync<GetById>(new { createdPlaceResponse.Id }, createdPlaceResponse, cancellation: ct);
        }
    }
}
