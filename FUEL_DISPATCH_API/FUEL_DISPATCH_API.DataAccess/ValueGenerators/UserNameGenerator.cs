using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using System.Security.Claims;
namespace FUEL_DISPATCH_API.DataAccess.ValueGenerators
{
    public class UserNameGenerator : ValueGenerator<string>
    {
        public override bool GeneratesTemporaryValues =>
            false;
        public override string Next(EntityEntry entry) =>
            entry.Context
            .GetService<IHttpContextAccessor>()?
            .HttpContext?
            .Items["UserName"]
            as string ?? "System";
    }
}
