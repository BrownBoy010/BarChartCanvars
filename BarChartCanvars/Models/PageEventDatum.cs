using System;
using System.Collections.Generic;

namespace BarChartCanvars.Models
{
    public partial class PageEventDatum
    {
        public PageEventDatum()
        {
            InterationData = new HashSet<InterationDatum>();
        }

        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Text { get; set; }
        public string? Data { get; set; }
        public string? DataKey { get; set; }

        public virtual ICollection<InterationDatum> InterationData { get; set; }
    }
}
