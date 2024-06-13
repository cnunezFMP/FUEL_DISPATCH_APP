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
using Twilio.TwiML.Voice;

namespace FUEL_DISPATCH_API.DataAccess.Repository.Implementations
{
    public class WareHouseServices : GenericRepository<WareHouse>, IWareHouseServices
    {
        private readonly FUEL_DISPATCH_DBContext _DBContext;
        public WareHouseServices(FUEL_DISPATCH_DBContext dbContext)
            : base(dbContext)
        {
            _DBContext = dbContext;
        }
        private bool WareHousExists(WareHouse wareHouse)
            => _DBContext.WareHouse.Any(x => x.Code == wareHouse.Code);
        bool BranchOfficeIdHasValue(WareHouse wareHouse)
        {
            if (wareHouse.BranchOfficeId.HasValue)
            {
                var branchOffice = _DBContext.BranchOffices.FirstOrDefault(x => x.Id == wareHouse.BranchOfficeId);
                // DONE: Añadir propiedad Status a WareHouseMovement.
                if (wareHouse!.Status is ValidationConstants.InactiveStatus)
                    throw new BadRequestException("This wareHouse is Inactive. ");
                wareHouse.FullDirection = branchOffice!.FullLocation;
                return true;
            };
            return false;
        }
        public override ResultPattern<WareHouse> Post(WareHouse wareHouse)
        {
            if (WareHousExists(wareHouse))
                throw new BadRequestException("Warehouse with this code exists. ");
            BranchOfficeIdHasValue(wareHouse);

            _DBContext.WareHouse.Add(wareHouse);
            _DBContext.SaveChanges();
            return ResultPattern<WareHouse>.Success(wareHouse, StatusCodes.Status200OK, "Warehouse added successfully. ");
        }
    }
}
