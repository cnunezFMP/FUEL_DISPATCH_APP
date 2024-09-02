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
            var entity = _DBContext.Set<T>()
                .AsNoTrackingWithIdentityResolution()
                .FirstOrDefault(predicate)
                ?? throw new NotFoundException(AppConstants.NOT_FOUND_MESSAGE);

            return ResultPattern<T>.Success(entity!, StatusCodes.Status200OK, AppConstants.DATA_OBTAINED_MESSAGE);
        }
        public virtual ResultPattern<Paging<T>> GetAll(GridifyQuery query)
        {
            string? companyId, branchId;
            companyId = _httpContextAccesor
                .HttpContext?
                .Items["CompanyId"] as string ??
                string.Empty;

            branchId = _httpContextAccesor
                .HttpContext?
                .Items["BranchOfficeId"] as string ??
                string.Empty;

            if (string.IsNullOrEmpty(companyId))
            {
                return ResultPattern<Paging<T>>.Success(new Paging<T>(),
                    StatusCodes.Status401Unauthorized,
                    "This user isn't in a company. ");
            }

            if (typeof(T).GetProperty("CompanyId") is not null &&
                typeof(T).GetProperty("BranchOfficeId") is not null)
            {
                var entitiesByCompAndBranch = _DBContext.Set<T>()
                    .AsNoTrackingWithIdentityResolution()
                    .Where(x => EF.Property<int>(x, "CompanyId") == int.Parse(companyId!) &&
                    EF.Property<int>(x, "BranchOfficeId") == int.Parse(branchId!))
                    .ApplyFilteringAndOrdering(query);

                var responseComBr = new Paging<T>
                {
                    Data = entitiesByCompAndBranch,
                    Count = entitiesByCompAndBranch.Count()
                };

                if(responseComBr.Count == 0)
                    return ResultPattern<Paging<T>>.Success(new Paging<T>(),
                                       StatusCodes.Status400BadRequest,
                                       "No hay datos en tu sucursal. ");

                return ResultPattern<Paging<T>>.Success(responseComBr,
                                       StatusCodes.Status200OK,
                                       AppConstants.DATA_OBTAINED_MESSAGE);
            }
            if (typeof(T)
                .GetProperty("BranchOfficeId") is not null)
            {
                var entitiesComp = _DBContext.Set<T>()
                    .AsNoTrackingWithIdentityResolution()
                    .Where(x => EF.Property<int>(x, "CompanyId") == int.Parse(companyId!))
                    .ApplyFilteringAndOrdering(query);

                var responseComp = new Paging<T>
                {
                    Data = entitiesComp,
                    Count = entitiesComp.Count()
                };
                return ResultPattern<Paging<T>>.Success(responseComp,
                    StatusCodes.Status200OK,
                    AppConstants.DATA_OBTAINED_MESSAGE);
            }

            var entities = _DBContext.Set<T>()
                .AsNoTrackingWithIdentityResolution()
                .ApplyFilteringAndOrdering(query);

            var responseEntities = new Paging<T>
            {
                Data = entities,
                Count = entities.Count()
            };

            return ResultPattern<Paging<T>>.Success(responseEntities,
                    StatusCodes.Status200OK,
                    AppConstants.DATA_OBTAINED_MESSAGE);
        }
        // DONE: Asignar los id de compañia y sucursal a las entidades que se creen.
        // DONE: Hacer esto al igual que el metodo GET, para los Id de compañia y sucursal.
        public virtual ResultPattern<T> Post(T entity)
        {
            _DBContext.Set<T>().Add(entity);
            _DBContext.SaveChanges();
            return ResultPattern<T>.Success(entity!,
                StatusCodes.Status201Created,
                AppConstants.DATA_SAVED_MESSAGE);
        }
        // UNDONE: Buscar alguna forma de que no se actualicen las propiedades que no envio en los PUT.
        public virtual ResultPattern<T> Update(Func<T, bool> predicate, T updatedEntity)
        {
            // UNDONE: Buscar el objeto por compañia y sucursal. 
            var entityToUpdate = _DBContext
                .Set<T>()
                .FirstOrDefault(predicate)
                ?? throw new NotFoundException(AppConstants.NOT_FOUND_MESSAGE);
            _DBContext.Entry(entityToUpdate)
                .CurrentValues
                .SetValues(updatedEntity);
            _DBContext.Update(entityToUpdate);
            _DBContext.SaveChanges();
            return ResultPattern<T>.Success(entityToUpdate,
                StatusCodes.Status200OK,
                AppConstants.DATA_UPDATED_MESSAGE);
        }
    }
}