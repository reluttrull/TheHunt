using FastEndpoints;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using TheHunt.Places.Locations.Endpoints;
using TheHunt.Places.Locations;
using Microsoft.AspNetCore.Authentication.OAuth;
using TheHunt.Common.Constants;

namespace TheHunt.Places.Places.Endpoints
{
    public class Delete(IPlaceService placeService) :
        Endpoint<DeletePlaceRequest, PlaceResponse>
    {
        private readonly IPlaceService _placeService = placeService;

        public override void Configure()
        {
            Delete("/places");
            Policies(AuthConstants.FreeMemberUserPolicyName);
        }

        public override async Task HandleAsync(DeletePlaceRequest req, CancellationToken ct)
        {
            var userIdClaim = User.FindFirst("userid")?.Value;

            if (!Guid.TryParse(userIdClaim, out var userId))
            {
                await HttpContext.Response.SendUnauthorizedAsync(cancellation: ct);
                return;
            }

            var placeToDelete = await _placeService.GetPlaceByIdAsync(req.Id);
            if (placeToDelete is null) return;
            if (placeToDelete.AddedByUserId != userId)
            {
                await HttpContext.Response.SendUnauthorizedAsync(cancellation: ct);
                return;
            }

            var success = await _placeService.DeletePlaceAsync(req.Id, ct);

            if (!success)
            {
                await HttpContext.Response.SendNotFoundAsync(cancellation: ct);
                return;
            }
            await HttpContext.Response.SendOkAsync(cancellation: ct);
        }
    }
}
