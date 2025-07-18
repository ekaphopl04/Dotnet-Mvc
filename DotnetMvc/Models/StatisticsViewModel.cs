namespace DotnetMvc.Models
{
    public class StatisticsViewModel
    {
        public int TotalComics { get; set; }
        public int AvailableComics { get; set; }
        public decimal TotalValue { get; set; }
        public decimal AveragePrice { get; set; }
        public decimal MostExpensive { get; set; }
        public decimal Cheapest { get; set; }
        public List<CategoryStatistic> CategoryStats { get; set; } = new List<CategoryStatistic>();
        public List<RecentComic> RecentComics { get; set; } = new List<RecentComic>();
    }

    public class CategoryStatistic
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public int ComicCount { get; set; }
        public decimal AveragePrice { get; set; }
    }

    public class RecentComic
    {
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
    }
}
