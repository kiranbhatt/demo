using System;

namespace Books.Mvc.Models
{
    public class BookViewModel
    {
        public Guid Id { get; set; }

        public string Author { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

    }
}
