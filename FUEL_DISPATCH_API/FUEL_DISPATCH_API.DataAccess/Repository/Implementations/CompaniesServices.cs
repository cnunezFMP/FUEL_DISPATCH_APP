using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.DataAccess.Repository.GenericRepository;
using FUEL_DISPATCH_API.DataAccess.Repository.Interfaces;
using FUEL_DISPATCH_API.Utils.Exceptions;
using FUEL_DISPATCH_API.Utils.ResponseObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

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
            // TODO: Buscar porque esta forma no funciona 
            //var company = _DBContext
            //    .Companies
            //    .FirstOrDefault(x => x.Id == companyId)
            //    ?? throw new NotFoundException("This company doesn't exist. ");

            //if (!company.BranchOffices!.Any())
            //    throw new BadRequestException("This company don't have branch offices. ");

            //return ResultPattern<List<BranchOffices>>.Success
            //(
            //    company.BranchOfficess,
            //    StatusCodes.Status200OK,
            //    "All company branch offices obtained. "
            //);

            var companyBranchOffices = _DBContext.BranchOffices
                .AsNoTracking()
                .Where(x => x.CompanyId == companyId)
                .ToList();

            if (!companyBranchOffices.Any())
                throw new BadRequestException("This company don't have branch offices. ");

            return ResultPattern<List<BranchOffices>>.Success
            (
                companyBranchOffices,
                StatusCodes.Status200OK,
                "All company branch offices obtained. "
            );
        }
        //public ResultPattern<Companies> GetCompanyByRnc(string companyRNC)
        //{
        //    var companyByRnc = _DBContext.Companies.FirstOrDefault(x => x.CompanyRNC == companyRNC)
        //        ?? throw new NotFoundException("No company with this RNC. ");
        //    return ResultPattern<Companies>.Success(companyByRnc, 
        //        StatusCodes.Status200OK, 
        //        "Company obtained. ");
        //}
        public bool IsCompanyUnique(Companies company)
            => !_DBContext.Companies.Any(x => x.CompanyRNC == company.CompanyRNC);
    }
}
