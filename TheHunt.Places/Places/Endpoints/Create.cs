using FastEndpoints;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
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
            var newPlace = req with { Id = Guid.NewGuid() };

            await _placeService.CreatePlaceAsync(newPlace);

            await HttpContext.Response.SendCreatedAtAsync<GetById>(new { newPlace.Id }, newPlace);
        }
    }
}
