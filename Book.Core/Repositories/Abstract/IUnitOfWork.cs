using Books.API.Services;
using Books.API.Services.Abstract;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Books.Core.Repositories.Abstract
{
    public interface IUnitOfWork
    {
        IBooksRepository BooksRepository { get; }
        IRolesRepository RolesRepository { get; }
        IUserRepository UserRepository { get; }
        Task<bool> Complete();
        bool HasChanges();
    }
}
