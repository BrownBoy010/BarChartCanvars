using System;
using System.Collections.Generic;

namespace BarChartCanvars.Models
{
    public partial class WhoIsInformation
    {
        public WhoIsInformation()
        {
            InterationData = new HashSet<InterationDatum>();
        }

        public int Id { get; set; }
        public string? Ip { get; set; }
        public DateTime SaveDateTime { get; set; }
        public string? Country { get; set; }
        public string? Region { get; set; }
        public string? City { get; set; }
        public string? PostalCode { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public string? MetroCode { get; set; }
        public string? BusinessName { get; set; }
        public string? Isp { get; set; }
        public string? Dns { get; set; }

        public virtual ICollection<InterationDatum> InterationData { get; set; }
    }
}
