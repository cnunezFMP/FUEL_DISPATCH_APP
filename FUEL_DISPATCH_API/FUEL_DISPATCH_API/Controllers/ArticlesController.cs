using FluentValidation;
using FluentValidation.Results;
using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.DataAccess.Repository.Interfaces;
using FUEL_DISPATCH_API.DataAccess.Validators;
using FUEL_DISPATCH_API.Utils.ResponseObjects;
using Gridify;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Security.Claims;

namespace FUEL_DISPATCH_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ArticlesController : ControllerBase
    {
        private readonly IArticleServices _articleServices;
        private readonly IValidator<ArticleDataMaster> _validator;
        public ArticlesController(IArticleServices articleServices, IValidator<ArticleDataMaster> validator)
        {
            _articleServices = articleServices;
            _validator = validator;
        }
        [HttpGet, Authorize(Roles = "Administrator")]
        public ActionResult<ResultPattern<Paging<ArticleDataMaster>>> GetArticles([FromQuery] GridifyQuery query)
        {
            return Ok(_articleServices.GetAll(query));
        }

        [HttpGet("{id:int}"), Authorize(Roles = "Administrator")]
        public ActionResult<ResultPattern<ArticleDataMaster>> GetArticle(int id)
        {
            return Ok(_articleServices.Get(x => x.Id == id));
        }

        /// <summary>
        /// Create new article.
        /// Example:
        /// {
        ///     "articleNumber": "IT00001",
        ///     "description": "string",
        ///     "unitPrice": 0,
        ///     "maker": "string",
        ///     "barCode": "string",
        /// }
        /// </summary>
        /// <param name="article"></param>
        /// <returns></returns>
        [HttpPost, Authorize(Roles = "Administrator")]
        public ActionResult<ResultPattern<Driver>> PostArticle([FromBody] ArticleDataMaster article)
        {
            var validationResult = _validator.Validate(article);
            if (!validationResult.IsValid)
            {
                return ValidationProblem(ModelStateResult.GetModelStateDic(validationResult));
            }
            article.CreatedBy = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            article.UpdatedBy = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            return CreatedAtAction(nameof(GetArticle), new { id = article.Id }, _articleServices.Post(article));
        }

        [HttpPut("{id:int}"), Authorize(Roles = "Administrator")]
        public ActionResult<ResultPattern<User>> UpdateArticle(int id, [FromBody] ArticleDataMaster article)
        {
            article.UpdatedAt = DateTime.Now;
            article.UpdatedBy = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            return Ok(_articleServices.Update(x => x.Id == id, article));
        }
    }
}