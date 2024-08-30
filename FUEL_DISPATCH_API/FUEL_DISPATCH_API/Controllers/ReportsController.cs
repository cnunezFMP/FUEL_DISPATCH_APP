using FUEL_DISPATCH_API.DataAccess.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Web.Http.Filters;
namespace FUEL_DISPATCH_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportsController : ControllerBase
    {
        private readonly IReportsServices _reportService;
        public ReportsController(IReportsServices reportsServics)
        {
            _reportService = reportsServics;
        }
        [HttpGet, Authorize(Roles = "Reportero")]
        public async Task<ActionResult> Get
            (
                DateTime fromDate,
                DateTime toDate,
                string? exportFileName
            )
        {
            try
            {
                byte[] report = await _reportService.GetReportAsync(fromDate, toDate, exportFileName);
                return File(report, "application/pdf", exportFileName ?? "Report.pdf");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
