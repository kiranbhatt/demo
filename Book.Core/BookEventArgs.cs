using System;
using System.IO;

namespace Books.Core
{
    public class BookEventArgs : EventArgs
    {
        public Guid AuthorId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public Stream File { get; set; }
    }
}
