using AutoMapper;
using Books.API.Entities;
using Books.API.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Books.API.Test.Services
{
    [TestClass]
    public class BooksServiveTests
    {
        private MockRepository mockRepository;

        private Mock<IBooksRepository> mockBooksRepository;
        private Mock<IMapper> mockMapper;
        private IEnumerable<Book> books;

        [TestInitialize]
        public void TestInitialize()
        {
            this.mockRepository = new MockRepository(MockBehavior.Default);

            this.mockBooksRepository = this.mockRepository.Create<IBooksRepository>();
            this.mockMapper = this.mockRepository.Create<IMapper>();

            books = new List<Book>() 
            {
                 new Book
                {
                    Id = Guid.Parse("5b1c2b4d-48c7-402a-80c3-cc796ad49c6b"),
                    AuthorId = Guid.Parse("d28888e9-2ba9-473a-a40f-e38cb54f9b35"),
                    Title = "The Winds of Winter",
                    Description = "The book that seems impossible to write."
                },
                new Book
                {
                    Id = Guid.Parse("d8663e5e-7494-4f81-8739-6e0de1bea7ee"),
                    AuthorId = Guid.Parse("d28888e9-2ba9-473a-a40f-e38cb54f9b35"),
                    Title = "A Game of Thrones",
                    Description = "A Game of Thrones is the first novel in A Song of Ice and Fire, a series of fantasy novels by American author George R. R. ... In the novel, recounting events from various points of view, Martin introduces the plot-lines of the noble houses of Westeros, the Wall, and the Targaryens."
                }
            };
        }

        private BooksServive CreateBooksServive()
        {
            return new BooksServive(
                this.mockBooksRepository.Object,
                this.mockMapper.Object);
        }


        [TestMethod]
        public async Task GetBooksAsync_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var booksServive = this.CreateBooksServive();
            this.mockBooksRepository.Setup(x => x.GetBooksAsync()).Returns(Task.FromResult(books));

            // Act
            var result = await booksServive.GetBooksAsync();

            // Assert
            this.mockRepository.VerifyAll();
        }
    }
}
