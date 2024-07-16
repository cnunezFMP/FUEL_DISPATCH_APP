using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#nullable disable
namespace FUEL_DISPATCH_API.DataAccess.Models
{
    // TODO: Hacer los servicios y controlador
    public class vw_WareHouseHistory
    {
        public int WareHouseId { get; set; }
        public string WareHouseCode { get; set; }
        public int ItemId { get; set; }
        public string MovementType { get; set; }
        public decimal ArtQuantity { get; set; }
        public string VehicleToken { get; set; }
        public string DispenserCode { get; set; }
        public string DriverName { get; set; }
        public DateTime MovementDate { get; set; }
        public DateTime RoadCode { get; set; }
    }
}
