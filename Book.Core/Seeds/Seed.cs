using Books.API.Contexts;
using Books.API.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Books.Core.Seeds
{
    public class Seed
    {
        public static async Task SeedBooks(BookContext context)
        {
            if (!await context.Books.AnyAsync())
            {
                var bookData = await File.ReadAllTextAsync("Configure/BookSeedData.json");

                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

                var books = JsonSerializer.Deserialize<List<Book>>(bookData);

                foreach (var book in books)
                {
                    context.Books.Add(book);
                }

                await context.SaveChangesAsync();
            }
        }


        public static async Task SeedUsers(BookContext context)
        {
            if (!await context.ApplicationUsers.AnyAsync())
            {
                var bookData = await File.ReadAllTextAsync("Configure/UserSeedData.json");

                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

                var books = JsonSerializer.Deserialize<List<ApplicationUser>>(bookData);

                foreach (var book in books)
                {
                    context.ApplicationUsers.Add(book);
                }

                await context.SaveChangesAsync();
            }
        }
    }
}
