namespace FUEL_DISPATCH_API.DataAccess.Models
{
    public class ArticleDataMaster
    {
        public int Id { get; set; }
        public string? ArticleNumber { get; set; }
        public string? Description { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
