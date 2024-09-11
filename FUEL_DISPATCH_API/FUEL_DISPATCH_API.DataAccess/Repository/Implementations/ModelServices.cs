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
    public class ModelServices(FUEL_DISPATCH_DBContext DBContext, IHttpContextAccessor httpContextAccessor) : GenericRepository<Model>(DBContext, httpContextAccessor), IModelServices
    {
        private readonly FUEL_DISPATCH_DBContext _DBContext = DBContext;
        public override ResultPattern<Paging<Model>> GetAll(GridifyQuery query)
        {
            if (query.PageSize == 0)
                query.PageSize = int.MaxValue;

            if (query.Page == 0)
                query.Page = 1;

            var entities = _DBContext.Model
                .AsNoTrackingWithIdentityResolution()
                .ApplyFilteringOrderingPaging(query);

            var totalItems = _DBContext.Model
                .AsNoTrackingWithIdentityResolution()
                .ToList();

            var responseEntities = new Paging<Model>
            {
                Data = entities,
                Count = totalItems.Count
            };

            return ResultPattern<Paging<Model>>.Success(responseEntities,
                    StatusCodes.Status200OK,
                    AppConstants.DATA_OBTAINED_MESSAGE);

        }
    }
}
