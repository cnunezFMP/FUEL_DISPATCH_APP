using Azure;
using FMP_DISPATCH_API.Services.Emails;
using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.DataAccess.Repository.Interfaces;
using FUEL_DISPATCH_API.Utils.ResponseObjects;
using Gridify;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FUEL_DISPATCH_API.DataAccess.Repository.Implementations
{
    public class DispatchServices : IDispatchServices
    {
        private readonly FUEL_DISPATCH_DBContext _DBContext;
        private readonly IEmailSender _emailSender;
        private const int ZeroGallons = 0;
        private const string InactiveStatus = "Inactive";
        public DispatchServices(FUEL_DISPATCH_DBContext DBContext, IEmailSender emailSender)
        {
            _DBContext = DBContext;
            _emailSender = emailSender;
        }

        public ServiceResults CreateDispath(Dispatch newDispatch)
        {
            try
            {
                var driverForDispatch = _DBContext.Drivers.Find(newDispatch.DriverId); // Se busca el conductor en la base de datos utilizando el ID proporcionado en el cuerpo
                var vehicleForDispatch = _DBContext.Vehicles.Find(newDispatch.VehicleToken); // Se busca el vehículo en la base de datos utilizando el ID proporcionado en el cuerpo
                //var dispenserForDispatch = _DBContext.Dispensers.Find(newDispatch.DispenserId); // Se busca el dispensador en la base de datos utilizando el ID proporcionado en el cuerpo
                var roadForDispatch = _DBContext.Roads.Find(newDispatch.RoadId); // Se busca la ruta en la base de datos utilizando el ID proporcionado en el cuerpo
                var locationForDispatch = _DBContext.BranchOffices.Find(newDispatch.BranchOfficeId); // Se busca la ubicacion en la base de datos utilizando el ID proporcionado en el cuerpo
                var previousDispatch = _DBContext.Dispatch.Where(x => x.VehicleToken == newDispatch.VehicleToken).OrderByDescending(x => x.Id).FirstOrDefault();
                //var odomentroAnteriorMant = _DBContext.Mantenimientos.Where(x => x.VehiculoId == crearDespacho.VehiculoId).OrderByDescending(x => x.Id).FirstOrDefaultAsync();

                if (newDispatch.Odometer < previousDispatch?.Odometer)
                    return ServiceResults.NewOdometerCantBeLowerThanPreviousDispatch();

                if (newDispatch.Odometer == previousDispatch?.Odometer)
                {
                    return ServiceResults.NewDispatchOdometerCantBeEqualsToThePreviousDispatch();
                    throw new Exception();
                }

                if (newDispatch.Gallons is ZeroGallons)
                {
                    return ServiceResults.GallonsForDispatchCantBeZero();
                    throw new Exception();
                }

                /***************************
                 * 
                 * // Drivers validations. 
                 *
                 ***************************/

                if (driverForDispatch is null)
                    return ServiceResults.DriverForDispatchNotFound(driverForDispatch!.Id);


                if (driverForDispatch.Status is InactiveStatus)
                    return ServiceResults.DriverIsInactive(driverForDispatch.Id);
                /***************************
                 * 
                 * // Vehicle validations.
                 *
                 ***************************/

                if (vehicleForDispatch is null)
                    return ServiceResults.VehicleForDispatchNotFound(vehicleForDispatch!.Token);
                if (vehicleForDispatch.Status is InactiveStatus)
                    return ServiceResults.VehicleIsInactive(vehicleForDispatch!.Token); ;

                /***************************
                 * 
                 * // Road validations.
                 *
                 ***************************/
                _DBContext.Dispatch.Add(newDispatch);
                _DBContext.SaveChanges();
                return ServiceResults.DispatchSuccessfully(newDispatch.Id);
            }
            catch
            {
                throw;
            }
        }

        public Dispatch GetDispatch(int id)
        {
            try
            {
                var dispatch = _DBContext.Dispatch.Find(id);
                if (dispatch is null)
                {
                    throw new NullReferenceException();
                }
                return dispatch;
            }
            catch
            {
                throw;
            }
        }

        public async Task<List<Dispatch>> GetDispatches
            (
                [FromQuery] DateTime? startDate,
                [FromQuery] DateTime? endDate,
                int page,
                int pageSize
            )
        {
            try
            {
                var gq = new GridifyQuery
                {
                    Page = page,
                    PageSize = pageSize,
                };

                var dispatchQuery = startDate.HasValue && endDate.HasValue
                    ? _DBContext.Dispatch.Where(x => x.CreatedAt >= startDate && x.CreatedAt <= endDate)
                    : _DBContext.Dispatch;

                var dispatchList = await dispatchQuery.ApplyPaging(gq).ToListAsync();

                return dispatchList;
            }
            catch
            {
                throw;
            }

        }

    }
}
