using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.DataAccess.Repository.GenericRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUEL_DISPATCH_API.DataAccess.Repository.Interfaces
{
    public interface IWareHouseServices : IGenericInterface<WareHouse>
    {
        bool WareHouseExists(WareHouse wareHouse);
        // bool BranchOfficeExist(WareHouse wareHouse);
        bool SetWareHouseDir(WareHouse wareHouse);
    }
}