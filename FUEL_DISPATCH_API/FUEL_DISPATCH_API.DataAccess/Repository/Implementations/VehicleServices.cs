using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.DataAccess.Repository.GenericRepository;
using FUEL_DISPATCH_API.DataAccess.Repository.Interfaces;
using FUEL_DISPATCH_API.Utils.Constants;
using FUEL_DISPATCH_API.Utils.Exceptions;
using FUEL_DISPATCH_API.Utils.ResponseObjects;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUEL_DISPATCH_API.DataAccess.Repository.Implementations
{
    public partial class VehiclesServices : GenericRepository<Vehicle>, IVehiclesServices
    {
        public readonly FUEL_DISPATCH_DBContext _DBContext;
        public VehiclesServices(FUEL_DISPATCH_DBContext DBContext)
            : base(DBContext)
        {
            _DBContext = DBContext;
        }
        public override ResultPattern<Vehicle> Post(Vehicle entity)
        {
            if (CheckIfMeasureExists(entity))
                throw new NotFoundException("The measure doesn't exists. ");

            if (CheckIfMakeExists(entity))
                throw new NotFoundException("Make not found. ");

            if (CheckIfModelExists(entity))
                throw new NotFoundException("Model not found. ");

            if (CheckIfGenerationExists(entity))
                throw new NotFoundException("Generation not found. ");

            if (CheckIfModEngineExists(entity))
                throw new NotFoundException("Generation not found. ");

            DriverIdHasValue(entity);

            _DBContext.Vehicle.Add(entity);
            _DBContext.SaveChanges();
            if (DriverIdHasValue(entity))
                CheckAndUpdateDriver(entity);

            return ResultPattern<Vehicle>.Success(entity, StatusCodes.Status201Created, "Vehicle created successfully. ");
        }
        
    }
}
