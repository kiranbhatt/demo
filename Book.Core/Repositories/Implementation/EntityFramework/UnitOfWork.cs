using Books.API.Contexts;
using Books.API.Entities;
using Books.API.Services;
using Books.API.Services.Abstract;
using Books.Core.Repositories.Abstract;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Books.Core.Repositories.Implementation.EntityFramework
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly BookContext _dbContext;
        private readonly ILoggerFactory _logger;
        private readonly IConfiguration _configuration;
        private readonly UserManager<ApplicationUser> _userManager;

        public UnitOfWork(BookContext dbContext, ILoggerFactory logger, IConfiguration configuration = null, UserManager<ApplicationUser> userManager = null)
        {
            _dbContext = dbContext;
            _logger = logger;
            _configuration = configuration;
            _userManager = userManager;
        }
      

        public IRolesRepository RolesRepository => new RolesRepository(_dbContext, _logger);

        public IUserRepository UserRepository => new UserRepository(_dbContext, _logger, _configuration, _userManager);

        public async Task<bool> Complete()
        {
           return  await _dbContext.SaveChangesAsync() > 0;
        }

        public bool HasChanges()
        {
            return _dbContext.ChangeTracker.HasChanges();
        }
    }
}
