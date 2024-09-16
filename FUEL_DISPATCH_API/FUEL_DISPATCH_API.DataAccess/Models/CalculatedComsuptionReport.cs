namespace FUEL_DISPATCH_API.DataAccess.Models;
public class CalculatedComsuptionReport
{
    public string? MakeName { get; set; }
    public string? ModelName { get; set; }
    public string? DriverFullName { get; set; }
    public decimal? Odometer { get; set; }
    public decimal? Qty { get; set; }
    public string? ArticleDescription { get; set; }
    public string? Measurename { get; set; }
    public int? MedidaDelVehiculo { get; set; }
    public decimal? AverageConsumption { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime? CreatedAt { get; set; }
    public string? Dispatcher { get; set; }
    public string? VehiclePlate { get; set; }
    public int Week { get; set; }
    public int Hour { get; set; }
    public int Month { get; set; }
    public int Year { get; set; }
    public decimal? TotalCalculatedComsuption { get; set; }
}
