using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Configuration;
using RestSharp;
using System.Runtime.InteropServices;

namespace FUEL_DISPATCH_API.DataAccess.Services
{
    public interface IReportsServices
    {
        Task<byte[]> GetReportAsync(DateTime fromDate,
            DateTime toDate,
            string? exportFileName);

        Task<byte[]> GetMaintenanceRptAsync(int maintenanceId, string? exportFileName);
    }
    public class ReportsServices : IReportsServices
    {
        private RestClient? _client;
        private readonly IConfiguration configuration;
        public ReportsServices(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public async Task<byte[]> GetReportAsync(DateTime fromDate, DateTime toDate, string? exportFileName)
        {
            string? reportsApiBaseUrl = configuration.GetValue<string>("reportsApiUrl:baseUrl");
            _client = new RestClient(reportsApiBaseUrl!);
            var request = new RestRequest($"api/Reports", Method.Get);
            request.AddParameter("fromDate", fromDate.ToString("yyyy-MM-dd"));
            request.AddParameter("toDate", toDate.ToString("yyyy-MM-dd"));
            request.AddParameter("exportFileName", exportFileName);
            var response = await _client.ExecuteAsync(request);
            if (response.IsSuccessful)
            {
                return response.RawBytes;
            }
            else
            {
                throw new Exception($"Error: {response.ErrorMessage}");
            }
        }
        public async Task<byte[]> GetMaintenanceRptAsync(int maintenanceId, string? exportFileName)
        {
            string? reportsApiBaseUrl = configuration.GetValue<string>("reportsApiUrl:baseUrl");
            _client = new RestClient(reportsApiBaseUrl!);
            var request = new RestRequest($"api/Reports/MaintenanceRpt", Method.Get);
            request.AddParameter("maintenanceId", maintenanceId);
            request.AddParameter("exportFileName", exportFileName);
            var response = await _client.ExecuteAsync(request);
            if (response.IsSuccessful)
            {
                return response.RawBytes;
            }
            else
            {
                throw new Exception($"Error: {response.ErrorMessage}");
            }
        }
    }
}
