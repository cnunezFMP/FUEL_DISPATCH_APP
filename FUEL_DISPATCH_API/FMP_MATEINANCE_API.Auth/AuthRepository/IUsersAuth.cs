using FUEL_DISPATCH_API.DataAccess.DTOs;
using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.DataAccess.Repository.GenericRepository;
using FUEL_DISPATCH_API.Utils.ResponseObjects;

namespace FMP_MATEINANCE_API.Auth
{
    public interface IUsersAuth : IGenericInterface<User>
    {
        ResultPattern<object> Login(LoginDto loginDto);
       
    }
}
