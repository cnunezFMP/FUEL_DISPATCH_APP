using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

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
                as string ?? string.Empty;

            return int.Parse(companyId);
        }
    }
}
