using FMP_DISPATCH_API.Services.Emails;
using FUEL_DISPATCH_API.Auth;
using FUEL_DISPATCH_API.DataAccess.DTOs;
using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.DataAccess.Repository.GenericRepository;
using FUEL_DISPATCH_API.Utils.Exceptions;
using FUEL_DISPATCH_API.Utils.ResponseObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
namespace FUEL_DISPATCH_API.DataAccess.Repository.Implementations
{
    public class UsersAuth : GenericRepository<User>, IUsersAuth
    {
        private readonly FUEL_DISPATCH_DBContext _DBContext;
        private readonly IHttpContextAccessor _httpContextAccesor;
        // private readonly IEmailSender _emailSender;
        private readonly string? _secretKey;
        public UsersAuth(IConfiguration config, FUEL_DISPATCH_DBContext DBContext,
            /*IEmailSender emailSender,*/
            IHttpContextAccessor httpContextAccessor)
            : base(DBContext, httpContextAccessor)
        {
            _DBContext = DBContext;
            _httpContextAccesor = httpContextAccessor;
            // _emailSender = emailSender;
            _secretKey = config.GetSection("settings:secretkey").Value; //Obtener llave y valor
        }
        public ResultPattern<User> UserRegistration(UserRegistrationDto entity)
        {
            if (IsUserNameUnique(entity))
                throw new BadRequestException("User with this user name exist. ");

            if (!string.IsNullOrEmpty(entity.Email))
                if (IsEmailUnique(entity))
                    throw new BadRequestException("User with this email exist. ");

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
                CompanyId = entity.CompanyId,
                CreatedAt = DateTime.Today,
                UpdatedAt = DateTime.Today,
                CreatedBy = entity.CreatedBy,
                UpdatedBy = entity.UpdatedBy
            };
            _DBContext.User.Add(newUser);
            _DBContext.SaveChanges();
            //if (entity.Email is not null)
            //    _emailSender.SendEmailAsync(entity.Email,
            //        AppConstants.ACCOUNT_CREATED_MESSAGE,
            //        $"Hola {entity.FullName} tu cuenta en la app a sido creada. {DateTime.Today}");

            return ResultPattern<User>.Success(newUser,
                StatusCodes.Status200OK,
                "Usuario registrado correctamente. ");
        }
        public override ResultPattern<User> Update(Func<User, bool> predicate, User updatedEntity)
        {
            PasswordHashing(updatedEntity.Password!);
            return base.Update(predicate, updatedEntity);
        }
        public ResultPattern<object> Login(LoginDto loginDto)
        {
            var authManager = new AuthManager(_DBContext, _secretKey!);
            var result = authManager.AuthToken(loginDto);
            return ResultPattern<object>.Success(result,
                StatusCodes.Status200OK,
                "Sesion iniciada correctamente. ");
        }
        public bool IsUserNameUnique(UserRegistrationDto user)
        {
            //string? companyId;

            //companyId = _httpContextAccesor.HttpContext?.Items["CompanyId"]?.ToString();


            return _DBContext.User.Any(x => x.Username == user.Username /*&&
            x.CompanyId == int.Parse(companyId)*/);
        }
        public bool IsEmailUnique(UserRegistrationDto user)
            => _DBContext.User.Any(x => x.Email == user.Email);
        static string PasswordHashing(string password)
            => BCrypt.Net.BCrypt.HashPassword(password, 13);
        public bool DriverIdHasValue(UserRegistrationDto user)
        {
            var driver = _DBContext.Driver.FirstOrDefault(x => x.Id == user.DriverId) ?? throw new NotFoundException("El conductor indicado no se encontro. ");
            if (driver!.Status is Enums.ActiveInactiveStatussesEnum.Inactive)
                throw new BadRequestException("El conductor esta inactivo. ");

            user.FullName = driver?.FullName!;
            user.Email = driver?.Email;
            user.PhoneNumber = driver?.PhoneNumber!;
            user.BirthDate = driver?.BirthDate;
            user.FullDirection = driver?.FullDirection!;
            return false;
        }
    }
}
