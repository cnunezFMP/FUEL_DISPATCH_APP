using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestSharp;

namespace FUEL_DISPATCH_API.DataAccess.Services
{
    public interface IReportsServices
    {
        Task<byte[]> GetReportAsync(DateTime fromDate, DateTime toDate, string? exportFileName);
    }

    public class ReportsServices : IReportsServices
    {
        private readonly RestClient _client;

        public ReportsServices()
        {
            _client = new RestClient("https://localhost:44338/");
        }

        public async Task<byte[]> GetReportAsync(DateTime fromDate, DateTime toDate, string? exportFileName)
        {
            var request = new RestRequest("api/Reports", Method.Get);
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
    }
}
