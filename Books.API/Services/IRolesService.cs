using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Books.Core.Entities;

namespace Books.API.Services
{
    public interface IRolesService
    {
        Task<IEnumerable<ApplicationRole>> GetRolesAsync();

        Task<Guid> AddRole(ApplicationRole role);
    }
}
