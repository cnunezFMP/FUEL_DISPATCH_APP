using FluentValidation;
using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.DataAccess.Repository.Interfaces;
namespace FUEL_DISPATCH_API.DataAccess.Validators
{
    public class ArticlesValidator : AbstractValidator<ArticleDataMaster>
    {
        public ArticlesValidator(IArticleServices articlesServices)
        {
            RuleFor(x => x.ArticleNumber)
                .NotNull()
                .NotEmpty();

            RuleFor(x => x.UnitPrice)
                .NotNull()
                .NotEmpty()
                .Must((x) => x > 0);

            RuleFor(x => x.ArticleNumber)
                .Must((artNumber, _) => articlesServices.IsArticleUnique(artNumber))
                .WithMessage("Ya existe un articulo con este codigo. ");
        }
    }
}