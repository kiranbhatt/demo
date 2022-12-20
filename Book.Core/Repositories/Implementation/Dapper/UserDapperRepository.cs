using Books.API.Entities;
using Books.API.Services.Abstract;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Books.Core.Repositories.Implementation.Dapper
{
    public class UserDapperRepository 
    {
        private readonly DapperContext _context;
        public UserDapperRepository(DapperContext context)
        {
            _context = context;
        }

       
        public  Task<(ApplicationUser LocalUser, string Token)> Login(ApplicationUser localUser, string password)
        {

            throw new NotImplementedException();

            //var sql = "SELECT * FROM Products WHERE Id = @Id";

            //ApplicationUser user = null;
            //using (var connection = _context.CreateConnection)
            //{
            //    connection.Open();
            //    user = await connection.QuerySingleOrDefaultAsync<ApplicationUser>(sql, new { Id = 1 });

            //}
        }

        public Task RemoveAsync(ApplicationUser entity)
        {
            throw new NotImplementedException();
        }

        public Task RemoveAsync(object Id)
        {
            throw new NotImplementedException();
        }

        public Task SaveAsync()
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(ApplicationUser entity)
        {
            throw new NotImplementedException();
        }
    }
}
