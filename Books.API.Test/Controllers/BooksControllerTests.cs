using Books.API.Controllers;
using Books.API.Models.Dto;
using Books.API.Services;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Books.API.Test.Controllers
{
    /// <summary>
    ///  Code coverage : https://www.code4it.dev/blog/code-coverage-on-azure-devops-yaml-pipelines#coverlet---the-nuget-package-for-code-coverage
    /// </summary>
    [TestClass]
    public class BooksControllerTests
    {
        private MockRepository mockRepository;

        private Mock<IBooksServive> mockBooksServive;

        private IEnumerable<Models.Dto.Book> books;

        private Mock<ILogger<BooksController>> mockLoggerServive;

        [TestInitialize]
        public void TestInitialize()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);

            this.mockBooksServive = this.mockRepository.Create<IBooksServive>();

            this.mockLoggerServive = this.mockRepository.Create<ILogger<BooksController>>();

            books = new List<Book>() 
            {
              new Book
                {

                    Id = Guid.Parse("5b1c2b4d-48c7-402a-80c3-cc796ad49c6b"),
                    Author ="Naveen Semwal",
                    Title = "The Winds of Winter",
                    Description = "The book that seems impossible to write."

                },
              new Book
                {
                    Id = Guid.Parse("d8663e5e-7494-4f81-8739-6e0de1bea7ee"),
                    Author ="Naveen Semwal",
                    Title = "A Game of Thrones",
                    Description = "A Game of Thrones is the first novel in A Song of Ice and Fire, a series of fantasy novels by American author George R. R. ... In the novel, recounting events from various points of view, Martin introduces the plot-lines of the noble houses of Westeros, the Wall, and the Targaryens."
                },
            };
            
        }

        private BooksController CreateBooksController()
        {
            return new BooksController(
                this.mockBooksServive.Object, this.mockLoggerServive.Object);
        }

        [TestMethod]
        public async Task GetBooks_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var booksController = this.CreateBooksController();

            this.mockBooksServive.Setup(x => x.GetBooksAsync()).Returns(Task.FromResult(books));

            // Act
            var result = await booksController.GetBooks();

            // Assert
            this.mockRepository.VerifyAll();
        }
    }
}
