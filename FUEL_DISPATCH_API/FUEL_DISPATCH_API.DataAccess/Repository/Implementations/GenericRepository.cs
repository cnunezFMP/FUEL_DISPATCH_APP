using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.DataAccess.Repository.Interfaces;
using FUEL_DISPATCH_API.Utils.Constants;
using FUEL_DISPATCH_API.Utils.Exceptions;
using FUEL_DISPATCH_API.Utils.ResponseObjects;
using Gridify;
using Microsoft.AspNetCore.Http;

namespace FUEL_DISPATCH_API.DataAccess.Repository.Implementations
{
    public class GenericRepository<T> : IGenericInterface<T> where T : class
    {
        private readonly FUEL_DISPATCH_DBContext _DBContext;
        public GenericRepository(FUEL_DISPATCH_DBContext DBContext) 
        {
            _DBContext = DBContext;
        }
        public ResultPattern<T> Get(Func<T, bool>predicate)
        {
            var entity = _DBContext.Set<T>().Where(predicate).FirstOrDefault();
            if (entity is null)
                throw new NotFoundException(AppConstants.NOT_FOUND_MESSAGE);

            return ResultPattern<T>.Success(entity!, StatusCodes.Status200OK, AppConstants.DATA_OBTAINED_MESSAGE);
        }

        public ResultPattern<Paging<T>> GetAll(GridifyQuery query)
        {
            var entities = _DBContext.Set<T>().Gridify(query);
            return ResultPattern<Paging<T>>.Success(entities, StatusCodes.Status200OK, AppConstants.DATA_OBTAINED_MESSAGE);
        }

        public ResultPattern<T> Post(T entity)
        {
            _DBContext.Set<T>().AddAsync(entity);
            return ResultPattern<T>.Success(entity!, StatusCodes.Status201Created, AppConstants.DATA_SAVED_MESSAGE);
        }

        public ResultPattern<T> Update(T entity)
        {
            _DBContext.Set<T>().Update(entity);
            return ResultPattern<T>.Success(entity!, StatusCodes.Status200OK, AppConstants.DATA_UPDATED_MESSAGE);
        }
    }
}
