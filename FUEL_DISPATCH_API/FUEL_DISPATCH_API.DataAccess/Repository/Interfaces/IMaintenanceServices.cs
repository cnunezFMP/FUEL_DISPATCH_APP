using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.DataAccess.Repository.GenericRepository;
using FUEL_DISPATCH_API.Utils.ResponseObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUEL_DISPATCH_API.DataAccess.Repository.Interfaces
{
    public interface IMaintenanceServices : IGenericInterface<Maintenance>
    {
        ResultPattern<string> UploadAnexo(IFormFile file, int maintenanceId);
        //bool SetCurrentOdometerByVehicle(Maintenance maintenance);
        //bool SetNextMaintenanceDate(Maintenance maintenance);
        //bool SetNextMaintenanceOdometer(MaintenanceDetails maintenance);
    }
}
