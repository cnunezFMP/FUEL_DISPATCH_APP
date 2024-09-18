
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
        public UsersServices(FUEL_DISPATCH_DBContext dbContext, IHttpContextAccessor httpContextAccessor)
            : base(dbContext, httpContextAccessor)
        {
            _DBContext = dbContext;
        }
        public override ResultPattern<User> Update(Func<User, bool> predicate, User updatedEntity)
        {
            var currentPassword = _DBContext.User.FirstOrDefault(predicate) ??
                throw new NotFoundException("Usuario no encontrado. ");

            if (updatedEntity.Password != currentPassword.Password)
            {
                string newPassword = PasswordHashing(updatedEntity.Password!);
                updatedEntity.Password = newPassword;
            }
            return base.Update(predicate, updatedEntity);
        }
        static string PasswordHashing(string password)
            => BCrypt.Net.BCrypt.HashPassword(password, 13);
        public override ResultPattern<User> Delete(Func<User, bool> predicate)
        {
            // DONE: Verificar los DELETE.
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
