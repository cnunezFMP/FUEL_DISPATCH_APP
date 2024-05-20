using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.Utils.ResponseObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUEL_DISPATCH_API.DataAccess.Repository.Interfaces
{
    public interface IDispatchServices
    {
        Task<List<Dispatch>> GetDispatches(DateTime? startDate, DateTime? endDate, int page = 1, int pageSize = 10); // Get all dispatches.
        Dispatch GetDispatch(int id); // Get Dispatch by id.
        ServiceResults CreateDispath(Dispatch newDispatch); // Create dispatch.
    }
}
