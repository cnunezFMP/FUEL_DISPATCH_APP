using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.DataAccess.Repository.GenericRepository;
using FUEL_DISPATCH_API.DataAccess.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUEL_DISPATCH_API.DataAccess.Repository.Implementations
{
    public class DispenserServices : GenericRepository<Dispenser>, IDispenserServices
    {
        private readonly FUEL_DISPATCH_DBContext _DBContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public DispenserServices(FUEL_DISPATCH_DBContext dbContext, IHttpContextAccessor httpContextAccessor)
            : base(dbContext, httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _DBContext = dbContext;
        }

        public bool DispenserCodeMustBeUnique(Dispenser dispenser)
        {
            string? companyId, branchId;
            companyId = _httpContextAccessor.HttpContext?.Items["CompanyId"]?.ToString();
            branchId = _httpContextAccessor.HttpContext?.Items["BranchOfficeId"]?.ToString();

            return !(from t0 in _DBContext.Dispenser
                    join t1 in _DBContext.BranchOffices on t0.BranchOfficeId equals int.Parse(branchId)
                    join t2 in _DBContext.Companies on t1.CompanyId equals int.Parse(companyId)
                    join t3 in _DBContext.BranchIslands on t0.BranchIslandId equals t3.Id
                    where t0.Code == dispenser.Code
                    select t0)
                    .AsNoTracking()
                   .Any();
        }

    }
}
