using System;
using System.Collections.Generic;
using System.Text;

namespace TheHunt.Common.Model
{
    public class UserBadge
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid BadgeId { get; set; }
        public DateTime AwardedDate { get; set; }
    }
}
