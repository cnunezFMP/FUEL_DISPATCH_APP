using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.DataAccess.Repository.GenericRepository;
using FUEL_DISPATCH_API.Utils.ResponseObjects;
using Microsoft.AspNetCore.Http;

namespace FUEL_DISPATCH_API.DataAccess.Repository.Interfaces
{
    public interface IMaintenanceServices : IGenericInterface<Maintenance>
    {
        ResultPattern<AnexoMantenimiento> UploadAnexo(IFormFile file, int maintenanceId);
        ResultPattern<string> DeleteFile(int maintenanceId);
        //bool SetCurrentOdometerByVehicle(Maintenance maintenance);
        //bool SetNextMaintenanceDate(Maintenance maintenance);
        //bool SetNextMaintenanceOdometer(MaintenanceDetails maintenance);
    }
}
