using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FUEL_DISPATCH_API.DataAccess.Models
{
    // DONE: Poner CompanyId
    public class ArticleDataMaster
    {
        public int Id { get; set; }
        [Required] public string ArticleNumber { get; set; } // Code
        public string? Description { get; set; }
        [Required] public decimal UnitPrice { get; set; }
        public string Manufacturer { get; set; }
        [Required] public string? BarCode { get; set; }
        public string? CreatedBy { get; set; }
        [Required] public int CompanyId { get; set; }
        public DateTime? CreatedAt { get; set; } = DateTime.Now;
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; } = DateTime.Now;
        [JsonIgnore] public virtual ICollection<Stock> Stocks { get; set; } = new List<Stock>();
        [JsonIgnore] public virtual ICollection<WareHouseMovement> WareHouseMovements { get; set; } = new List<WareHouseMovement>();
        [JsonIgnore] public virtual Companies? Company { get; set; }
    }
}
