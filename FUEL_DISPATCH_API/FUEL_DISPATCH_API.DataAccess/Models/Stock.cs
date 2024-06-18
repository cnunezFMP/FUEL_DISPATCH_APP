using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
#nullable disable
namespace FUEL_DISPATCH_API.DataAccess.Models
{
    public class Stock
    {
        public int Id { get; set; }
        public int WareHouseId { get; set; }
        public int ItemId { get; set; }
        public decimal StockQty { get; set; }
        [JsonIgnore]
        public virtual WareHouse WareHouse { get; set; }
        [JsonIgnore]
        public virtual ArticleDataMaster ArticleDataMaster { get; set; }
    }
}
