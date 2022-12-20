using Books.API.Contexts;
using Books.API.Entities;
using Books.API.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Books.Core.Repositories.Implementation.EntityFramework
{
    public class BooksRepository : Repository<Book>, IBooksRepository, IDisposable
    {
        private BookContext _context;

        public BooksRepository(BookContext context, ILoggerFactory logger) : base(context, logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_context != null)
                {
                    _context.Dispose();
                    _context = null;
                }

            }
        }

        /// <summary>
        ///  Question : Why we have't use AddAsync() to save data asyncronalsy 
        ///  
        /// Answer : Go to the defination of _context.Books.AddAsync().
        /// 
        ///  This method is async only to allow special value generators, such as the one
        //     used by 'Microsoft.EntityFrameworkCore.Metadata.SqlServerValueGenerationStrategy.SequenceHiLo',
        //     to access the database asynchronously.
        //
        //     For all other cases the non async method should be used.
        /// </summary>
        /// <param name="bookToAdd"></param>
        public void AddBook(Book bookToAdd)
        {
            if (bookToAdd == null)
            {
                throw new ArgumentNullException(nameof(bookToAdd));
            }
            _context.Books.Add(bookToAdd);
        }


        public async Task UpdateBookPatch(Guid bookId, JsonPatchDocument<Book> bookModel)
        {
            var book = await _context.Books.FindAsync(bookId);

            if (book != null)
            {
                bookModel.ApplyTo(book);

                _context.Books.Update(book);
            }
        }
    }
}
