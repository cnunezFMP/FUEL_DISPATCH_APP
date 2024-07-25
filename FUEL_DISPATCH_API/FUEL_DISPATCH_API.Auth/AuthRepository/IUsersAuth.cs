using FUEL_DISPATCH_API.DataAccess.DTOs;
using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.DataAccess.Repository.GenericRepository;
using FUEL_DISPATCH_API.Utils.ResponseObjects;

namespace FUEL_DISPATCH_API.Auth
{
    public interface IUsersAuth : IGenericInterface<User>
    {
        ResultPattern<object> Login(LoginDto loginDto);
        ResultPattern<User> UserRegistration(UserRegistrationDto entity);
        bool IsUserNameUnique(UserRegistrationDto user);
        bool IsEmailUnique(UserRegistrationDto user);
        bool DriverIdHasValue(UserRegistrationDto user);
    }
}
