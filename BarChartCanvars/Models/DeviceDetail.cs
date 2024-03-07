using System;
using System.Collections.Generic;

namespace BarChartCanvars.Models
{
    public partial class DeviceDetail
    {
        public DeviceDetail()
        {
            InterationData = new HashSet<InterationDatum>();
        }

        public int Id { get; set; }
        public string? Browser { get; set; }
        public bool BrowserCanJavaScript { get; set; }
        public string? UserAgent { get; set; }
        public DateTime SaveDateTime { get; set; }
        public bool BrowserHtml5VideoCanVideo { get; set; }
        public bool BrowserHtml5AudioCanAudio { get; set; }
        public bool CanTouchScreen { get; set; }
        public int? DeviceType { get; set; }
        public string? DeviceModelName { get; set; }
        public string? DeviceOperatingSystemModel { get; set; }
        public string? DeviceOperatingSystemVendor { get; set; }
        public string? DeviceVendor { get; set; }
        public int HardwareDisplayWidth { get; set; }
        public int HardwareDisplayHeight { get; set; }
        public bool DeviceIsSmartphone { get; set; }
        public bool IsBot { get; set; }

        public virtual ICollection<InterationDatum> InterationData { get; set; }
    }
}
