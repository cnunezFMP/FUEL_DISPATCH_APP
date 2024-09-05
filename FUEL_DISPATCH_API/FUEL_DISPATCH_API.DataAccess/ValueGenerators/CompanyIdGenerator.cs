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
                ?? throw new BadHttpRequestException("Al parecer el usuario no esta en una compañia. ");


            return int.Parse(companyId);
        }
    }
}