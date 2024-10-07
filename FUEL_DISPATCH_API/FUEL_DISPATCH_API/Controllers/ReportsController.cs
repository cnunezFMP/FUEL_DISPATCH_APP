using FUEL_DISPATCH_API.DataAccess.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Web.Http.Filters;
namespace FUEL_DISPATCH_API.Controllers
{
    [ApiController, Route("api/[controller]"), Authorize/*(Roles = "CanGenerateReport, Administrador")*/]
    public class ReportsController : ControllerBase
    {
        private readonly IReportsServices _reportService;
        public ReportsController(IReportsServices reportsServics)
        {
            _reportService = reportsServics;
        }
        [HttpGet, Authorize]
        public async Task<ActionResult> Get(DateTime fromDate, DateTime toDate, string? exportFileName)
        {
            try
            {
                byte[] report = await _reportService.GetReportAsync(fromDate, toDate, exportFileName);
                return File(report, "application/pdf", exportFileName ?? "SalidaRpt.pdf");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("MaintenanceRpt"), Authorize]
        public async Task<ActionResult> GetMaintenanceRpt(int maitenanceId, string? exportFileName)
        {
            try
            {
                byte[] report = await _reportService.GetMaintenanceRptAsync(maitenanceId, exportFileName);
                return File(report, "application/pdf", exportFileName ?? "MaitenanceRpt.pdf");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
