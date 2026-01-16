using System;
using System.Collections.Generic;
using System.Text;

namespace TheHunt.Common.Model
{
    public class Location
    {
        public Guid Id { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public DateTime RecordedDate { get; set; }
        public Guid RecordedByUser { get; set; }
    }
}
