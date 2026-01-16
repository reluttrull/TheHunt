using FastEndpoints;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using TheHunt.Common.Constants;

namespace TheHunt.Places.Locations.Endpoints
{
    public class GetAll(ILocationService locationService) :
        Endpoint<GetAllLocationsRequest, IEnumerable<LocationResponse>>
    {
        private readonly ILocationService _locationService = locationService;

        public override void Configure()
        {
            Get("/locations");
            Policies(AuthConstants.FreeMemberUserPolicyName);
        }

        public override async Task HandleAsync(GetAllLocationsRequest req, CancellationToken ct)
        {
            var locations = await _locationService.GetAllLocationsAsync(req);

            await HttpContext.Response.SendAsync(locations, cancellation: ct);
        }
    }
}
