using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.ValueGeneration;
namespace FUEL_DISPATCH_API.DataAccess.ValueGenerators
{
    public class CompanyIdGenerator : ValueGenerator<int>
    {
        public override bool GeneratesTemporaryValues =>
            false;
        public override int Next(EntityEntry entry)
        {
            var companyId = entry.Context.GetService<IHttpContextAccessor>()?
                .HttpContext?
                .Items["CompanyId"]
                as string
                ?? string.Empty;

            if(!string.IsNullOrEmpty(companyId))
                return int.Parse(companyId);

            return 0;
        }
    }
}
