using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Web.Http.ValueProviders.Providers;
namespace FUEL_DISPATCH_API.Reporting.Repository
{
    public class CrystalReport
    {
        public static HttpResponseMessage RenderReport(
            DateTime fromDate,
            DateTime toDate,
            string exportFilename = null)
        {
            var rd = new ReportDocument();
            string connStr = ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString;
            rd.Load(Path
                .Combine(System.Web.Hosting.HostingEnvironment.MapPath("~/Reports"), "SalidaRpt.rpt"));

            var builder = new SqlConnectionStringBuilder(connStr);

            // Modelo necesario para la coneccion a la base de datos. 
            ConnectionInfo connectionInfo = new ConnectionInfo
            {
                ServerName = builder.DataSource,
                DatabaseName = builder.InitialCatalog,
                UserID = builder.UserID,
                Password = builder.Password,

            };
            foreach (Table table in rd.Database.Tables)
            {
                TableLogOnInfo logOnInfo = table.LogOnInfo;
                logOnInfo.ConnectionInfo = connectionInfo;
                table.ApplyLogOnInfo(logOnInfo);
            };

            rd.SetParameterValue("FromDate", fromDate);
            rd.SetParameterValue("ToDate", toDate);
            MemoryStream ms = new MemoryStream();
            using (var stream = rd.ExportToStream(ExportFormatType.PortableDocFormat))
            {
                stream.CopyTo(ms);
            }
            var result = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(ms.ToArray())
            };
            result.Content.Headers.ContentDisposition =
                new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
                {
                    FileName = exportFilename ?? "Report.pdf"
                };
            result.Content.Headers.ContentType =
                new System.Net.Http.Headers.MediaTypeHeaderValue("application/pdf");
            return result;
        }

        public static HttpResponseMessage GetMaintenanceReport(int maintenanceId, string exportFileName = null)
        {
            var rd = new ReportDocument();

            string connStr = ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString;

            rd.Load(Path
                .Combine(System.Web.Hosting.HostingEnvironment.MapPath("~/Reports"), "Report1.rpt"));

            var builder = new SqlConnectionStringBuilder(connStr);

            // Modelo necesario para la coneccion a la base de datos. 
            ConnectionInfo connectionInfo = new ConnectionInfo
            {
                ServerName = builder.DataSource,
                DatabaseName = builder.InitialCatalog,
                UserID = builder.UserID,
                Password = builder.Password,

            };
            foreach (Table table in rd.Database.Tables)
            {
                TableLogOnInfo logOnInfo = table.LogOnInfo;
                logOnInfo.ConnectionInfo = connectionInfo;
                table.ApplyLogOnInfo(logOnInfo);
            };

            rd.SetParameterValue("MaintenanceId", Convert.ToInt32(maintenanceId));
            MemoryStream ms = new MemoryStream();
            using (var stream = rd.ExportToStream(ExportFormatType.PortableDocFormat))
            {
                stream.CopyTo(ms);
            }
            var result = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(ms.ToArray())
            };
            result.Content.Headers.ContentDisposition =
                new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
                {
                    FileName = exportFileName ?? "MaintenanceRpt.pdf"
                };
            result.Content.Headers.ContentType =
                new System.Net.Http.Headers.MediaTypeHeaderValue("application/pdf");
            return result;
        }
    }
}