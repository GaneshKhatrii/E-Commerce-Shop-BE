namespace ECommerce.Application.DTOs.Admin
{
    public class DashboardStatsResponseDto
    {
        public int TotalUsers { get; set; }
        public int TotalProducts { get; set; }
        public int TotalOrders { get; set; }
        public decimal TotalRevenue { get; set; }
    }
}
