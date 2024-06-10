using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUEL_DISPATCH_API.DataAccess.Models
{
    public class StoreMovement
    {
        public int Id { get; set; }
        public string? Type { get; set; }
        public decimal? Qty { get; set; }
        public int? StoreId { get; set; }
        public virtual Store Store { get; set; }
    }
}
