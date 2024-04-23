using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.DataAccess.DTOs;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Diagnostics;

namespace FMP_MATEINANCE_API.Auth
{
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

            var credencialesCorretas = _dbContext.Users.Include(x => x.Rols).SingleOrDefault(x => x.Username == username);

            if (credencialesCorretas != null && BCrypt.Net.BCrypt.Verify(password, credencialesCorretas.Password))
            {
                var keyBytes = Encoding.UTF8.GetBytes(_secretKey);
                var claims = new ClaimsIdentity();

                claims.AddClaim(new Claim(ClaimTypes.NameIdentifier, usuario.Username!));
                claims.AddClaim(new Claim(ClaimTypes.Email, credencialesCorretas.Email));
                
                foreach (var role in credencialesCorretas.Rols)
                    claims.AddClaim(new Claim(ClaimTypes.Role, role.RolName));
                
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
                    _dbContext.UsersTokens.Add(new UsersTokens() { Token = tokenCreado, UserId = credencialesCorretas.Id, CreatedAt = DateTime.Now, ExpData = DateTime.Now + TimeSpan.FromDays(5)});
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
                // Devolver algún tipo de indicador de error en lugar de Unauthorized directamente
                throw new Exception("User Does Not Exist. ");
            }
        }
    }
}
