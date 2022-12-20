using System;

namespace Books.API.Models.Dto
{
    public class PhotoDto
    {
        public Guid Id { get; set; }
        public string Url { get; set; }
        public bool IsMain { get; set; }
        public string PublicId { get; set; }
    }
}