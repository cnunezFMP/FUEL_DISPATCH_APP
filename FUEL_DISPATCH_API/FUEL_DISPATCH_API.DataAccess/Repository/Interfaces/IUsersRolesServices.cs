using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.DataAccess.Repository.GenericRepository;
using FUEL_DISPATCH_API.Utils.ResponseObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUEL_DISPATCH_API.DataAccess.Repository.Interfaces
{
    public interface IUsersRolesServices : IGenericInterface<UsersRols>
    {
        ResultPattern<UsersRols> UpdateUserRol(Func<UsersRols, bool> predicate, UsersRols updatedEntity);
        bool UserHasTheRole(User user, int rolId);
    }
}
