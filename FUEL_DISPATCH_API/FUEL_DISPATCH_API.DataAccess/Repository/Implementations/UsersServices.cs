
using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.DataAccess.Repository.GenericRepository;
using FUEL_DISPATCH_API.DataAccess.Repository.Interfaces;
using FUEL_DISPATCH_API.Utils.Constants;
using FUEL_DISPATCH_API.Utils.Exceptions;
using FUEL_DISPATCH_API.Utils.ResponseObjects;
using Microsoft.AspNetCore.Http;

namespace FUEL_DISPATCH_API.DataAccess.Repository.Implementations
{
    public class UsersServices : GenericRepository<User>, IUserServices
    {
        private readonly FUEL_DISPATCH_DBContext _DBContext;

        public UsersServices(FUEL_DISPATCH_DBContext dbContext) : base(dbContext) { _DBContext = dbContext; }

        public override ResultPattern<User> Delete(Func<User, bool> predicate)
        {
            var userToDelete = _DBContext.User.FirstOrDefault(predicate) ??
                throw new NotFoundException(AppConstants.NOT_FOUND_MESSAGE);
            if (userToDelete.DriverId.HasValue)
                throw new BadRequestException("This user has driver assigned. ");
            _DBContext.User.Remove(userToDelete!);
            _DBContext.SaveChanges();
            return ResultPattern<User>.Success(
                userToDelete!,
                StatusCodes.Status200OK,
                AppConstants.DATA_DELETED_MESSAGE);
        }


    }
}
