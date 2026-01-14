using FastEndpoints;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using TheHunt.Common.Constants;

namespace TheHunt.Places.Locations.Endpoints
{
    public class GetById(ILocationService locationService) :
        Endpoint<GetLocationByIdRequest, LocationResponse>
    {
        private readonly ILocationService _locationService = locationService;

        public override void Configure()
        {
            Get("/locations/{Id}");
            Policies(AuthConstants.FreeMemberUserPolicyName);
        }

        public override async Task HandleAsync(GetLocationByIdRequest req, CancellationToken ct)
        {
            var location = await _locationService.GetLocationByIdAsync(req.Id);

            if (location is null)
            {
                await HttpContext.Response.SendNotFoundAsync();
                return;
            }

            await HttpContext.Response.SendAsync(location);
        }
    }
}
