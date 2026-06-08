namespace ECommerce.Application.DTOs.Products
{
    public class SearchProductsRequestDto
    {
        public string? SearchTerm { get; set; }
        public Guid? CategoryId { get; set; }
        public Guid? BrandId { get; set; }
        public string? Size { get; set; }
        public string? Color { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
