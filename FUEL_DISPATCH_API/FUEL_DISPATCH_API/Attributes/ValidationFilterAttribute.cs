using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using FUEL_DISPATCH_API.Utils.ResponseObjects;
namespace FUEL_DISPATCH_API.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class ValidationFilterAttribute : Attribute, IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context) { }
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
                context.Result = new BadRequestObjectResult(
                    ResultPattern<object>.Failure(StatusCodes.Status400BadRequest, string.Join(" | ",
                    context.ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage))
           ));
        }
    }
}
