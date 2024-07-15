using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.DataAccess.Repository.GenericRepository;
namespace FUEL_DISPATCH_API.DataAccess.Repository.Interfaces
{
    public interface IZoneServices : IGenericInterface<Zone>
    {
        bool ZoneCodeMustBeUnique(Zone zone);
    }
}
