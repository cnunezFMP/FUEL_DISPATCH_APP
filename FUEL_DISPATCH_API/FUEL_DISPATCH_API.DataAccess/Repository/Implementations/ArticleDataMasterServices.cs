using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.DataAccess.Repository.GenericRepository;
using FUEL_DISPATCH_API.DataAccess.Repository.Interfaces;
using FUEL_DISPATCH_API.Utils.Constants;
using FUEL_DISPATCH_API.Utils.Exceptions;
using FUEL_DISPATCH_API.Utils.ResponseObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUEL_DISPATCH_API.DataAccess.Repository.Implementations
{
    public class ArticleDataMasterServices : GenericRepository<ArticleDataMaster>, IArticleServices
    {
        private readonly FUEL_DISPATCH_DBContext _DBContext;
        public ArticleDataMasterServices(FUEL_DISPATCH_DBContext dbContext)
            : base(dbContext)
        {
            _DBContext = dbContext;
        }

        public ResultPattern<ArticleDataMaster> GetByCode(string code)
        {
            var article = _DBContext.ArticleDataMaster.Find(code);
            if (article is null)
                throw new NotFoundException("No article find for this code. ");
            return ResultPattern<ArticleDataMaster>.Success(article, StatusCodes.Status200OK, "Article obtained succesfully. ");
        }

        public override ResultPattern<ArticleDataMaster> Post(ArticleDataMaster newArticle)
        {
            if (!IsArticleUnique(newArticle))
                throw new BadRequestException("Article with same code exists. ");

            _DBContext.ArticleDataMaster.Add(newArticle!);
            _DBContext.SaveChanges();
            return ResultPattern<ArticleDataMaster>.Success(newArticle!, StatusCodes.Status201Created, "Article saved succesfully. ");
        }

        bool IsArticleUnique(ArticleDataMaster newArticle)
            => !_DBContext.ArticleDataMaster.Any(x => x.ArticleNumber == newArticle.ArticleNumber);
    }
}
