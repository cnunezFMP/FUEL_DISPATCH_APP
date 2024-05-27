using FMP_DISPATCH_API.Services.Emails;
using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.DataAccess.Repository.Interfaces;
using FUEL_DISPATCH_API.Utils.Constants;
using FUEL_DISPATCH_API.Utils.Exceptions;
using FUEL_DISPATCH_API.Utils.ResponseObjects;
using Gridify;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System.Linq.Expressions;

namespace FUEL_DISPATCH_API.DataAccess.Repository.Implementations
{
    public class DispatchServices : IDispatchServices, IGridifyServices<Dispatch>
    {
        private readonly FUEL_DISPATCH_DBContext _DBContext;
        private readonly IEmailSender _emailSender;

        public DispatchServices(FUEL_DISPATCH_DBContext DBContext, IEmailSender emailSender)
        {
            _DBContext = DBContext;
            _emailSender = emailSender;
        }

        public ResultPattern<Dispatch> CreateDispath(Dispatch newDispatch)
        {
            var driverForDispatch = _DBContext.Drivers.Find(newDispatch.DriverId); // Se busca el conductor en la base de datos utilizando el ID proporcionado en el cuerpo
            var vehicleForDispatch = _DBContext.Vehicles.Find(newDispatch.VehicleToken); // Se busca el vehículo en la base de datos utilizando el ID proporcionado en el cuerpo
                                                                                         //var dispenserForDispatch = _DBContext.Dispensers.Find(newDispatch.DispenserId); // Se busca el dispensador en la base de datos utilizando el ID proporcionado en el cuerpo
            var roadForDispatch = _DBContext.Roads.Find(newDispatch.RoadId); // Se busca la ruta en la base de datos utilizando el ID proporcionado en el cuerpo
            var locationForDispatch = _DBContext.BranchOffices.Find(newDispatch.BranchOfficeId); // Se busca la ubicacion en la base de datos utilizando el ID proporcionado en el cuerpo
            var previousDispatch = _DBContext.Dispatch.Where(x => x.VehicleToken == newDispatch.VehicleToken).OrderByDescending(x => x.Id).FirstOrDefault();
            //var odomentroAnteriorMant = _DBContext.Mantenimientos.Where(x => x.VehiculoId == crearDespacho.VehiculoId).OrderByDescending(x => x.Id).FirstOrDefaultAsync();

            //if (newDispatch.Odometer < previousDispatch?.Odometer)
            //    return ResultPattern<Dispatch>.Failure();

            if (newDispatch.Odometer == previousDispatch?.Odometer)
                // throw new NewDispatchOdometerCantBeEqualsToThePreviousDispatchException();

                if (newDispatch.Gallons is ValidationConstants.ZeroGallons)
                    return ResultPattern<Dispatch>.Failure(StatusCodes.Status400BadRequest, "Gallons for dispatch can't be zero");

            /***************************
             *
             * // Drivers validations.
             *
             ***************************/

            if (driverForDispatch is null)
                // throw new DriverForDispatchNotFoundException(driverForDispatch!.Id);

                if (driverForDispatch.Status is ValidationConstants.InactiveStatus)
                    //return ServiceResults.DriverIsInactive(driverForDispatch.Id);
                    /***************************
                     *
                     * // Vehicle validations.
                     *
                     ***************************/

                    if (vehicleForDispatch is null)
                        //return ServiceResults.VehicleForDispatchNotFound(vehicleForDispatch!.Token);

                        if (vehicleForDispatch.Status is ValidationConstants.InactiveStatus)
                            //return ServiceResults.VehicleIsInactive(vehicleForDispatch!.Token);

                            /***************************
                             *
                             * // Road validations.
                             *
                             ***************************/
                            _DBContext.Dispatch.Add(newDispatch);
            _DBContext.SaveChanges();
            return ResultPattern<Dispatch>.Success(newDispatch, StatusCodes.Status200OK, "Despacho creado correctamente. ");
        }

        public ResultPattern<Dispatch> GetDispatch(int id)
        {
            var dispatch = _DBContext.Dispatch.Find(id);
            if (dispatch is null)
                return ResultPattern<Dispatch>.Failure(StatusCodes.Status400BadRequest, "No Dispatch found. ");

            return ResultPattern<Dispatch>.Success(dispatch, StatusCodes.Status200OK, "Dispatch obtained. ");
        }
        public Paging<Dispatch> GetAll(GridifyQuery query)
        {
            return _DBContext.Set<Dispatch>().Gridify(query);
        }
    }
}