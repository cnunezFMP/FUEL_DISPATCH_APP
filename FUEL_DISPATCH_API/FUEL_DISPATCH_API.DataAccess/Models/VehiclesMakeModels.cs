namespace FUEL_DISPATCH_API.DataAccess.Models
{
    public class VehiclesMakeModels
    {
        public int Id { get; set; }
        public string? Plate { get; set; }
        public decimal? Odometer { get; set; }
        public string? Ficha { get; set; }
        public string? MakeName { get; set; }
        public string? ModelName { get; set; }
    }
}
