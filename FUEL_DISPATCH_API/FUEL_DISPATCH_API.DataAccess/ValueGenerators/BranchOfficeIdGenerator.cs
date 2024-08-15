using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.ValueGeneration;
namespace FUEL_DISPATCH_API.DataAccess.ValueGenerators
{
    public class BranchOfficeIdGenerator : ValueGenerator<int>
    {
        public override bool GeneratesTemporaryValues =>
            false;
        public override int Next(EntityEntry entry)
        {
            var companyId = entry.Context.GetService<IHttpContextAccessor>()?
                .HttpContext?
                .Items["BranchOfficeId"] 
                as string ?? string.Empty;

            return int.Parse(companyId);
        }
    }
}
