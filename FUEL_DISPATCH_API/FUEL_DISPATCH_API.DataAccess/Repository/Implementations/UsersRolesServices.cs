using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.DataAccess.Repository.GenericRepository;
using FUEL_DISPATCH_API.DataAccess.Repository.Interfaces;
using FUEL_DISPATCH_API.Utils.Exceptions;
using FUEL_DISPATCH_API.Utils.ResponseObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUEL_DISPATCH_API.DataAccess.Repository.Implementations
{
    public class UsersRolesServices : GenericRepository<User>, IUsersRolesServices
    {
        private readonly FUEL_DISPATCH_DBContext _DBContext;
        public UsersRolesServices(FUEL_DISPATCH_DBContext dbContext, IHttpContextAccessor httpContextAccessor)
            : base(dbContext, httpContextAccessor)
        {
            _DBContext = dbContext;
        }
        public ResultPattern<User> DeleteUserRol(int userId, int roleId)
        {
            var user = _DBContext.User.Include(x => x.Rols).FirstOrDefault(x => x.Id == userId)
                ?? throw new NotFoundException("This user doesn't exist. ");

            var rol = _DBContext.UsersRols.Find(userId, roleId)
                ?? throw new NotFoundException("This rol doesn't exist. ");

            _DBContext.UsersRols.Remove(rol!);
            _DBContext.SaveChanges();
            return ResultPattern<User>.Success(user, StatusCodes.Status200OK, "Rol updated. ");
        }

        // DONE: Corregir: Me da un 405(Method not allowed. )
        public ResultPattern<User> UpdateUserRol(int userId, int roleId)
        {
            var user = _DBContext.User.Include(x => x.Rols).FirstOrDefault(x => x.Id == userId)
                ?? throw new NotFoundException("This user doesn't exist. ");
            
            var rol = _DBContext.Role.Find(roleId)
                ?? throw new NotFoundException("This rol doesn't exist. ");                

            if (UserHasTheRole(user, roleId))
                throw new BadRequestException("This user has this role. ");

            user.Rols!.Add(rol);
            _DBContext.User.Update(user);
            _DBContext.SaveChanges();
            return ResultPattern<User>.Success(user, StatusCodes.Status200OK, "User rols updated. ");
        }
        public bool UserHasTheRole(User user, int rolId)
            => user.Rols!.Any(r => r.Id == rolId);
    }
}
