using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.Utils.Constants;
using FUEL_DISPATCH_API.Utils.Exceptions;
using FUEL_DISPATCH_API.Utils.ResponseObjects;
using Gridify;
using Microsoft.AspNetCore.Http;

namespace FUEL_DISPATCH_API.DataAccess.Repository.GenericRepository
{
    public abstract class GenericRepository<T> : IGenericInterface<T> where T : class
    {
        private readonly FUEL_DISPATCH_DBContext _DBContext;
        public GenericRepository(FUEL_DISPATCH_DBContext DBContext)
        {
            _DBContext = DBContext;
        }
        public virtual ResultPattern<T> Get(Func<T, bool> predicate)
        {
            var entity = _DBContext.Set<T>().FirstOrDefault(predicate)
                ?? throw new NotFoundException(AppConstants.NOT_FOUND_MESSAGE);

            return ResultPattern<T>.Success(entity!, StatusCodes.Status200OK, AppConstants.DATA_OBTAINED_MESSAGE);
        }
        public virtual ResultPattern<Paging<T>> GetAll(GridifyQuery query)
        {
            var entities = _DBContext.Set<T>().Gridify(query);
            return ResultPattern<Paging<T>>.Success(entities, StatusCodes.Status200OK, AppConstants.DATA_OBTAINED_MESSAGE);
        }
        public virtual ResultPattern<T> Post(T entity)
        {
            _DBContext.Set<T>().AddAsync(entity);
            _DBContext.SaveChanges();
            return ResultPattern<T>.Success(entity!, StatusCodes.Status201Created, AppConstants.DATA_SAVED_MESSAGE);
        }
        public virtual ResultPattern<string> SaveChanges()
        {
            _DBContext.SaveChanges();
            return ResultPattern<string>.Success(AppConstants.DATA_SAVED_MESSAGE, StatusCodes.Status200OK, "Data saved");
        }
        public virtual ResultPattern<T> Update(T entity)
        {
            _DBContext.Set<T>().Update(entity);
            _DBContext.SaveChanges();
            return ResultPattern<T>.Success(entity!, StatusCodes.Status200OK, AppConstants.DATA_UPDATED_MESSAGE);
        }
    }
}
