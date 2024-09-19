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
    public class MakeServices(FUEL_DISPATCH_DBContext dbContext, IHttpContextAccessor httpContextAccessor)
        : GenericRepository<Make>(dbContext, httpContextAccessor), IMakeServices
    {
        private readonly FUEL_DISPATCH_DBContext _DBContext = dbContext;
        public override ResultPattern<Paging<Make>> GetAll(GridifyQuery query)
        {
            if (query.PageSize == 0)
                query.PageSize = int.MaxValue;

            if (query.Page == 0)
                query.Page = 1;
            var entities = _DBContext.Make
                .AsNoTrackingWithIdentityResolution()
                .ApplyFilteringOrderingPaging(query);

            var totalItems = _DBContext.Make
                .AsNoTrackingWithIdentityResolution()
                .ToList();

            var responseEntities = new Paging<Make>
            {
                Data = entities,
                Count = totalItems.Count
            };

            return ResultPattern<Paging<Make>>.Success(responseEntities,
                    StatusCodes.Status200OK,
                    AppConstants.DATA_OBTAINED_MESSAGE);
        }

        public override ResultPattern<Make> Post(Make entity)
        {
            var makeExist = _DBContext.Make.FirstOrDefault(x => x.Name == entity.Name);
            if (makeExist is not null)
                throw new BadHttpRequestException("Una marca con este nombre ya existe. ");

            return base.Post(entity);
        }
    }
}
