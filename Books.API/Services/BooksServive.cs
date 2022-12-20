using AutoMapper;
using Books.API.Models.Dto;
using Books.Core.Repositories.Abstract;
using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Books.API.Services
{
    public class BooksServive : IBooksServive
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public BooksServive(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        /// Primary key which is GUID is auto generated. See migration classes - ValueGeneratedOnAdd()
        /// </summary>
        /// <param name="bookForCreation"></param>
        /// <returns></returns>
        public async Task<Guid> AddBook(BookForCreation bookForCreation)
        {
            var bookEntity = _mapper.Map<Entities.Book>(bookForCreation);

            _unitOfWork.BooksRepository.AddAsync(bookEntity);
            await _unitOfWork.Complete();

            return bookEntity.Id;
        }

        public async Task<Book> GetBookAsync(Guid id)
        {
            var bookEntity = await _unitOfWork.BooksRepository.GetAsync(x => x.Id == id, traked: false, includeProperties: "Author");

            return _mapper.Map<Book>(bookEntity);
        }

        public async Task<IEnumerable<Book>> GetBooksAsync()
        {
            var booksEntity = await _unitOfWork.BooksRepository.GetAllAsync(includeProperties: "Author");

            return _mapper.Map<IEnumerable<Book>>(booksEntity);
        }

        public async Task UpdateBookPatchAsync(Guid bookId, JsonPatchDocument<BookForCreation> model)
        {
            var bookEntity = _mapper.Map<JsonPatchDocument<Entities.Book>>(model);

            await _unitOfWork.BooksRepository.UpdateBookPatch(bookId, bookEntity);

            await _unitOfWork.Complete();
        }
    }
}
