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
    public class UserNameGenerator : ValueGenerator<string>
    {
        public override bool GeneratesTemporaryValues =>
            false;
        public override string Next(EntityEntry entry) =>
            entry.Context.GetService<IHttpContextAccessor>()
            .HttpContext?
            .User
            .Claims?
            .FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?
            .Value ?? "System";

    }
}
