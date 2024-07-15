using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.Utils.ResponseObjects;

namespace FUEL_DISPATCH_API.DataAccess.Repository.Interfaces
{
    public interface ICompaniesUsersServices
    {
        bool UserIsInTheCompany(User user, int companieId);
        ResultPattern<User> UpdateUserCompany(int userId, int companieId);
        ResultPattern<UsersCompanies> DeleteUserCompany(int userId, int companieId);
    }
}
