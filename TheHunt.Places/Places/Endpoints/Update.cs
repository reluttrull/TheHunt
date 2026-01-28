using FastEndpoints;
using System;
using System.Collections.Generic;
using System.Text;
using TheHunt.Common.Constants;

namespace TheHunt.Places.Places.Endpoints
{
    public class Update(IPlaceService placeService) :
        Endpoint<UpdatePlaceRequest, KnownPlaceResponse>
    {
        private readonly IPlaceService _placeService = placeService;

        public override void Configure()
        {
            Put("/places");
            Policies(AuthConstants.FreeMemberUserPolicyName);
        }

        public override async Task HandleAsync(UpdatePlaceRequest req, CancellationToken ct)
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

            var place = await _placeService.UpdatePlaceAsync(req.Id, req, ct);
            if (place is null)
            {
                await HttpContext.Response.SendNotFoundAsync(cancellation: ct);
                return;
            }
            await HttpContext.Response.SendOkAsync(place.MapToResponse(), cancellation: ct);
        }
    }
}
