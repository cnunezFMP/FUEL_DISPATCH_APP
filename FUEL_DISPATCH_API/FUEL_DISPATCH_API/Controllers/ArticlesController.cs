using FluentValidation;
using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.DataAccess.Repository.Interfaces;
using FUEL_DISPATCH_API.Utils.ResponseObjects;
using Gridify;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FUEL_DISPATCH_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ArticlesController : ControllerBase
    {
        private readonly IArticleServices _articleServices;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IValidator<ArticleDataMaster> _validator;
        public ArticlesController(IArticleServices articleServices,
                                  IValidator<ArticleDataMaster> validator,
                                  IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _articleServices = articleServices;
            _validator = validator;
        }
        [HttpGet, Authorize]
        public ActionResult<ResultPattern<Paging<ArticleDataMaster>>> GetArticles([FromQuery] GridifyQuery query)
        {
            return Ok(_articleServices.GetAll(query));
        }

        [HttpGet("{id:int}"), Authorize]
        public ActionResult<ResultPattern<ArticleDataMaster>> GetArticle(int id)
        {
            string? companyId, branchId;
            companyId = _httpContextAccessor.HttpContext?.Items["CompanyId"]?.ToString();
            branchId = _httpContextAccessor.HttpContext?.Items["BranchOfficeId"]?.ToString();
            bool predicate(ArticleDataMaster x) => x.Id == id &&
                                                   x.CompanyId == int.Parse(companyId);
            return Ok(_articleServices.Get(predicate));
        }

        /// <summary>
        /// Crear un nueva parte de vehiculo. 
        /// </summary>
        /// <param name="article"></param>
        /// <response code="201">Si se crea el articulo correctamente. </response>
        /// <response code="400">Si se intenta agregar un articulo con el codigo de una ya existente. </response>
        /// <response code="400">Si se envia el numero de articulo nulo. </response>
        /// <returns></returns>
        [HttpPost, Authorize]
        public ActionResult<ResultPattern<ArticleDataMaster>> PostArticle([FromBody] ArticleDataMaster article)
        {
            //var validationResult = _validator.Validate(article);
            //if (!validationResult.IsValid)
            //{
            //    return ValidationProblem(ModelStateResult.GetModelStateDic(validationResult));
            //}
            return Created(string.Empty, _articleServices.Post(article));
        }
        [HttpPut("{id:int}"), Authorize]
        public ActionResult<ResultPattern<Part>> UpdateArticle(int id, [FromBody] ArticleDataMaster article)
        {
            //var validationResult = _validator.Validate(article);
            //if (!validationResult.IsValid)
            //{
            //    return ValidationProblem(ModelStateResult.GetModelStateDic(validationResult));
            //}
            string? companyId, branchId;
            companyId = _httpContextAccessor.HttpContext?.Items["CompanyId"]?.ToString();
            branchId = _httpContextAccessor.HttpContext?.Items["BranchOfficeId"]?.ToString();
            bool predicate(ArticleDataMaster x) => x.Id == id &&
                x.CompanyId == int.Parse(companyId);

            return Ok(_articleServices.Update(predicate, article));
        }
    }
}