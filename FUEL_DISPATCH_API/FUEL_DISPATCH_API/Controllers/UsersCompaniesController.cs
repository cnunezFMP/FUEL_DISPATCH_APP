using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.DataAccess.Repository.Interfaces;
using FUEL_DISPATCH_API.Utils.ResponseObjects;
using Microsoft.AspNetCore.Mvc;

namespace FUEL_DISPATCH_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersCompaniesController : ControllerBase
    {
        private readonly ICompaniesUsersServices _companiesUsersServices;

        public UsersCompaniesController(ICompaniesUsersServices companiesUsersServices)
        {
            _companiesUsersServices = companiesUsersServices;
        }

        [HttpDelete("{userId}/Company/{companyId}")/*, Authorize(Roles = "Administrator")*/]
        public ActionResult<ResultPattern<User>> DeleteUserCompany(int userId, int companyId)
        {
            return Ok(_companiesUsersServices.DeleteUserCompany(userId, companyId));
        }

        [HttpPut("{userId}/Company/{companyId}")/*, Authorize(Roles = "Administrator")*/]
        public ActionResult<ResultPattern<User>> UpdateUserCompany(int userId, int companyId)
        {
            return Ok(_companiesUsersServices.UpdateUserCompany(userId, companyId));
        }
    }
}
