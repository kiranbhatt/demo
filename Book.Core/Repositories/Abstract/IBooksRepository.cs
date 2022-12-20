using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Books.API.Services
{
    public interface IBooksRepository : IRepository<Entities.Book>
    {
        Task UpdateBookPatch(Guid bookId, JsonPatchDocument<Entities.Book> model);
    }
}
