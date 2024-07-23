namespace FUEL_DISPATCH_API.DataAccess.Models
{
    // DONE: Hacer los servicios y controlador
    public class vw_WareHouseHistory
    {
        public int? WareHouse { get; set; }
        public int? ToWareHouse { get; set; }
        public string? WareHouseCode { get; set; }
        public string? ToWareHouseCode { get; set; }
        public int? ItemId { get; set; }
        public string? MovementType { get; set; }
        public decimal? ArtQuantity { get; set; }
        public decimal? Odometer { get; set; }
        public string? VehicleToken { get; set; }
        public string? DispenserCode { get; set; }
        public string? DriverName { get; set; }
        public DateTime? MovementDate { get; set; }
        public string? RoadCode { get; set; }
    }
}
