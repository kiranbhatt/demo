using Books.API.Models;
using Books.API.Models.Dto;
using Books.API.Services;
using Books.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Web.Resource;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Books.API.Controllers
{
    /// <summary
    /// https://code-maze.com/global-error-handling-aspnetcore/
    /// 
    /// https://docs.microsoft.com/en-us/azure/active-directory/develop/howto-add-app-roles-in-azure-ad-apps
    /// 
    /// https://www.youtube.com/watch?v=xEvSFyXBX58&list=PLUGuCqrhcwZzht4r2sbByidApmrvEjL9m
    /// </summary>

    [Route("api/books")]
    [Authorize(Roles = "Admin")]
    public class BooksController : BaseApiController
    {
        private readonly IBooksServive _booksServive;
        private readonly ILogger<BooksController> _logger;
        protected APIResponse _aPIResponse;
        

        public BooksController(IBooksServive booksServive, ILogger<BooksController> logger)
        {
            _booksServive = booksServive ??
                throw new ArgumentNullException(nameof(booksServive));
            _logger = logger;

            _aPIResponse = new APIResponse();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Book>))]
        //[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes:Book.ReadAll")]
        public async Task<ActionResult<APIResponse>> GetBooks()
        {
            var bookEntities = await _booksServive.GetBooksAsync();

            _aPIResponse.StatusCode = System.Net.HttpStatusCode.OK;
            _aPIResponse.Data = bookEntities;

            return Ok(_aPIResponse);
        }

        [HttpGet]
        [Route("{id:guid}", Name = "GetBook")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Book))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes:Book.Read")]
        public async Task<IActionResult> GetBook(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest();
            }

            var bookEntity = await _booksServive.GetBookAsync(id);

            if (bookEntity == null)
            {
                return NotFound();
            }
            //var bookCovers = await _booksRepository.GetBookCoversAsync(id);

            //var propertyBag = new Tuple<Entities.Book, IEnumerable<ExternalModels.BookCover>>
            //    (bookEntity, bookCovers);

            //(Entities.Book book, IEnumerable<ExternalModels.BookCover> bookCovers) 
            //    propertyBag = (bookEntity, bookCovers);

            return Ok((bookEntity));
        }


        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Book))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes:Book.Create")]
        public async Task<IActionResult> CreateBook([FromForm] BookForCreation bookForCreation)  /*[FromForm] - fix-415-unsupported-media-type-on-file-upload*/
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(bookForCreation);
            }

            //var existingBook = await _booksServive.GetBookAsync(bookForCreation.AuthorId);

            //if (existingBook != null)
            //{
            //    ModelState.AddModelError("BookExistsError", "Book already exists !");
            //    return BadRequest(bookForCreation);
            //}
            //else
            //{

            await Task.Run(() => UploadBook(bookForCreation));

            var bookId = await Task.Run(() => _booksServive.AddBook(bookForCreation));
            Book bookEntity = await _booksServive.GetBookAsync(bookId);

            // To return 201 status and "GetBook" is name of route defined above. = Status 200 + URL (see location parameter in response)
            return CreatedAtRoute("GetBook", new { id = bookId }, bookEntity);
            //}
        }


        [Route("{id:guid}", Name = "DeleteBook")]
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult DeleteBook(Guid id)
        {
            //When we delete, we donot return anything
            return NoContent();
        }

        [Route("{id:guid}")]
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult UpdateBook([FromRoute] Guid id, [FromBody] BookForCreation bookForUpdate)
        {
            if (bookForUpdate == null || id != bookForUpdate.AuthorId)
            {
                return BadRequest();
            }

            return NoContent();
        }

        [Route("{id:guid}")]
        [HttpPatch]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult UpdateBookPatch(Guid id, JsonPatchDocument<BookForCreation> bookForUpdate)
        {
            if (bookForUpdate == null || id == Guid.Empty)
            {
                return BadRequest();
            }

            _booksServive.UpdateBookPatchAsync(id, bookForUpdate);

            return NoContent();
        }

        private static void UploadBook(BookForCreation bookForCreation)
        {
            BookMessageProducer messageProducer = new BookMessageProducer();

            // Store first in local drive and then in BLOB
            string path = Path.Combine(Directory.GetCurrentDirectory(), "images", bookForCreation.FormFile.FileName);

            using (Stream stream = new FileStream(path, FileMode.Create))
            {
                bookForCreation.FormFile.CopyTo(stream);

                // Invoking an event
               messageProducer.AddBookToQueue(new BookEventArgs { AuthorId = bookForCreation.AuthorId, Description = bookForCreation.Description, Title = bookForCreation.Title, File = stream });
                stream.Flush();
            }
        }

        [HttpPost(nameof(UploadFile))]
        public IActionResult UploadFile(IFormFile files)
        {
            string systemFileName = files.FileName;

            return Ok(files);
        }
    }
}
