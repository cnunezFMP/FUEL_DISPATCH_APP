using FUEL_DISPATCH_API.Utils.Exceptions;
using FUEL_DISPATCH_API.Utils.ResponseObjects;
using System.Net;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace FUEL_DISPATCH_API.CustomAttributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class CustomAuthorizedAttribute : Attribute
    {
        public CustomAuthorizedAttribute(HttpContext httpContext)
        {
            var user = httpContext.User;
            if (user?.Identity?.IsAuthenticated is false)
                throw new UnauthorizedException("You are unauthorized to access this resource");
        }
    }
}
