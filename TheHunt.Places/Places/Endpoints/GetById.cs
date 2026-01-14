using FastEndpoints;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using TheHunt.Common.Constants;

namespace TheHunt.Places.Places.Endpoints
{
    public class GetById(IPlaceService placeService) :
        Endpoint<GetPlaceByIdRequest, PlaceResponse>
    {
        private readonly IPlaceService _placeService = placeService;

        public override void Configure()
        {
            Get("/places/{Id}");
            Policies(AuthConstants.FreeMemberUserPolicyName);
        }

        public override async Task HandleAsync(GetPlaceByIdRequest req, CancellationToken ct)
        {
            var place = await _placeService.GetPlaceByIdAsync(req.Id);

            if (place is null)
            {
                await HttpContext.Response.SendNotFoundAsync();
                return;
            }

            await HttpContext.Response.SendAsync(place);
        }
    }
}
