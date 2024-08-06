using FUEL_DISPATCH_API.DataAccess.Models.SAP;
using FUEL_DISPATCH_API.DataAccess.Repository.Interfaces;
using FUEL_DISPATCH_API.Utils.ResponseObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUEL_DISPATCH_API.DataAccess.Repository.Implementations
{
    public class SAPSLServices : ISAPSLServices
    {
        private readonly HttpClient _httpClient;
        public SAPSLServices(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public bool Login(SapUserModel sapUserModel)
        {
            //_httpClient.PostAsync("https://localhost:50000/b1s/v1/Login", )
            return false;
        }
    }
}
