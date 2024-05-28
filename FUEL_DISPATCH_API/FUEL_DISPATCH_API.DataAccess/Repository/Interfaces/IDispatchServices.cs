using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.DataAccess.Repository.GenericRepository;
using FUEL_DISPATCH_API.Utils.ResponseObjects;
using Gridify;
using Microsoft.AspNetCore.Mvc;

namespace FUEL_DISPATCH_API.DataAccess.Repository.Interfaces
{
    public interface IDispatchServices : IGenericInterface<Dispatch>
    {
    }
}