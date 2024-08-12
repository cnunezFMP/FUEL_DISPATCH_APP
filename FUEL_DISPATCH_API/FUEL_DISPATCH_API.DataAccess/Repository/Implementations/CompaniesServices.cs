using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.DataAccess.Repository.GenericRepository;
using FUEL_DISPATCH_API.DataAccess.Repository.Interfaces;
using FUEL_DISPATCH_API.Utils.Exceptions;
using FUEL_DISPATCH_API.Utils.ResponseObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
namespace FUEL_DISPATCH_API.DataAccess.Repository.Implementations
{
    public class CompaniesServices : GenericRepository<Companies>, ICompaniesServices
    {
        private readonly FUEL_DISPATCH_DBContext _DBContext;
        public CompaniesServices(FUEL_DISPATCH_DBContext dbContext, IHttpContextAccessor httpContextAccessor)
            : base(dbContext, httpContextAccessor)
        {
            _DBContext = dbContext;
        }
        public ResultPattern<ICollection<BranchOffices>> GetCompanyBranchOfficess(int companyId)
        {
            // DONE: Buscar porque esta forma no funciona
            var company = _DBContext
                .Companies
                .Include(x => x.BranchOffices)
                .FirstOrDefault(x => x.Id == companyId)
                ?? throw new NotFoundException("This company doesn't exist. ");

            if (!company.BranchOffices!.Any())
                throw new BadRequestException("This company don't have branch offices. ");

            return ResultPattern<ICollection<BranchOffices>>.Success
            (
                company.BranchOffices!,
                StatusCodes.Status200OK,
                "All company branch offices obtained. "
            );

            /*var companyBranchOffices = _DBContext.BranchOffices
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
            );*/
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
