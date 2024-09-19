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
    public class GenerationServices(FUEL_DISPATCH_DBContext dbContext, IHttpContextAccessor httpContextAccessor) : GenericRepository<Generation>(dbContext, httpContextAccessor), IGenerationServices
    {
        private readonly FUEL_DISPATCH_DBContext _DBContext = dbContext;
        public override ResultPattern<Paging<Generation>> GetAll(GridifyQuery query)
        {

            if (query.PageSize == 0)
                query.PageSize = int.MaxValue;

            if (query.Page == 0)
                query.Page = 1;

            var entities = _DBContext.Generation
                .AsNoTrackingWithIdentityResolution()
                .ApplyFilteringOrderingPaging(query);

            var totalItems = _DBContext.Generation
                .AsNoTrackingWithIdentityResolution()
                .ToList();

            var responseEntities = new Paging<Generation>
            {
                Data = entities,
                Count = totalItems.Count
            };

            return ResultPattern<Paging<Generation>>.Success(responseEntities,
                    StatusCodes.Status200OK,
                    AppConstants.DATA_OBTAINED_MESSAGE);
        }

        public override ResultPattern<Generation> Post(Generation entity)
        {
            var genExist = _DBContext.Generation.FirstOrDefault(x => x.Name == entity.Name);
            if (genExist is not null)
                throw new BadHttpRequestException("Una generacion con este nombre ya existe. ");

            return base.Post(entity);
        }
    }
}
