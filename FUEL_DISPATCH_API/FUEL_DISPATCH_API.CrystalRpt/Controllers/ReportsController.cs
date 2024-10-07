using FUEL_DISPATCH_API.CrystalRpt.Attributes;
using FUEL_DISPATCH_API.Reporting.Repository;
using System;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http;
namespace FUEL_DISPATCH_API.Reporting.Controllers
{
    [RoutePrefix("api/Reports")]
    public class ReportsController : ApiController
    {
        [HttpGet]
        [ClientCacheWithEtag(60)]  //1 min client side caching
        public HttpResponseMessage ExitRpt(DateTime fromDate, DateTime toDate)
        {
            HttpResponseMessage result = CrystalReport.RenderReport(fromDate, toDate);
            return result;
        }

        [HttpGet, Route("MaintenanceRpt")]
        [ClientCacheWithEtag(60)]
        public HttpResponseMessage MaintenanceRpt(int maintenanceId)
        {
            HttpResponseMessage result = CrystalReport.GetMaintenanceReport(maintenanceId);
            return result;
        }
        //[AllowAnonymous]
        //[Route("Financial/VarianceAnalysisReport")]
        //[HttpGet]
        //[ClientCacheWithEtag(60)]  //1 min client side caching
        //public HttpResponseMessage FinancialVarianceAnalysisReport()
        //{
        //    string reportPath = "~/Reports/Financial";
        //    string reportFileName = "YTDVarianceCrossTab.rpt";
        //    string exportFilename = "YTDVarianceCrossTab.pdf";

        //    HttpResponseMessage result = CrystalReport.RenderReport(reportPath, reportFileName, exportFilename);
        //    return result;
        //}

        //[AllowAnonymous]
        //[Route("VersatileandPrecise/Invoice")]
        //[HttpGet]
        //[ClientCacheWithEtag(60)]  //1 min client side caching
        //public HttpResponseMessage VersatileandPreciseInvoice()
        //{
        //    string reportPath = "~/Reports/VersatileandPrecise";
        //    string reportFileName = "Invoice.rpt";
        //    string exportFilename = "Invoice.pdf";

        //    HttpResponseMessage result = CrystalReport.RenderReport(reportPath, reportFileName, exportFilename);
        //    return result;
        //}

        //[AllowAnonymous]
        //[Route("VersatileandPrecise/FortifyFinancialAllinOneRetirementSavings")]
        //[HttpGet]
        //[ClientCacheWithEtag(60)]  //1 min client side caching
        //public HttpResponseMessage VersatileandPreciseFortifyFinancialAllinOneRetirementSavings()
        //{
        //    string reportPath = "~/Reports/VersatileandPrecise";
        //    string reportFileName = "FortifyFinancialAllinOneRetirementSavings.rpt";
        //    string exportFilename = "FortifyFinancialAllinOneRetirementSavings.pdf";

        //    HttpResponseMessage result = CrystalReport.RenderReport(reportPath, reportFileName, exportFilename);

        //    return result;
        //}
    }
}
