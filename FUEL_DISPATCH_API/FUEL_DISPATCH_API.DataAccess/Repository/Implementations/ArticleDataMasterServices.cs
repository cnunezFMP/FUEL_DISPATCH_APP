using FluentValidation;
using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.DataAccess.Repository.GenericRepository;
using FUEL_DISPATCH_API.DataAccess.Repository.Interfaces;
using FUEL_DISPATCH_API.DataAccess.Services;
using FUEL_DISPATCH_API.Utils.Exceptions;
using FUEL_DISPATCH_API.Utils.ResponseObjects;
using Gridify;
using Microsoft.AspNetCore.Http;
namespace FUEL_DISPATCH_API.DataAccess.Repository.Implementations
{
    public class ArticleDataMasterServices : GenericRepository<ArticleDataMaster>, IArticleServices
    {
        private readonly FUEL_DISPATCH_DBContext _DBContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ISAPService _sapService;
        public ArticleDataMasterServices(FUEL_DISPATCH_DBContext dbContext, IHttpContextAccessor httpContextAccessor, ISAPService sAPService)
            : base(dbContext, httpContextAccessor)
        {
            _DBContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
            _sapService = sAPService;
        }
        public override ResultPattern<ArticleDataMaster> Post(ArticleDataMaster entity)
        {

            try
            {
                var getArticleTask = _sapService.GetItemsSAP(entity.ArticleNumber);
                getArticleTask.Wait();
            }
            catch (Exception ex)
            {
                entity.ArticleNumber = null;
                return ResultPattern<ArticleDataMaster>.Failure(
                    StatusCodes.Status400BadRequest,
                    ex.Message);
            }
            return base.Post(entity);
        }

        public bool IsArticleUnique(ArticleDataMaster articleDataMaster)
        {
            string? companyId;
            companyId = _httpContextAccessor.HttpContext?.Items["CompanyId"]?.ToString();
            return !_DBContext.ArticleDataMaster
                .Any((x) => x.ArticleNumber == articleDataMaster.ArticleNumber
                && x.CompanyId == int.Parse(companyId));
        }
        public ResultPattern<ArticleDataMaster> GetByCode(string code)
        {
            string? companyId;
            companyId = _httpContextAccessor.HttpContext?.Items["CompanyId"]?.ToString();

            var article = _DBContext.ArticleDataMaster
                .FirstOrDefault(x => x.ArticleNumber == code
                && x.CompanyId == int.Parse(companyId))
                ?? throw new NotFoundException("No article find for this code. ");

            return ResultPattern<ArticleDataMaster>.Success(article, StatusCodes.Status200OK, "Article obtained succesfully. ");
        }
    }
}
