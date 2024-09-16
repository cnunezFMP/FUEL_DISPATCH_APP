using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUEL_DISPATCH_API.DataAccess.Models.SAP
{
    public class SAPGenExit
    {
        public DateTime DocDate { get; set; }
        public string? Comments { get; set; }
        public List<SAPGenExitLine> DocumentLines { get; set; } = [];

    }

    public class SAPGenExitLine
    {
        public required string ItemCode { get; set; }
        public required decimal UnitPrice { get; set; }
        public required string WarehouseCode { get; set; }
        public required decimal Quantity { get; set; }
    }

}
