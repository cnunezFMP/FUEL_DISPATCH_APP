using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.Utils.Constants;
using FUEL_DISPATCH_API.Utils.Exceptions;
using FUEL_DISPATCH_API.Utils.ResponseObjects;
using Gridify;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
namespace FUEL_DISPATCH_API.DataAccess.Repository.GenericRepository
{
    public abstract class GenericRepository<T> : IGenericInterface<T> where T : class
    {
        private readonly FUEL_DISPATCH_DBContext _DBContext;
        private readonly IHttpContextAccessor _httpContextAccesor;
        public GenericRepository(FUEL_DISPATCH_DBContext DBContext,
                                 IHttpContextAccessor httpContextAccesor)
        {
            _DBContext = DBContext;
            _httpContextAccesor = httpContextAccesor;
        }
        public virtual ResultPattern<T> Delete(Func<T, bool> predicate)
        {
            var entityToDelete = _DBContext.Set<T>().FirstOrDefault(predicate)
                ?? throw new NotFoundException(AppConstants.NOT_FOUND_MESSAGE);
            _DBContext.Set<T>().Remove(entityToDelete!);
            _DBContext.SaveChanges();
            return ResultPattern<T>.Success(entityToDelete!, StatusCodes.Status200OK, AppConstants.DATA_DELETED_MESSAGE);
        }
        public virtual ResultPattern<T> Get(Func<T, bool> predicate)
        {
            var entity = _DBContext.Set<T>().AsNoTrackingWithIdentityResolution().FirstOrDefault(predicate)
                ?? throw new NotFoundException(AppConstants.NOT_FOUND_MESSAGE);

            return ResultPattern<T>.Success(entity!, StatusCodes.Status200OK, AppConstants.DATA_OBTAINED_MESSAGE);
        }
        public virtual ResultPattern<Paging<T>> GetAll(GridifyQuery query)
        {
            string? companyId, branchId;
            companyId = _httpContextAccesor.HttpContext?.Items["CompanyId"]?.ToString();
            branchId = _httpContextAccesor.HttpContext?.Items["BranchOfficeId"]?.ToString();


            if (branchId is not null &&
                companyId is not null &&
                typeof(T).GetProperty("CompanyId") is not null &&
                typeof(T).GetProperty("BranchOfficeId") is not null)
            {
                var entitiesByCompAndBranch = _DBContext.Set<T>()
                    .AsNoTrackingWithIdentityResolution()
                    .Where(x => EF.Property<int>(x, "CompanyId") == int.Parse(companyId!) && EF.Property<int>(x, "BranchOfficeId") == int.Parse(branchId!))
                    .Gridify(query);

                return ResultPattern<Paging<T>>.Success(entitiesByCompAndBranch,
                                       StatusCodes.Status200OK,
                                       AppConstants.DATA_OBTAINED_MESSAGE);
            }

            if (companyId is not null && typeof(T).GetProperty("CompanyId") is not null)
            {
                var entitiesComp = _DBContext.Set<T>()
                    .AsNoTrackingWithIdentityResolution()
                    .Where(x => EF.Property<int>(x, "CompanyId") == int.Parse(companyId!))
                    .Gridify(query);

                return ResultPattern<Paging<T>>.Success(entitiesComp,
                    StatusCodes.Status200OK,
                    AppConstants.DATA_OBTAINED_MESSAGE);
            }

            if (branchId is not null && typeof(T).GetProperty("BranchOfficeId") is not null)
            {
                var entitiesBranch = _DBContext.Set<T>()
                    .AsNoTrackingWithIdentityResolution()
                    .Where(x => EF.Property<int>(x, "BranchOfficeId") == int.Parse(branchId!))
                    .Gridify(query);

                return ResultPattern<Paging<T>>.Success(entitiesBranch,
                    StatusCodes.Status200OK,
                    AppConstants.DATA_OBTAINED_MESSAGE);
            }

            if (companyId is not null ||
                branchId is not null &&
                typeof(T).GetProperty("BranchOfficeId") is null &&
                typeof(T).GetProperty("CompanyId") is null)
            {
                // Retornar todo
                var entities = _DBContext.Set<T>()
                    .AsNoTrackingWithIdentityResolution()
                    .Gridify(query);

                return ResultPattern<Paging<T>>.Success(entities,
                    StatusCodes.Status200OK,
                    AppConstants.DATA_OBTAINED_MESSAGE);
            }

            return ResultPattern<Paging<T>>.Success(new Paging<T>(),
                StatusCodes.Status200OK,
                "This user isn't in a company or branch office. ");

        }
        // DONE: Asignar los id de compañia y sucursal a las entidades que se creen.
        // DONE: Hacer esto al igual que el metodo GET, para los Id de compañia y sucursal.
        public virtual ResultPattern<T> Post(T entity)
        {
            string? companyId, branchId;
            companyId = _httpContextAccesor.HttpContext?.Items["CompanyId"]?.ToString();
            branchId = _httpContextAccesor.HttpContext?.Items["BranchOfficeId"]?.ToString();

            if (branchId is not null &&
                companyId is not null &&
                typeof(T).GetProperty("CompanyId") is not null &&
                typeof(T).GetProperty("BranchOfficeId") is not null)
            {
                typeof(T).GetProperty("CompanyId")?
                    .SetValue(entity,
                    int.Parse(companyId!));

                typeof(T).GetProperty("BranchOfficeId")?
                    .SetValue(entity,
                    int.Parse(branchId!));
            }

            if (companyId is not null &&
                typeof(T).GetProperty("CompanyId") is not null)
            {
                typeof(T).GetProperty("CompanyId")?
                    .SetValue(entity,
                    int.Parse(companyId!));
            }

            if (branchId is not null &&
                typeof(T).GetProperty("BranchOfficeId") is not null)
            {
                typeof(T).GetProperty("BranchOfficeId")?
                    .SetValue(entity,
                    int.Parse(branchId!));
            }

            _DBContext.Set<T>().Add(entity);
            _DBContext.SaveChanges();
            return ResultPattern<T>.Success(entity!,
                StatusCodes.Status201Created,
                AppConstants.DATA_SAVED_MESSAGE);
        }
        // UNDONE: Buscar alguna forma de que no se actualicen las propiedades que no envio en los PUT.
        public virtual ResultPattern<T> Update(Func<T, bool> predicate, T updatedEntity)
        {
            var entityToUpdate = _DBContext.Set<T>().FirstOrDefault(predicate)
                ?? throw new NotFoundException(AppConstants.NOT_FOUND_MESSAGE);
            _DBContext.Entry(entityToUpdate).CurrentValues.SetValues(updatedEntity);
            _DBContext.SaveChanges();
            return ResultPattern<T>.Success(entityToUpdate, StatusCodes.Status200OK, AppConstants.DATA_UPDATED_MESSAGE);
        }
    }
}