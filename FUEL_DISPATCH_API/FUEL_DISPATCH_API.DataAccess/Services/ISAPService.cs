using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.DataAccess.Models.SAP;
using FUEL_DISPATCH_API.DataAccess.Repository.Interfaces;
using FUEL_DISPATCH_API.Utils.Exceptions;
using Microsoft.AspNetCore.Http;
using RestSharp;
using RestSharp.Serializers.Json;
using System.ComponentModel.Design;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
namespace FUEL_DISPATCH_API.DataAccess.Services
{
    public interface ISAPService
    {
        Task PostGenExit(WareHouseMovement whsMovement);
        Task<dynamic> GetWarehouseSAP(string id);
        Task<dynamic> GetItemsSAP(string id);
        Task<WarehouseItemStock?> GetItemsStockSAP(string whsCode, string[]? itemCodes = null);
    }

    public class SAPService : ISAPService
    {
        private static JsonSerializerOptions JsonSerializerOptions => new()
        {
            PropertyNamingPolicy = null,
            PropertyNameCaseInsensitive = true,
            ReferenceHandler = ReferenceHandler.IgnoreCycles,
            NumberHandling = JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString
        };
        private RestClient? _restClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ICompaniesServices _companiesService;
        private readonly FUEL_DISPATCH_DBContext _dbContext;
        public SAPService(IHttpContextAccessor contextAccessor,
                          ICompaniesServices companiesServices,
                          FUEL_DISPATCH_DBContext dbContext)
        {
            _httpContextAccessor = contextAccessor;
            _companiesService = companiesServices;
            _dbContext = dbContext;
        }
        private async Task<LoginResponse> Login(CompanySAPParams sapParams)
        {
            _restClient ??= new RestClient(sapParams.ServiceLayerURL, c => c.RemoteCertificateValidationCallback = (a, b, c, d) => true, configureSerialization: s => s.UseSystemTextJson(JsonSerializerOptions));

            var request = new RestRequest("/Login", Method.Post);
            request.AddJsonBody(new SapUserModel()
            {
                CompanyDB = sapParams.CompanyDB,
                Password = sapParams.Password,
                UserName = sapParams.UserName
            });
            var response = await _restClient.ExecuteAsync<LoginResponse>(request);
            if (response.IsSuccessful)
            {
                _restClient.AddDefaultHeader("Cookie", $"B1SESSION={response.Data?.SessionId}");
                return response.Data!;
            }
            var errorResponse = JsonSerializer.Deserialize<ErrorResponseModel>(response.Content ?? "", JsonSerializerOptions)
                ?? throw new BadRequestException("Invalid Response");

            throw new BadRequestException(errorResponse.Error?.Message?.Value ?? "Invalid Response");
        }
        public async Task PostGenExit(WareHouseMovement whsMovement)
        {
            var companyId = _httpContextAccessor
                .HttpContext?
                .Items["CompanyId"]?
                .ToString()
                ?? throw new BadRequestException("Invalid Company");

            var company = _companiesService.Get(x => x.Id == int.Parse(companyId))?.Data
                ?? throw new NotFoundException("Company not found");

            if (company.CompanySAPParams is null)
                throw new NotFoundException("Company connection params not set");

            var loginResponse = await Login(company.CompanySAPParams);

            var item = _dbContext.ArticleDataMaster
                .FirstOrDefault(x => x.Id == whsMovement.ItemId && x.CompanyId == int.Parse(companyId))
                ?? throw new NotFoundException("Article not found");

            var whs = _dbContext.WareHouse.FirstOrDefault(x => x.Id == whsMovement.WareHouseId && x.CompanyId == int.Parse(companyId))
                ?? throw new NotFoundException("Warehouse not found");

            var request = new RestRequest("/InventoryGenExits", Method.Post)
                .AddJsonBody(new SAPGenExit()
                {
                    DocDate = whsMovement.CreatedAt ?? DateTime.Now,
                    Comments = whsMovement.Notes,
                    DocumentLines =
                    [
                        new()
                        {
                            ItemCode = item.ArticleNumber,
                            Quantity = whsMovement.Qty,
                            WarehouseCode = whs.Code ?? ""
                        }
                    ]
                });

            var response = await _restClient!.ExecuteAsync(request);

            if (!response.IsSuccessful)
            {
                var errorResponse = JsonSerializer.Deserialize<ErrorResponseModel>(response.Content ?? "", JsonSerializerOptions)
                    ?? throw new BadRequestException("Invalid Response");
            }
        }
        public async Task<dynamic> GetWarehouseSAP(string id)
        {
            var companyId = _httpContextAccessor
                .HttpContext?
                .Items["CompanyId"]?
                .ToString()
                ?? throw new BadRequestException("Invalid Company");

            var company = _companiesService.Get(x => x.Id == int.Parse(companyId))?.Data
               ?? throw new NotFoundException("Company not found");

            if (company.CompanySAPParams is null)
                throw new NotFoundException("Company connection params not set");

            var loginResponse = await Login(company.CompanySAPParams);

            var request = new RestRequest($"/Warehouses('{id}')", Method.Get);

            var response = await _restClient!.ExecuteAsync(request);

            if (!response.IsSuccessful)
            {
                var errorResponse = JsonSerializer.Deserialize<ErrorResponseModel>(response.Content ?? "", JsonSerializerOptions)
                    ?? throw new BadRequestException("Invalid Response");

                throw new BadRequestException(errorResponse.Error?.Message?.Value ?? "Invalid Response");
            }

            return response.Content;

        }

