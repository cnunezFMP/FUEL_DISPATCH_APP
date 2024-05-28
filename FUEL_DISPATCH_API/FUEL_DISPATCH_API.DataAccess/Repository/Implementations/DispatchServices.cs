using FMP_DISPATCH_API.Services.Emails;
using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.DataAccess.Repository.GenericRepository;
using FUEL_DISPATCH_API.DataAccess.Repository.Interfaces;
using FUEL_DISPATCH_API.Utils.Constants;
using FUEL_DISPATCH_API.Utils.Exceptions;
using FUEL_DISPATCH_API.Utils.ResponseObjects;
using Gridify;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System.Linq.Expressions;

namespace FUEL_DISPATCH_API.DataAccess.Repository.Implementations
{
    public class DispatchServices : GenericRepository<Dispatch>, IDispatchServices
    {
        private readonly FUEL_DISPATCH_DBContext _DBContext;
        private readonly IEmailSender _emailSender;
        public DispatchServices(IGenericInterface<Dispatch> genericInterface, IEmailSender emailSender, FUEL_DISPATCH_DBContext dBContext)
            : base(dBContext)
        {
            //_genericInterface = genericInterface;
            _emailSender = emailSender;
            _DBContext = dBContext;
        }
    }
}
