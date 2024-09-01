using FMP_DISPATCH_API.Services.Emails;
using FUEL_DISPATCH_API.Auth;
using FUEL_DISPATCH_API.DataAccess.DTOs;
using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.DataAccess.Repository.GenericRepository;
using FUEL_DISPATCH_API.Utils.Constants;
using FUEL_DISPATCH_API.Utils.Exceptions;
using FUEL_DISPATCH_API.Utils.ResponseObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
namespace FUEL_DISPATCH_API.DataAccess.Repository.Implementations
{
    public class UsersAuth : GenericRepository<User>, IUsersAuth
    {
        private readonly FUEL_DISPATCH_DBContext _DBContext;
        private readonly IEmailSender _emailSender;
        private readonly string? _secretKey;
        public UsersAuth(IConfiguration config, FUEL_DISPATCH_DBContext DBContext,
            IEmailSender emailSender,
            IHttpContextAccessor httpContextAccessor)
            : base(DBContext, httpContextAccessor)
        {
            _DBContext = DBContext;
            _emailSender = emailSender;
            _secretKey = config.GetSection("settings:secretkey").Value; //Obtener llave y valor
        }
        public ResultPattern<User> UserRegistration(UserRegistrationDto entity)
        {
            if (IsUserNameUnique(entity))
                throw new BadRequestException("User with this user name exist. ");

            if (IsEmailUnique(entity))
                throw new BadRequestException("User with this user name exist. ");

            if (entity.DriverId.HasValue)
                DriverIdHasValue(entity);

            var passwordHash = PasswordHashing(entity.Password);
            entity.Password = passwordHash;

            var newUser = new User
            {
                Email = entity.Email,
                FullName = entity.FullName,
                Username = entity.Username,
                BirthDate = entity.BirthDate,
                PhoneNumber = entity.PhoneNumber,
                FullDirection = entity.FullDirection,
                Password = entity.Password,
                DriverId = entity.DriverId,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                CreatedBy = entity.CreatedBy,
                UpdatedBy = entity.UpdatedBy
            };

            _DBContext.User.Add(newUser);
            _DBContext.SaveChanges();
            if (entity.Email is not null)
                _emailSender.SendEmailAsync(entity.Email, AppConstants.ACCOUNT_CREATED_MESSAGE, $"Hello {entity.FullName} your account in the app was created successfully at {DateTime.Now}");

            return ResultPattern<User>.Success(newUser, StatusCodes.Status200OK, "Usuario registrado correctamente. ");
        }
        public ResultPattern<object> Login(LoginDto loginDto)
        {
            var authManager = new AuthManager(_DBContext, _secretKey!); // Instancia de AuthManager
            var result = authManager.AuthToken(loginDto); // Validar usuario)
            return ResultPattern<object>.Success(result, StatusCodes.Status200OK, "Token obtenido correctamente. "); // Devolver token
        }
        public bool IsUserNameUnique(UserRegistrationDto user)
           => _DBContext.User.Any(x => x.Username == user.Username);
        public bool IsEmailUnique(UserRegistrationDto user)
            => _DBContext.User.Any(x => x.Email == user.Email);
        string PasswordHashing(string password)
            => BCrypt.Net.BCrypt.HashPassword(password, 13);
        public bool DriverIdHasValue(UserRegistrationDto user)
        {
            var driver = _DBContext.Driver.FirstOrDefault(x => x.Id == user.DriverId);
            if (driver!.Status is ValidationConstants.InactiveStatus)
                throw new BadRequestException("This driver is Inactive. ");

            user.FullName = driver?.FullName!;
            user.Email = driver?.Email;
            user.PhoneNumber = driver?.PhoneNumber!;
            user.BirthDate = driver?.BirthDate;
            user.FullDirection = driver?.FullDirection!;
            return false;
        }
    }
}
