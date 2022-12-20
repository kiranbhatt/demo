using Books.API.Contexts;
using Books.API.Entities;
using Books.API.Services.Abstract;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Books.Core.Repositories.Implementation.EntityFramework
{
    public class UserRepository : Repository<ApplicationUser>, IUserRepository
    {
        private BookContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly string secretkey;

        public UserRepository(BookContext context, ILoggerFactory logger, IConfiguration configuration, UserManager<ApplicationUser> userManager) : base(context, logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _userManager = userManager;
            secretkey = configuration.GetSection("ApiSettings:Secret").ToString();
        }

        public bool IsUniqueUser(string userName)
        {
            var user = _context.ApplicationUsers.FirstOrDefault(x => x.UserName.Equals(userName));

            if (user == null)
            {
                return true;
            }

            return false;
        }

        public async Task<(ApplicationUser LocalUser, string Token)> Login(ApplicationUser loginRequestDto, string password)
        {
            var user = await _context.ApplicationUsers.FirstOrDefaultAsync(x => x.Email.ToLower() == loginRequestDto.UserName.ToLower());

            bool isValidPassword = await _userManager.CheckPasswordAsync(user, password);

            if (user == null || isValidPassword == false)
            {
                return (new ApplicationUser(), string.Empty);
            }

            var roles = await _userManager.GetRolesAsync(user);

            return (user, GenerateJwtToken(user, roles));
        }

        private string GenerateJwtToken(ApplicationUser user, IList<string> roles)
        {


            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("[SECRET USED TO SIGN AND VERIFY JWT TOKENS, IT CAN BE ANY STRING]");

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = GetClaimsIdentity(user, roles),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        private ClaimsIdentity GetClaimsIdentity(ApplicationUser user, IList<string> roles)
        {
            var claims = new ClaimsIdentity();

            claims.AddClaim(new Claim("id", user.Id.ToString()));
            claims.AddClaim(new Claim(ClaimTypes.Name, user.NormalizedUserName));

            foreach (var item in roles)
            {
                claims.AddClaim(new Claim(ClaimTypes.Role, item));
            }

            return new ClaimsIdentity(claims);
        }
    }
}
