using FluentValidation;
using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.DataAccess.Repository.GenericRepository;
using FUEL_DISPATCH_API.DataAccess.Repository.Interfaces;
using FUEL_DISPATCH_API.Utils.Exceptions;
using FUEL_DISPATCH_API.Utils.ResponseObjects;
using Microsoft.AspNetCore.Http;
namespace FUEL_DISPATCH_API.DataAccess.Repository.Implementations
{
    public class ArticleDataMasterServices : GenericRepository<ArticleDataMaster>, IArticleServices
    {
        private readonly FUEL_DISPATCH_DBContext _DBContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ArticleDataMasterServices(FUEL_DISPATCH_DBContext dbContext, IHttpContextAccessor httpContextAccessor)
            : base(dbContext, httpContextAccessor)
        {
            _DBContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
        }

        public ResultPattern<ArticleDataMaster> GetByCode(string code, string companyId)
        {
            var article = _DBContext.ArticleDataMaster
                .FirstOrDefault(x => x.ArticleNumber == code &&
                x.CompanyId == Convert.ToInt32(companyId));

            if (article is null)
                throw new NotFoundException("No article find for this code. ");

            return ResultPattern<ArticleDataMaster>.Success(article, StatusCodes.Status200OK, "Article obtained succesfully. ");
        }
        public override ResultPattern<ArticleDataMaster> Post(ArticleDataMaster newArticle)
        {
            _DBContext.ArticleDataMaster.Add(newArticle!);
            _DBContext.SaveChanges();
            return ResultPattern<ArticleDataMaster>.Success(newArticle!, StatusCodes.Status201Created, "Article saved succesfully. ");
        }
        public bool IsArticleUnique(ArticleDataMaster articleDataMaster)
        {
            string? companyId, branchId;
            GetUserCompanyAndBranchClass.GetUserCompanyAndBranch(out companyId, out branchId);

            return !_DBContext.ArticleDataMaster
                .Where(x => x.CompanyId == Convert.ToInt32(companyId))
                .Any(x => x.ArticleNumber == articleDataMaster.ArticleNumber);

        }

        public ResultPattern<ArticleDataMaster> GetByCode(string code)
        {

            var article = _DBContext.ArticleDataMaster
                .FirstOrDefault(x => x.ArticleNumber == code);

            if (article is null)
                throw new NotFoundException("No article find for this code. ");

            return ResultPattern<ArticleDataMaster>.Success(article, StatusCodes.Status200OK, "Article obtained succesfully. ");
        }
    }
}
