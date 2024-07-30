using FUEL_DISPATCH_API.DataAccess.Models;
using Swashbuckle.AspNetCore.Filters;

namespace FUEL_DISPATCH_API.Swagger.SwaggerExamples
{
    public class ArticleSwaggerExample : IExamplesProvider<ArticleDataMaster>
    {
        public ArticleDataMaster GetExamples()
        {
            return new ArticleDataMaster
            {
                ArticleNumber = "IT-001",
                Description = "Alguna descripcion del articulo. ",
                UnitPrice = 100,
                Manufacturer = "Shell",
                BarCode = "00010001",
                CompanyId = 1
            };
        }
    }
}
