using Books.API.Contexts;
using Books.API.Entities;
using Books.API.Services.Abstract;
using Books.Core.Entities;
using Books.Core.Repositories.Abstract;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Books.Core.Repositories.Implementation.EntityFramework
{
    public class RolesRepository : Repository<ApplicationRole>, IRolesRepository
    {
        public RolesRepository(BookContext dbContext, ILoggerFactory logger) : base(dbContext, logger)
        {
        }
    }
}
