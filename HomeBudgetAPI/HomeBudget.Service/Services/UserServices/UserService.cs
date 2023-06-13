using HomeBudget.Core;
using HomeBudget.Core.Entities;
using HomeBudget.Core.Enums;
using HomeBudget.Service.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HomeBudget.Service.Services.UserServices
{
    public class UserService : IUserService
    {
        private readonly HomeBudgetDbContext _dbContext;
        private readonly AuthenticationSettings _authenticationSettings;

        public UserService(HomeBudgetDbContext dbContext, AuthenticationSettings authenticationSettings)
        {
            _dbContext = dbContext;
            _authenticationSettings = authenticationSettings;
        }

        public Role GetRole(UserRoleEnum userRole)
            => _dbContext.Roles.SingleOrDefault(r => r.Name == Enum.GetName(userRole));

        public async Task<Role> GetRoleAsync(UserRoleEnum userRole)
            => await _dbContext.Roles.SingleOrDefaultAsync(r => r.Name == Enum.GetName(userRole));

        public string GenerateTokenJWT(User user)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
                new Claim(ClaimTypes.Role, $"{user.Role.Name}"),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authenticationSettings.JwtKey));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(_authenticationSettings.JwtExpireDays);

            var token = new JwtSecurityToken(_authenticationSettings.JwtIssuer,
                _authenticationSettings.JwtIssuer,
                claims,
                expires: expires,
                signingCredentials: cred);

            var tokenHandler = new JwtSecurityTokenHandler();

            return tokenHandler.WriteToken(token);
        }
    }
}