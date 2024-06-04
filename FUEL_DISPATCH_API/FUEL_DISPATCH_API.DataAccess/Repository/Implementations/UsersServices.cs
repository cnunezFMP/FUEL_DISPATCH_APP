using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.DataAccess.Repository.GenericRepository;
using FUEL_DISPATCH_API.DataAccess.Repository.Interfaces;
using FUEL_DISPATCH_API.Utils.Constants;
using FUEL_DISPATCH_API.Utils.Exceptions;
using FUEL_DISPATCH_API.Utils.ResponseObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
namespace FUEL_DISPATCH_API.DataAccess.Repository.Implementations
{
    public class UsersServices : GenericRepository<Users>, IUserServices
    {
        private readonly FUEL_DISPATCH_DBContext _DBContext;
        public UsersServices(FUEL_DISPATCH_DBContext dbContext)
            : base(dbContext)
        {
            _DBContext = dbContext;
        }

        public override ResultPattern<Users> Delete(Func<Users, bool> predicate)
        {
            var userToDelete = _DBContext.Users.FirstOrDefault(predicate)
                 ?? throw new NotFoundException(AppConstants.NOT_FOUND_MESSAGE);
            if (userToDelete.DriverId.HasValue)
                throw new BadRequestException("This user has driver assigned. ");
            _DBContext.Users.Remove(userToDelete!);
            _DBContext.SaveChanges();
            return ResultPattern<Users>.Success(userToDelete!, StatusCodes.Status200OK, AppConstants.DATA_DELETED_MESSAGE);
        }
        public ResultPattern<Users> DeleteUserRol(int userId, int roleId)
        {
            var user = _DBContext.Users.Include(x => x.Rols).FirstOrDefault(x=>x.Id == userId);
            if (user is null)
                throw new NotFoundException("This user doesn't exist. ");

            var rol = _DBContext.UsersRols.Find(userId, roleId);

            if(rol is null)
                throw new NotFoundException("This rol doesn't exist. ");

            _DBContext.UsersRols.Remove(rol!);
            _DBContext.SaveChanges();
            return ResultPattern<Users>.Success(user, StatusCodes.Status200OK, "Rol updated. ");
        }
        public ResultPattern<Users> UpdateUserRol(int userId, int roleId)
        {
            var user = _DBContext.Users.Include(x => x.Rols).FirstOrDefault(x => x.Id == userId);
            if (user is null)
                throw new NotFoundException("This user doesn't exist. ");

            var rol = _DBContext.Role.Find(roleId);

            if (rol is null)
                throw new NotFoundException("This rol doesn't exist. ");

            if (UserHasTheRole(user, roleId))
                throw new BadRequestException("This user has this role. ");

            user.Rols.Add(rol);
            _DBContext.Users.Update(user);
            _DBContext.SaveChanges();

            _DBContext.Role.Remove(rol!);
            _DBContext.SaveChanges();
            return ResultPattern<Users>.Success(user, StatusCodes.Status200OK, "User rols updated. ");
        }
        bool UserHasTheRole(Users user, int rolId)
            => user.Rols.Any(r => r.Id == rolId);
    }
}
