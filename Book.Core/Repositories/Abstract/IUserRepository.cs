using Books.API.Entities;
using System;
using System.Threading.Tasks;

namespace Books.API.Services.Abstract
{
    public interface IUserRepository : IRepository<Entities.ApplicationUser>
    {
        bool IsUniqueUser(string userName);

        Task<(ApplicationUser LocalUser, string Token)> Login(ApplicationUser localUser, string password);
    }
}
