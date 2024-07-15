using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.DataAccess.Repository.GenericRepository;

namespace FUEL_DISPATCH_API.DataAccess.Repository.Interfaces
{
    public interface IRequestServices : IGenericInterface<Request>
    {
        bool CheckVehicle(Request newRequest);
        bool CheckDispatch(Request newRequest);
    }
}
