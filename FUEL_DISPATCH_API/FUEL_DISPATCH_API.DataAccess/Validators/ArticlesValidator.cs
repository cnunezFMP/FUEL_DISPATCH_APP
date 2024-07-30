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
                .Must(x => x > 0)
                .WithMessage("El precio unitario no se puede enviar vacio. ");
            RuleFor(x => x.ArticleNumber)
                .Must((artNumber, _) =>
            {
                return articlesServices.IsArticleUnique(artNumber);
            }).WithMessage("An article with this code already exist. ");

        }
    }
}