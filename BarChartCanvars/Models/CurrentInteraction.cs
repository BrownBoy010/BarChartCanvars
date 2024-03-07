using System;
using System.Collections.Generic;

namespace BarChartCanvars.Models
{
    public partial class CurrentInteraction
    {
        public long Id { get; set; }
        public string? SessionId { get; set; }
        public string? ContactId { get; set; }
        public string? CustomerId { get; set; }
        public string? TransactionId { get; set; }
        public string? Ip { get; set; }
        public string PageUrl { get; set; } = null!;
        public string? Referrer { get; set; }
        public string? UserId { get; set; }
        public DateTime? SaveDateTime { get; set; }
        public string? SiteName { get; set; }
        public string? UserAgent { get; set; }
    }
}
