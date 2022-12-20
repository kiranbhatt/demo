using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Web;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Books.WebUI.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ITokenAcquisition _tokenAcquisition;
        public string accessToken;

        public IndexModel(ILogger<IndexModel> logger, ITokenAcquisition tokenAcquisition)
        {
            _logger = logger;
            _tokenAcquisition = tokenAcquisition;
        }

        public async Task OnGet()
        {
            const string scopesToStorageRequest = "api://e867ced8-0812-4ec3-ac00-beb40122ee3b/Book.ReadAll";
            accessToken = await _tokenAcquisition.GetAccessTokenForUserAsync(new string[] { scopesToStorageRequest });

            string apiURL = "https://bookprod.azurewebsites.net/";

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiURL);
               
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                //GET Method
                HttpResponseMessage response = await client.GetAsync("api/books");
                if (response.IsSuccessStatusCode)
                {

                }
                else
                {
                    Console.WriteLine("Internal server Error");
                }
            }
        }
    }
}

