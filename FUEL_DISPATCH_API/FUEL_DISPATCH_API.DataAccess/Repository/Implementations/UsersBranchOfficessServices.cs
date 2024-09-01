using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.DataAccess.Repository.GenericRepository;
using FUEL_DISPATCH_API.DataAccess.Repository.Interfaces;
using FUEL_DISPATCH_API.Utils.Constants;
using FUEL_DISPATCH_API.Utils.Exceptions;
using FUEL_DISPATCH_API.Utils.ResponseObjects;
using Microsoft.AspNetCore.Http;

namespace FUEL_DISPATCH_API.DataAccess.Repository.Implementations
{
    public class UsersBranchOfficessServices : GenericRepository<UsersBranchOffices>, IUsersBranchOfficesServices
    {
        private readonly FUEL_DISPATCH_DBContext _DBContext;
        public UsersBranchOfficessServices(FUEL_DISPATCH_DBContext dbContext, IHttpContextAccessor httpContextAccessor)
            : base(dbContext, httpContextAccessor)
        {
            _DBContext = dbContext;
        }

        public override ResultPattern<UsersBranchOffices> Post(UsersBranchOffices entity)
        {
            if (IsUserInBranchOffice(entity.UserId, entity.BranchOfficeId))
                throw new BadRequestException("El usuario ya esta asignado a la sucursal. ");

            return base.Post(entity);
        }
        public override ResultPattern<UsersBranchOffices> Delete(Func<UsersBranchOffices, bool> predicate)
        {
            var userBranchOfficeEntityToDelete = _DBContext.UsersBranchOffices
                .FirstOrDefault(predicate)
                ?? throw new NotFoundException(AppConstants.NOT_FOUND_MESSAGE);

            _DBContext.UsersBranchOffices.Remove(userBranchOfficeEntityToDelete!);
            _DBContext.SaveChanges();
            return ResultPattern<UsersBranchOffices>.Success(userBranchOfficeEntityToDelete!, StatusCodes.Status200OK, "User deleted from Branch Office. ");
        }
        // DONE: Actualizar.
        public ResultPattern<UsersBranchOffices> UpdateUserBranchOffice(Func<UsersBranchOffices, bool> predicate, UsersBranchOffices updatedEntity)
        {
            var userBranchOfficeEntity = _DBContext.UsersBranchOffices
                .FirstOrDefault(predicate)
                ?? throw new NotFoundException(AppConstants.NOT_FOUND_MESSAGE);
            // Remove the existing entity
            _DBContext.UsersBranchOffices.Remove(userBranchOfficeEntity);
            _DBContext.SaveChanges();

            // Add the updated entity
            _DBContext.UsersBranchOffices.Add(updatedEntity);
            _DBContext.SaveChanges();

            return ResultPattern<UsersBranchOffices>.Success(updatedEntity, StatusCodes.Status200OK, AppConstants.DATA_UPDATED_MESSAGE);
        }
        // Verificar si el usuario pertenece a la sucursal.
        public bool IsUserInBranchOffice(int userId, int? branchOfficeId)
            => _DBContext.UsersBranchOffices.Any(x => x.UserId == userId &&
                       x.BranchOfficeId == branchOfficeId);
    }
}
