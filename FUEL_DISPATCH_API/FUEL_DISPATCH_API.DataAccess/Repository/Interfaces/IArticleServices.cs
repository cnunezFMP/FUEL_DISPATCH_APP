using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.DataAccess.Repository.GenericRepository;
using FUEL_DISPATCH_API.Utils.ResponseObjects;
using System;
using System.Linq;

namespace FUEL_DISPATCH_API.DataAccess.Repository.Interfaces
{
    public interface IArticleServices : IGenericInterface<ArticleDataMaster>
    {
        ResultPattern<ArticleDataMaster> GetByCode(string code);
        bool IsArticleUnique(ArticleDataMaster articleDataMaster);
    }
}
