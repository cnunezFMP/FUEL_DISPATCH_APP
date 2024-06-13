using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUEL_DISPATCH_API.DataAccess.Models
{
    public class Stock
    {
        public int Id { get; set; }
        public int WareHouseId { get; set; }
        public int ArticleId {  get; set; }
        public DateTime UpdatedAt { get; set; }
        public virtual WareHouse WareHouse { get; set; }    
        public virtual ArticleDataMaster ArticleDataMaster { get; set; }
    }
}
