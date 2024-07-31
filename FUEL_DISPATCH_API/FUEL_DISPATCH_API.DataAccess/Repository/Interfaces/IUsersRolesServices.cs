using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.DataAccess.Repository.GenericRepository;
using FUEL_DISPATCH_API.Utils.ResponseObjects;
namespace FUEL_DISPATCH_API.DataAccess.Repository.Interfaces
{
    public interface IUsersRolesServices : IGenericInterface<UsersRols>
    {
        ResultPattern<UsersRols> UpdateUserRol(Func<UsersRols, bool> predicate, UsersRols updatedEntity);
        public bool IsUserRol(int userId, int rolId);
    }
}
