using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FUEL_DISPATCH_API.DataAccess.Models.SAP
{
    public class WarehouseItemStock
    {
        [JsonPropertyName("odata.metadata")]
        public string odatametadata { get; set; } = "";

        [JsonPropertyName("value")]
        public List<WarehouseItemStockValue> Value { get; set; } = [];
    }


    public class WarehouseItemStockValue
    {
        [JsonPropertyName("Items")]
        public Items? Items { get; set; }

        [JsonPropertyName("Items/ItemWarehouseInfoCollection")]
        public ItemsItemwarehouseinfocollection? StockInfo { get; set; }
    }

    public class Items
    {
        public string? ItemCode { get; set; }
        public string? ItemName { get; set; }
    }

    public class ItemsItemwarehouseinfocollection
    {
        public decimal InStock { get; set; }
    }

}
