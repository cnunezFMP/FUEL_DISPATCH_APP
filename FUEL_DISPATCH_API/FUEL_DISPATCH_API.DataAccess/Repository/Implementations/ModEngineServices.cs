using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.DataAccess.Repository.GenericRepository;
using FUEL_DISPATCH_API.DataAccess.Repository.Interfaces;
using FUEL_DISPATCH_API.Utils.Constants;
using FUEL_DISPATCH_API.Utils.ResponseObjects;
using Gridify;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
namespace FUEL_DISPATCH_API.DataAccess.Repository.Implementations
{
    public class ModEngineServices(FUEL_DISPATCH_DBContext DBContext, IHttpContextAccessor httpContextAccessor) : GenericRepository<ModEngine>(DBContext, httpContextAccessor), IModEngineServices
    {
        private readonly FUEL_DISPATCH_DBContext _DBContext = DBContext;

        public override ResultPattern<Paging<ModEngine>> GetAll(GridifyQuery query)
        {

            if (query.PageSize == 0)
                query.PageSize = int.MaxValue;

            if (query.Page == 0)
                query.Page = 1;
            var entities = _DBContext.ModEngine
                .AsNoTrackingWithIdentityResolution()
                .ApplyFilteringOrderingPaging(query);

            var totalItems = _DBContext.ModEngine
                .AsNoTrackingWithIdentityResolution()
                .ToList();

            var responseEntities = new Paging<ModEngine>
            {
                Data = entities,
                Count = totalItems.Count
            };

            return ResultPattern<Paging<ModEngine>>.Success(responseEntities,
                    StatusCodes.Status200OK,
                    AppConstants.DATA_OBTAINED_MESSAGE);
        }
        public override ResultPattern<ModEngine> Post(ModEngine entity)
        {
            var modEngineExist = _DBContext.ModEngine.FirstOrDefault(x => x.Name == entity.Name);
            if (modEngineExist is not null)
                throw new BadHttpRequestException("Un motor con este nombre ya existe. ");

            return base.Post(entity);
        }
    }
}
