using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#nullable disable
namespace FUEL_DISPATCH_API.DataAccess.Models
{
    public class vw_ActualStock
    {
        public int WareHouseId { get; set; }
        public string WareHouseName { get; set; }
        public string WareHouseCode { get; set; }
        public int ItemId { get; set; }
        public string ArticleDescription { get; set; }
        public string ArticleCode { get; set; }
    }
}
