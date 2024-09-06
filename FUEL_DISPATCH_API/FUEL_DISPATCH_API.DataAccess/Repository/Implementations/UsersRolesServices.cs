using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.DataAccess.Repository.GenericRepository;
using FUEL_DISPATCH_API.DataAccess.Repository.Interfaces;
using FUEL_DISPATCH_API.Utils.Constants;
using FUEL_DISPATCH_API.Utils.Exceptions;
using FUEL_DISPATCH_API.Utils.ResponseObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace FUEL_DISPATCH_API.DataAccess.Repository.Implementations
{
    public class UsersRolesServices : GenericRepository<UsersRols>, IUsersRolesServices
    {
        private readonly FUEL_DISPATCH_DBContext _DBContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UsersRolesServices(FUEL_DISPATCH_DBContext dbContext, IHttpContextAccessor httpContextAccessor)
            : base(dbContext, httpContextAccessor)
        {
            _DBContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
        }
        public override ResultPattern<UsersRols> Post(UsersRols entity)
        {
            if (IsUserRol(entity.UserId, entity.RolId))
                throw new BadRequestException("El usuario ya tiene el rol asignado. ");

            return base.Post(entity);
        }
        public override ResultPattern<UsersRols> Delete(Func<UsersRols, bool> predicate)
        {
            var userRolEntityToDelete = _DBContext.UsersRols
                .FirstOrDefault(predicate)
                ?? throw new NotFoundException(AppConstants.NOT_FOUND_MESSAGE);

            _DBContext.UsersRols.Remove(userRolEntityToDelete!);
            _DBContext.SaveChanges();
            return ResultPattern<UsersRols>.Success(userRolEntityToDelete!, StatusCodes.Status200OK, "Rol deleted for user. ");
        }

        // DONE: Corregir: Me da un 405(Method not allowed. )
        public ResultPattern<UsersRols> UpdateUserRol(Func<UsersRols, bool> predicate, UsersRols updatedEntity)
        {
            var userRolEntity = _DBContext.UsersRols
                .FirstOrDefault(predicate)
                ?? throw new NotFoundException(AppConstants.NOT_FOUND_MESSAGE);

            // Remove the existing entity
            _DBContext.UsersRols.Remove(userRolEntity);
            _DBContext.SaveChanges();

            // Add the updated entity
            _DBContext.UsersRols.Add(updatedEntity);
            _DBContext.SaveChanges();

            return ResultPattern<UsersRols>.Success(updatedEntity, StatusCodes.Status200OK, AppConstants.DATA_UPDATED_MESSAGE);
        }
        // Verificar si el usuario tiene el rol.
        public bool IsUserRol(int userId, int rolId)
        {
            //string? companyId = _httpContextAccessor.HttpContext?.Items["CompanyId"]?.ToString(); 
            return _DBContext.UsersRols.Any(x => x.UserId == userId &&
                                  x.RolId == rolId /*&&
                                  x.CompanyId == int.Parse(companyId)*/);
        }
             
    }
}
