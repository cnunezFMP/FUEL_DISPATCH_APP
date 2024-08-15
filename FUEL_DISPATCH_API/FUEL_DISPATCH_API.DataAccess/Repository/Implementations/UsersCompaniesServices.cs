//using FUEL_DISPATCH_API.DataAccess.Models;
//using FUEL_DISPATCH_API.DataAccess.Repository.GenericRepository;
//using FUEL_DISPATCH_API.DataAccess.Repository.Interfaces;
//using FUEL_DISPATCH_API.Utils.Constants;
//using FUEL_DISPATCH_API.Utils.Exceptions;
//using FUEL_DISPATCH_API.Utils.ResponseObjects;
//using Microsoft.AspNetCore.Http;

//namespace FUEL_DISPATCH_API.DataAccess.Repository.Implementations
//{
//    public class UsersCompaniesServices : GenericRepository<UsersCompanies>, ICompaniesUsersServices
//    {
//        private readonly FUEL_DISPATCH_DBContext _DBContext;
//        public UsersCompaniesServices(FUEL_DISPATCH_DBContext dBContext, IHttpContextAccessor httpContextAccessor)
//            : base(dBContext, httpContextAccessor)
//        {
//            _DBContext = dBContext;
//        }
//        public override ResultPattern<UsersCompanies> Delete(Func<UsersCompanies, bool> predicate)
//        {
//            var userCompanyEntity = _DBContext.UsersCompanies
//                .FirstOrDefault(predicate);

//            _DBContext.UsersCompanies.Remove(userCompanyEntity!);
//            _DBContext.SaveChanges();
//            return ResultPattern<UsersCompanies>.Success(userCompanyEntity!, StatusCodes.Status200OK, "User removed from the company. ");
//        }
//        // DONE: Exception - Property cannot be null. (Parameter 'entity')
//        public ResultPattern<UsersCompanies> UpdateUserCompany(Func<UsersCompanies, bool> predicate, UsersCompanies updatedEntity)
//        {
//            var userCompanyEntity = _DBContext.UsersCompanies
//                .FirstOrDefault(predicate)
//                ?? throw new NotFoundException(AppConstants.NOT_FOUND_MESSAGE);

            
//            // Remove the existing entity
//            _DBContext.UsersCompanies.Remove(userCompanyEntity);
//            _DBContext.SaveChanges();

//            // Add the updated entity
//            _DBContext.UsersCompanies.Add(updatedEntity);
//            _DBContext.SaveChanges();

//            return ResultPattern<UsersCompanies>.Success(updatedEntity, StatusCodes.Status200OK, AppConstants.DATA_UPDATED_MESSAGE);
//        }
//        // Verificar si el usuario pertenece a la compañía.
//        public bool IsUserInCompany(int userId, int companyId)
//            => !_DBContext.UsersCompanies.Any(x => x.UserId == userId &&
//            x.CompanyId == companyId);
//    }
//}