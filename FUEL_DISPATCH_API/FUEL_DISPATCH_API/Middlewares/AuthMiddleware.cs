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
            var token = context.Request.Headers.Authorization.FirstOrDefault()?.Split(" ").Last() ?? " ";

            var user = context.User;

            if (user.Identity?.IsAuthenticated ??
                false)
            {
                string? /*companyId, branchId,*/ username;
                //companyId = user.Claims
                //    .FirstOrDefault(x => x.Type == "CompanyId")?
                //    .Value ??
                //    throw new BadRequestException("User is not in branch office. ");

                //branchId = user.Claims
                //    .FirstOrDefault(x => x.Type == "BranchOfficeId")?
                //    .Value
                //    ??
                //    throw new BadRequestException("User is not in branch office. ");

                username = user
                    .Claims
                    .FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value
                    ?? throw new BadRequestException("User don't has username. ");

                context.Items["UserName"] = username;
                //context.Items["CompanyId"] = companyId;
                //context.Items["BranchOfficeId"] = branchId;

            }
            await _next(context);
        }
    }
}
