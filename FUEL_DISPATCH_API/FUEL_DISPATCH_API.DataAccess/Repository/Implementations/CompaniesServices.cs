using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.DataAccess.Repository.GenericRepository;
using FUEL_DISPATCH_API.DataAccess.Repository.Interfaces;
using FUEL_DISPATCH_API.Utils.Exceptions;
using FUEL_DISPATCH_API.Utils.ResponseObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace FUEL_DISPATCH_API.DataAccess.Repository.Implementations
{
    public class CompaniesServices : GenericRepository<Companies>, ICompaniesServices
    {
        private readonly FUEL_DISPATCH_DBContext _DBContext;
        public CompaniesServices(FUEL_DISPATCH_DBContext dbContext)
            : base(dbContext)
        {
            _DBContext = dbContext;
        }

        public ResultPattern<List<BranchOffices>> GetCompanyBranchOfficess(int companyId)
        {
            var companyBranchOffices = _DBContext.BranchOffices
                .AsNoTracking()
                .Where(x => x.CompanyId == companyId)
                .ToList()
                ?? throw new NotFoundException("This company has no branch. ");

            return ResultPattern<List<BranchOffices>>
                .Success
                (
                    companyBranchOffices,
                    StatusCodes.Status200OK,
                    "All company branch offices obtained. "
                );
        }
        public ResultPattern<Companies> GetCompanyByRnc(string companyRNC)
        {
            var companyByRnc = _DBContext.Companies.FirstOrDefault(x => x.CompanyRNC == companyRNC)
                ?? throw new BadRequestException("No company with this RNC. ");
            return ResultPattern<Companies>.Success(companyByRnc, StatusCodes.Status200OK, "Company obtained. ");
        }
        public bool IsCompanyUnique(Companies company)
            => !_DBContext.Companies.Any(x => x.CompanyRNC == company.CompanyRNC);
    }
}
