using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.DataAccess.Repository.GenericRepository;
using FUEL_DISPATCH_API.DataAccess.Repository.Interfaces;
using FUEL_DISPATCH_API.Utils.Exceptions;
using FUEL_DISPATCH_API.Utils.ResponseObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace FUEL_DISPATCH_API.DataAccess.Repository.Implementations
{
    public class UsersCompaniesServices : GenericRepository<User>, ICompaniesUsersServices
    {
        private readonly FUEL_DISPATCH_DBContext _DBContext;
        public UsersCompaniesServices(FUEL_DISPATCH_DBContext dBContext) : base(dBContext)
        {
            _DBContext = dBContext;
        }
        public ResultPattern<UsersCompanies> DeleteUserCompany(int userId, int companyId)
        {
            //var user = _DBContext.User.Include(x => x.Companies).FirstOrDefault(x => x.Id == userId)
            //    ?? throw new NotFoundException("This user doesn't exist. ");

            var company = _DBContext.UsersCompanies.Find(userId, companyId)
                ?? throw new NotFoundException("Can't find relation user-company");

            _DBContext.UsersCompanies.Remove(company!);
            _DBContext.SaveChanges();
            return ResultPattern<UsersCompanies>.Success(company, StatusCodes.Status200OK, "User-Company Updated Successfully!");
        }
        public ResultPattern<User> UpdateUserCompany(int userId, int companieId)
        {
            var user = _DBContext.User.Include(x => x.Companies).FirstOrDefault(x => x.Id == userId)
                ?? throw new NotFoundException("This user doesn't exist. ");

            var company = _DBContext.Companies.Find(companieId)
                ?? throw new NotFoundException("This company doesn't exist. ");

            if (UserIsInTheCompany(user, companieId))
                throw new BadRequestException("This user has this company. ");

            user.Companies.Add(company);

            _DBContext.User.Update(user);
            _DBContext.SaveChanges();
            return ResultPattern<User>.Success(user, StatusCodes.Status200OK, "User company updated. ");
        }
        public bool UserIsInTheCompany(User user, int rolId)
            => user.Companies.Any(r => r.Id == rolId);
    }
}