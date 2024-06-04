using FMP_DISPATCH_API.Services.Emails;
using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.DataAccess.Repository.GenericRepository;
using FUEL_DISPATCH_API.DataAccess.Repository.Interfaces;
using FUEL_DISPATCH_API.Utils.Constants;
using FUEL_DISPATCH_API.Utils.Exceptions;
using FUEL_DISPATCH_API.Utils.ResponseObjects;
using Microsoft.AspNetCore.Http;

namespace FUEL_DISPATCH_API.DataAccess.Repository.Implementations
{
    public class DriverServices : GenericRepository<Drivers>, IDriverServices
    {
        private readonly FUEL_DISPATCH_DBContext _DBContext;
        private readonly IEmailSender _emailSender;
        public DriverServices(FUEL_DISPATCH_DBContext dBContext, IEmailSender emailSender)
            : base(dBContext)
        {
            _DBContext = dBContext;
            _emailSender = emailSender;
        }
        public override ResultPattern<Drivers> Post(Drivers entity)
        {
            if (!CheckIfIdIsUnique(entity))
                throw new BadRequestException("This identification exists. ");

            if (!IsEmailUnique(entity.Email))
                throw new BadRequestException("Email exists. ");

            _DBContext.Drivers.Add(entity);
            _DBContext.SaveChanges();
            _emailSender.SendEmailAsync(entity.Email, AppConstants.ACCOUNT_CREATED_MESSAGE, "Your account was created successfully. ");
            return ResultPattern<Drivers>.Success(entity, StatusCodes.Status200OK, "Driver added successfully. ");
        }
        bool CheckIfIdIsUnique(Drivers entity)
            => !_DBContext.Drivers.Any(x => x.Identification == entity.Identification);
        bool IsEmailUnique(string email)
            => !_DBContext.Users.Any(x => x.Email == email);
    }
}
