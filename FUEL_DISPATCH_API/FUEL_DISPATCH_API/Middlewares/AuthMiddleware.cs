using FUEL_DISPATCH_API.Utils.Exceptions;
using System.Security.Claims;
using System.Security.Cryptography;

namespace FUEL_DISPATCH_API.Middlewares
{
    public class AuthMiddleware
    {
        private readonly RequestDelegate _next;
        public AuthMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            // DONE: Validar los tokens. 
            var token = context.Request
                .Headers
                .Authorization
                .FirstOrDefault()?
                .Split(" ")
                .Last() ??
                " ";

            var user = context.User;

            if (user.Identity?.IsAuthenticated ??
                false)
            {
                string? companyId, branchId;
                companyId = user.Claims
                    .FirstOrDefault(x => x.Type == "CompanyId")?
                    .Value ??
                    " ";

                branchId = user.Claims
                    .FirstOrDefault(x => x.Type == "BranchOfficeId")?
                    .Value
                    ?? " ";

                /*nameid = user.Claims
                    .FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?
                    .Value
                    ?? " ";*/

                context.Items["CompanyId"] = companyId;
                context.Items["BranchOfficeId"] = branchId;
                //context.Items["nameid"] = nameid;

            }
            await _next(context);
        }
    }
}
