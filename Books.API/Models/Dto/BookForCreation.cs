using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace Books.API.Models.Dto
{
    public class BookForCreation
    {
        [Required]
        public Guid AuthorId { get; set; }

        [Required]
        public string Title { get; set; } 

        public string Description { get; set; }

        public IFormFile FormFile { get; set; }
    }

}
