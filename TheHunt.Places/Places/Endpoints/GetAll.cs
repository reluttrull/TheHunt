using FastEndpoints;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using TheHunt.Common.Constants;

namespace TheHunt.Places.Places.Endpoints
{
    public class GetAll(IPlaceService placeService) :
        Endpoint<GetAllPlacesRequest, IEnumerable<UnknownPlaceResponse>>
    {
        private readonly IPlaceService _placeService = placeService;

        public override void Configure()
        {
            Get("/places");
            Policies(AuthConstants.FreeMemberUserPolicyName);
        }

        public override async Task HandleAsync(GetAllPlacesRequest req, CancellationToken ct)
        {
            var places = await _placeService.GetAllPlacesAsync(req);

            await HttpContext.Response.SendAsync(places.Select(p => p.MapToUnknownResponse()), cancellation: ct);
        }
    }
}
