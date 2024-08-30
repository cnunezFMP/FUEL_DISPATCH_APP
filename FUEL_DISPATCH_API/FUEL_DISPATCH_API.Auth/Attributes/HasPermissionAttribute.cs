using Microsoft.AspNetCore.Authorization;
namespace FUEL_DISPATCH_API.Auth.Attributes
{
    public sealed class HasPermissionAttribute(string permission)
        : AuthorizeAttribute(permission)
    {
    }
}
