using FUEL_DISPATCH_API.DataAccess.Models.SAP;
using FUEL_DISPATCH_API.DataAccess.Repository.GenericRepository;
namespace FUEL_DISPATCH_API.DataAccess.Repository.Interfaces
{
    public interface ISAPSLServices
    {
        bool Login(SapUserModel sapUserModel);
    }
}
