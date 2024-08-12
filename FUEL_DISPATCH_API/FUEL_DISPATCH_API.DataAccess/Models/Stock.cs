using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
#nullable disable
namespace FUEL_DISPATCH_API.DataAccess.Models
{
    public class Stock
    {
        public int? Id { get; set; }
        [Required] public int? WareHouseId { get; set; }
        [Required] public int? ItemId { get; set; }
        [Required] public decimal? StockQty { get; set; }
        [JsonIgnore] public virtual WareHouse WareHouse { get; set; }

        [JsonIgnore] public virtual ArticleDataMaster ArticleDataMaster { get; set; }
    }
}
