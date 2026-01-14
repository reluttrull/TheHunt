using FastEndpoints;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using TheHunt.Places.Locations.Endpoints;
using TheHunt.Places.Locations;
using Microsoft.AspNetCore.Authentication.OAuth;
using TheHunt.Common.Constants;

namespace TheHunt.Places.Locations.Endpoints
{
    public class Create(ILocationService locationService) :
        Endpoint<CreateLocationRequest, LocationResponse>
    {
        private readonly ILocationService _locationService = locationService;

        public override void Configure()
        {
            Post("/locations");
            Policies(AuthConstants.FreeMemberUserPolicyName);
        }

        public override async Task HandleAsync(CreateLocationRequest req, CancellationToken ct)
        {
            var newLocation = req with { Id = Guid.NewGuid() };

            await _locationService.CreateLocationAsync(newLocation);

            await HttpContext.Response.SendCreatedAtAsync<GetById>(new { newLocation.Id }, newLocation);
        }
    }
}
