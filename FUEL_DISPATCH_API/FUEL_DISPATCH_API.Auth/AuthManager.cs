using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.DataAccess.DTOs;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using FUEL_DISPATCH_API.Utils.Exceptions;
namespace FUEL_DISPATCH_API.Auth;
public class AuthManager
{
    private readonly FUEL_DISPATCH_DBContext _dbContext;
    private readonly string _secretKey;

    public AuthManager(FUEL_DISPATCH_DBContext dbContext, string secretKey)
    {
        _dbContext = dbContext;
        _secretKey = secretKey;
    }

    public object AuthToken(LoginDto usuario)
    {
        var username = usuario.Username;
        var password = usuario.Password;
        var credenciales = _dbContext.User.Include(x => x.Rols).SingleOrDefault(x => x.Username == username);
        if (credenciales != null && BCrypt.Net.BCrypt.Verify(password, credenciales.Password))
        {

            var companyId = _dbContext.UsersCompanies
                            .Where(uc => uc.UserId == credenciales.Id)
                            .Select(uc => uc.CompanyId)
                            .FirstOrDefault();

            var branchId = _dbContext.UsersBranchOffices
                            .Where(uc => uc.UserId == credenciales.Id)
                            .Select(uc => uc.BranchOfficeId)
                            .FirstOrDefault();

            var keyBytes = Encoding.UTF8.GetBytes(_secretKey);
            var claims = new ClaimsIdentity();
            claims.AddClaim(new Claim(ClaimTypes.NameIdentifier, usuario.Username!));
            if (companyId is not 0)
                claims.AddClaim(new Claim("CompanyId", companyId.ToString()));

            if (branchId is not 0)
                claims.AddClaim(new Claim("BranchOfficeId", branchId.ToString()));
            if (credenciales.Email is not null)
                claims.AddClaim(new Claim(ClaimTypes.Email, credenciales.Email));

            foreach (var role in credenciales.Rols!)
                claims.AddClaim(new Claim(ClaimTypes.Role, role.RolName!));
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claims,
                Expires = DateTime.UtcNow.AddDays(5),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(keyBytes), SecurityAlgorithms.HmacSha256Signature)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenConfig = tokenHandler.CreateToken(tokenDescriptor);
            var tokenCreado = tokenHandler.WriteToken(tokenConfig);
            try
            {
                _dbContext.UserToken.Add(new UserToken
                {
                    Token = tokenCreado,
                    UserId = credenciales.Id,
                    CreatedAt = DateTime.Now,
                    ExpData = tokenDescriptor.Expires
                });
                _dbContext.SaveChanges();

                var tokenObj = new
                {
                    Token = tokenCreado,
                    Success = true,
                    ExpDate = DateTime.Now + TimeSpan.FromDays(5),
                };
                return tokenObj;
            }
            catch
            {
                throw;
            }
        }
        else
        {
            throw new UnauthorizedException("El usuario no tiene acceso. ");
        }
    }
}

