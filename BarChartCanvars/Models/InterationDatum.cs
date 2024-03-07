using System;
using System.Collections.Generic;

namespace BarChartCanvars.Models
{
    public partial class InterationDatum
    {
        public int Id { get; set; }
        public int? DeviceInfoId { get; set; }
        public int? GeoIpid { get; set; }
        public int? PageEventId { get; set; }

        public virtual DeviceDetail? DeviceInfo { get; set; }
        public virtual WhoIsInformation? GeoIp { get; set; }
        public virtual PageEventDatum? PageEvent { get; set; }
    }
}
