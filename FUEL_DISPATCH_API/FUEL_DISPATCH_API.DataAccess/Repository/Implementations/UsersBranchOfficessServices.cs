using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.DataAccess.Repository.GenericRepository;
using FUEL_DISPATCH_API.DataAccess.Repository.Interfaces;
using FUEL_DISPATCH_API.Utils.Constants;
using FUEL_DISPATCH_API.Utils.ResponseObjects;
using Microsoft.AspNetCore.Http;

namespace FUEL_DISPATCH_API.DataAccess.Repository.Implementations
{
    public class UsersBranchOfficessServices : GenericRepository<UsersBranchOffices>, IUsersBranchOfficesServices
    {
        private readonly FUEL_DISPATCH_DBContext _DBContext;
        public UsersBranchOfficessServices(FUEL_DISPATCH_DBContext dbContext)
            : base(dbContext)
        {
            _DBContext = dbContext;
        }

        public override ResultPattern<UsersBranchOffices> Delete(Func<UsersBranchOffices, bool> predicate)
        {
            var userBranchOfficeEntityToDelete = _DBContext.UsersBranchOffices
                .FirstOrDefault(predicate);

            _DBContext.UsersBranchOffices.Remove(userBranchOfficeEntityToDelete!);
            _DBContext.SaveChanges();
            return ResultPattern<UsersBranchOffices>.Success(userBranchOfficeEntityToDelete!, StatusCodes.Status200OK, "User deleted from Branch Office. ");
        }

        // DONE: Actualizar.
        public override ResultPattern<UsersBranchOffices> Update(Func<UsersBranchOffices, bool> predicate, UsersBranchOffices updatedEntity)
        {
            var userCompanyEntity = _DBContext.UsersBranchOffices.FirstOrDefault(predicate);

            _DBContext.Entry(userCompanyEntity!).CurrentValues.SetValues(updatedEntity);
            _DBContext.SaveChanges();
            return ResultPattern<UsersBranchOffices>.Success(userCompanyEntity!, StatusCodes.Status200OK, AppConstants.DATA_UPDATED_MESSAGE);

        }
    }
}
