using FluentValidation;
using FluentValidation.Results;
using FMP_DISPATCH_API.Services.Emails;
using FMP_MATEINANCE_API.Auth;
using FUEL_DISPATCH_API.DataAccess.DTOs;
using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.DataAccess.Repository.GenericRepository;
using FUEL_DISPATCH_API.DataAccess.Validators;
using FUEL_DISPATCH_API.Utils.Constants;
using FUEL_DISPATCH_API.Utils.Exceptions;
using FUEL_DISPATCH_API.Utils.ResponseObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Runtime.CompilerServices;
using System.Web.Http.ModelBinding;
using Twilio.Rest.Verify.V2;

namespace FUEL_DISPATCH_API.DataAccess.Repository.Implementations
{
    public class UsersAuth : GenericRepository<User>, IUsersAuth
    {
        private readonly FUEL_DISPATCH_DBContext _DBContext;
        private readonly IEmailSender _emailSender;

        private readonly string? _secretKey;
        public UsersAuth(IConfiguration config, FUEL_DISPATCH_DBContext DBContext, IEmailSender emailSender)
            : base(DBContext)
        {
            _DBContext = DBContext;
            _emailSender = emailSender;
            _secretKey = config.GetSection("settings:secretkey").Value; //Obtener llave y valor
        }
        public override ResultPattern<User> Post(User entity)
        {

            var passwordHash = PasswordHashing(entity.Password);
            entity.Password = passwordHash;
            _DBContext.User.Add(entity);
            _DBContext.SaveChanges();
            _emailSender.SendEmailAsync(entity.Email, AppConstants.ACCOUNT_CREATED_MESSAGE, $"Hello {entity.FullName} your account in the app was created successfully at {DateTime.Now}");
            return ResultPattern<User>.Success(entity, StatusCodes.Status200OK, "Usuario registrado correctamente. ");
        }
        public ResultPattern<object> Login(LoginDto loginDto)
        {
            var authManager = new AuthManager(_DBContext, _secretKey!); // Instancia de AuthManager
            var result = authManager.AuthToken(loginDto); // Validar usuario)
            return ResultPattern<object>.Success(result, StatusCodes.Status200OK, "Token obtenido correctamente. "); // Devolver token
        }
        bool IsUserNameUnique(string username)
           => !_DBContext.User.Any(x => x.Username == username);
        bool IsEmailUnique(string email)
            => !_DBContext.User.Any(x => x.Email == email);
        string PasswordHashing(string password)
            => BCrypt.Net.BCrypt.HashPassword(password, 13);
        bool DriverIdHasValue(User user)
        {
            if (user.DriverId.HasValue)
            {
                var driver = _DBContext.Driver.FirstOrDefault(x => x.Id == user.DriverId);
                if (driver!.Status is ValidationConstants.InactiveStatus)
                    throw new BadRequestException("This driver is Inactive. ");

                user.FullName = driver!.FullName;
                user.Email = driver!.Email;
                user.PhoneNumber = driver!.PhoneNumber;
                user.BirthDate = driver!.BirthDate;
                user.FullDirection = driver!.FullDirection;
                return true;
            };
            return false;
        }
        //bool CheckIfDriverAlreadyHasUser(User user)
        //{
        //    var driver = _DBContext.Drivers.FirstOrDefault(x=>x.Id == user.DriverId);
        //    return driver!.User.Any();
        //}


        //void SetDriverRol(User user)
        //{
        //    _DBContext.UsersRols.Add(new UsersRols
        //    {
        //        RolId = 2,
        //        UserId = user.Id
        //    });
        //    _DBContext.SaveChanges();
        //}
    }
}
