using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace TheHunt.Common.Extensions
{
    public static class IdentityExtensions
    {
        public static Guid? GetUserId(this HttpContext context)
        {
            var userId = context.User.Claims.SingleOrDefault(x => x.Type == "userid");

            if (Guid.TryParse(userId?.Value, out var parsedId))
            {
                return parsedId;
            }

            return null;
        }
    }
}
