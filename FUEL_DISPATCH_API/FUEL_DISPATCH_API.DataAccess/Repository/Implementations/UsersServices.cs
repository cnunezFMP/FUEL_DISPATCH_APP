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
    public class UsersServices : GenericRepository<User>, IUserServices
    {
        private readonly FUEL_DISPATCH_DBContext _DBContext;
        public UsersServices(FUEL_DISPATCH_DBContext dbContext)
            : base(dbContext)
        {
            _DBContext = dbContext;
        }

        public override ResultPattern<User> Delete(Func<User, bool> predicate)
        {
            var userToDelete = _DBContext.User.FirstOrDefault(predicate)
                 ?? throw new NotFoundException(AppConstants.NOT_FOUND_MESSAGE);
            if (userToDelete.DriverId.HasValue)
                throw new BadRequestException("This user has driver assigned. ");
            _DBContext.User.Remove(userToDelete!);
            _DBContext.SaveChanges();
            return ResultPattern<User>.Success(userToDelete!, StatusCodes.Status200OK, AppConstants.DATA_DELETED_MESSAGE);
        }
        public ResultPattern<User> DeleteUserRol(int userId, int roleId)
        {
            var user = _DBContext.User.Include(x => x.Rols).FirstOrDefault(x => x.Id == userId);
            if (user is null)
                throw new NotFoundException("This user doesn't exist. ");

            var rol = _DBContext.UsersRols.Find(userId, roleId);

            if (rol is null)
                throw new NotFoundException("This rol doesn't exist. ");

            _DBContext.UsersRols.Remove(rol!);
            _DBContext.SaveChanges();
            return ResultPattern<User>.Success(user, StatusCodes.Status200OK, "Rol updated. ");
        }

        public ResultPattern<User> UpdateUserCompanie(int userId, int companieId)
        {
            var user = _DBContext.User.Include(x => x.Companies).FirstOrDefault(x => x.Id == userId);
            if (user is null)
                throw new NotFoundException("This user doesn't exist. ");

            var company = _DBContext.Companies.Find(companieId);

            if (company is null)
                throw new NotFoundException("This company doesn't exist. ");

            if (UserHasTheRole(user, companieId))
                throw new BadRequestException("This user has this company. ");

#pragma warning disable CS8602 // Dereference of a possibly null reference.
            user.Companies.Add(company);
#pragma warning restore CS8602 // Dereference of a possibly null reference.

            _DBContext.User.Update(user);
            _DBContext.SaveChanges();

            _DBContext.Companies.Remove(company);
            _DBContext.SaveChanges();
            return ResultPattern<User>.Success(user, StatusCodes.Status200OK, "User rols updated. ");

        }

        public ResultPattern<User> UpdateUserRol(int userId, int roleId)
        {
            var user = _DBContext.User.Include(x => x.Rols).FirstOrDefault(x => x.Id == userId);
            if (user is null)
                throw new NotFoundException("This user doesn't exist. ");

            var rol = _DBContext.Role.Find(roleId);

            if (rol is null)
                throw new NotFoundException("This rol doesn't exist. ");

            if (UserHasTheRole(user, roleId))
                throw new BadRequestException("This user has this role. ");

#pragma warning disable CS8602 // Dereference of a possibly null reference.
            user.Rols.Add(rol);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            _DBContext.User.Update(user);
            _DBContext.SaveChanges();

            _DBContext.Role.Remove(rol!);
            _DBContext.SaveChanges();
            return ResultPattern<User>.Success(user, StatusCodes.Status200OK, "User rols updated. ");
        }

        bool UserHasTheRole(User user, int rolId)
            => user.Rols.Any(r => r.Id == rolId);

    }
}