        public async Task<dynamic> GetItemsSAP(string id)
        {
            var companyId = _httpContextAccessor
                .HttpContext?
                .Items["CompanyId"]?
                .ToString()
                ?? throw new BadRequestException("Invalid Company");

            var company = _companiesService.Get(x => x.Id == int.Parse(companyId))?.Data
               ?? throw new NotFoundException("Company not found");

            if (company.CompanySAPParams is null)
                throw new NotFoundException("Company connection params not set");

            var loginResponse = await Login(company.CompanySAPParams);

            var request = new RestRequest($"/Items('{id}')", Method.Get);

            var response = await _restClient!.ExecuteAsync(request);

            if (!response.IsSuccessful)
            {
                var errorResponse = JsonSerializer.Deserialize<ErrorResponseModel>(response.Content ?? "", JsonSerializerOptions)
                    ?? throw new BadRequestException("Invalid Response");

                throw new BadRequestException(errorResponse.Error?.Message?.Value ?? "Invalid Response");
            }

            return response.Content;
        }

        public async Task<WarehouseItemStock?> GetItemsStockSAP(string whsCode, string[]? itemCodes = null)
        {
            var companyId = _httpContextAccessor
                .HttpContext?
                .Items["CompanyId"]?
                .ToString()
                ?? throw new BadRequestException("Invalid Company");

            var company = _companiesService.Get(x => x.Id == int.Parse(companyId))?.Data
               ?? throw new NotFoundException("Company not found");

            if (company.CompanySAPParams is null)
                throw new NotFoundException("Company connection params not set");

            var loginResponse = await Login(company.CompanySAPParams);

            var queryBuilder = new StringBuilder($"/$crossjoin(Items, Items/ItemWarehouseInfoCollection)?$expand=Items($select=ItemCode,ItemName),Items/ItemWarehouseInfoCollection($select=InStock)&$filter=Items/ItemCode eq Items/ItemWarehouseInfoCollection/ItemCode and Items/ItemWarehouseInfoCollection/WarehouseCode eq '{whsCode}'");

            if (itemCodes is not null && itemCodes.Length > 0)
            {
                queryBuilder.Append(" and (");
                foreach (var item in itemCodes.Select((item, i) => new { i, item }))
                {
                    if (item.i > 0)
                        queryBuilder.Append($" or Items/ItemCode eq '{item.item}'");
                    else
                        queryBuilder.Append($"Items/ItemCode eq '{item.item}'");
                }
                queryBuilder.Append(")");
            }

            var request = new RestRequest(queryBuilder.ToString(), Method.Get);

            var response = await _restClient!.ExecuteAsync<WarehouseItemStock>(request);

            if (!response.IsSuccessful)
            {
                var errorResponse = JsonSerializer.Deserialize<ErrorResponseModel>(response.Content ?? "", JsonSerializerOptions)
                    ?? throw new BadRequestException("Invalid Response");

                throw new BadRequestException(errorResponse.Error?.Message?.Value ?? "Invalid Response");
            }

            return response.Data;
        }
    }
}
