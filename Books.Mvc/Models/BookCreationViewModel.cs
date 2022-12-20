using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace Books.Mvc.Models
{
    public class BookCreationViewModel
    {
        [Required]
        public Guid AuthorId { get; set; }

        [Required]
        public string Title { get; set; } 

        public string Description { get; set; }

        public IFormFile FormFile { get; set; }
    }

}
