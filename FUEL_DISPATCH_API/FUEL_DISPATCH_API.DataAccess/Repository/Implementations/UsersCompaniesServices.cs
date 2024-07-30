using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.DataAccess.Repository.GenericRepository;
using FUEL_DISPATCH_API.DataAccess.Repository.Interfaces;
using FUEL_DISPATCH_API.Utils.Constants;
using FUEL_DISPATCH_API.Utils.ResponseObjects;
using Microsoft.AspNetCore.Http;

namespace FUEL_DISPATCH_API.DataAccess.Repository.Implementations
{
    public class UsersCompaniesServices : GenericRepository<UsersCompanies>, ICompaniesUsersServices
    {
        private readonly FUEL_DISPATCH_DBContext _DBContext;
        public UsersCompaniesServices(FUEL_DISPATCH_DBContext dBContext, IHttpContextAccessor httpContextAccessor)
            : base(dBContext, httpContextAccessor)
        {
            _DBContext = dBContext;
        }
        public override ResultPattern<UsersCompanies> Delete(Func<UsersCompanies, bool> predicate)
        {
            var userCompanyEntity = _DBContext.UsersCompanies
                .FirstOrDefault(predicate);

            _DBContext.UsersCompanies.Remove(userCompanyEntity!);
            _DBContext.SaveChanges();
            return ResultPattern<UsersCompanies>.Success(userCompanyEntity!, StatusCodes.Status200OK, "User removed from the company. ");
        }
        // TODO: Exception - Property cannot be null. (Parameter 'entity')
        public override ResultPattern<UsersCompanies> Update(Func<UsersCompanies, bool> predicate, UsersCompanies updatedEntity)
        {
            var userCompanyEntity = _DBContext.UsersCompanies.FirstOrDefault(predicate);
            _DBContext.Entry(userCompanyEntity!).CurrentValues.SetValues(updatedEntity);
            _DBContext.SaveChanges();
            return ResultPattern<UsersCompanies>.Success(userCompanyEntity!, StatusCodes.Status200OK, AppConstants.DATA_UPDATED_MESSAGE);
        }
        public bool UserIsInTheCompany(User user, int rolId)
            => user.Companies.Any(r => r.Id == rolId);
    }
}