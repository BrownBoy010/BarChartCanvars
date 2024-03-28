namespace BarChartCanvars.Models
{
    public class UserLoginDetail
    {
        public int Id { get; set; }

        public string? UserId { get; set; }

        public DateTime Login { get; set; }

        public DateTime? Logout { get; set; }
    }
}
