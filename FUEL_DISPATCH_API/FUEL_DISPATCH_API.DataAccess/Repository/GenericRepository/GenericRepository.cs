using Azure;
using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.DataAccess.Repository.Implementations;
using FUEL_DISPATCH_API.Utils.Constants;
using FUEL_DISPATCH_API.Utils.Exceptions;
using FUEL_DISPATCH_API.Utils.ResponseObjects;
using Gridify;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;

namespace FUEL_DISPATCH_API.DataAccess.Repository.GenericRepository
{
    public abstract class GenericRepository<T> : IGenericInterface<T> where T : class
    {
        private readonly FUEL_DISPATCH_DBContext _DBContext;
        private readonly IHttpContextAccessor _httpContextAccesor;
        public GenericRepository(FUEL_DISPATCH_DBContext DBContext, IHttpContextAccessor httpContextAccesor)
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

            if (companyId is not null && branchId is not null && typeof(T).GetProperty("BranchOfficeId") is not null)
            {
                var entities = _DBContext.Set<T>()
                    .AsNoTrackingWithIdentityResolution()
                    .Where(x => EF.Property<int>(x, "CompanyId") == int.Parse(companyId) &&
                    EF.Property<int>(x, "BranchOfficeId") == int.Parse(companyId))
                    .Gridify(query);

                return ResultPattern<Paging<T>>.Success(entities, StatusCodes.Status200OK, AppConstants.DATA_OBTAINED_MESSAGE);
            }

            var entitiesComp = _DBContext.Set<T>()
                .AsNoTrackingWithIdentityResolution()
                .Where(x => EF.Property<int>(x, "CompanyId") == int.Parse(companyId!))
                .Gridify(query);

            return ResultPattern<Paging<T>>.Success(entitiesComp,
                StatusCodes.Status200OK,
                AppConstants.DATA_OBTAINED_MESSAGE);
        }
        public virtual ResultPattern<T> Post(T entity)
        {
            _DBContext.Set<T>().Add(entity);
            _DBContext.SaveChanges();
            return ResultPattern<T>.Success(entity!, StatusCodes.Status201Created, AppConstants.DATA_SAVED_MESSAGE);
        }
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