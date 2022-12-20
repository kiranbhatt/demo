using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Books.API.Entities;

namespace Books.API.Services
{
    public interface ITokenService
    {
         string GenerateJwtToken(ApplicationUser user, IList<string> roles);
    }
}