namespace FUEL_DISPATCH_API.DataAccess.Models;
public class CalculatedComsuptionReport
{
    public int? Id { get; set; }
    public int? VehicleId { get; set; }
    public string? MakeName { get; set; }
    public string? ModelName { get; set; }
    public int? DriverId { get; set; }
    public string? DriverFullName { get; set; }
    public int? BranchOfficeId { get; set; }
    public string? BranchName { get; set; }
    public int? CompanyId { get; set; }
    public decimal? Odometer { get; set; }
    public decimal? Qty { get; set; }
    public int? ItemId { get; set; }
    public string? ArticleNumber { get; set; }
    public string? ArticleDescription { get; set; }
    public string? MeasureName { get; set; }
    public int? MedidaDelVehiculo { get; set; }
    public decimal? AverageConsumption { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime? CreatedAt { get; set; }
    public string? Dispatcher { get; set; }
    public decimal? TotalCalculatedComsuption { get; set; }
}
