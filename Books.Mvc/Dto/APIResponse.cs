using System.Collections.Generic;
using System.Net;

namespace Books.Mvc.Dto
{
    public class APIResponse
    {
        public bool IsSuccess { get; set; }

        public List<string> ErrorMessages { get; set; } = new List<string>();

        public object Data { get; set; }

        public HttpStatusCode StatusCode { get; set; }
    }
}
