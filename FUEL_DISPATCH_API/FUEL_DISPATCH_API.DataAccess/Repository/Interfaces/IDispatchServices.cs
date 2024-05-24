using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.Utils.ResponseObjects;

namespace FUEL_DISPATCH_API.DataAccess.Repository.Interfaces
{
    public interface IDispatchServices
    {
        Task<ResultPattern<List<Dispatch>>> GetDispatches(DateTime? startDate, DateTime? endDate, int page = 1, int pageSize = 10); // Get all dispatches.

        ResultPattern<Dispatch> GetDispatch(int id); // Get Dispatch by id.

        ResultPattern<Dispatch> CreateDispath(Dispatch newDispatch); // Create dispatch.
    }
}