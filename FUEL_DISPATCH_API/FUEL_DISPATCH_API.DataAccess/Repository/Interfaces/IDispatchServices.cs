using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.Utils.ResponseObjects;
using Gridify;
using Microsoft.AspNetCore.Mvc;

namespace FUEL_DISPATCH_API.DataAccess.Repository.Interfaces
{
    public interface IDispatchServices
    {
        // Task<ResultPattern<List<Dispatch>>> GetDispatches(DateTime? startDate, DateTime? endDate, string param, int page = 1, int pageSize = 10); // Get all dispatches.
        //Paging<Dispatch> GetAll([FromQuery] GridifyQuery query);
        ResultPattern<Dispatch> GetDispatch(int id); // Get Dispatch by id.

        ResultPattern<Dispatch> CreateDispath(Dispatch newDispatch); // Create dispatch.
    }
}