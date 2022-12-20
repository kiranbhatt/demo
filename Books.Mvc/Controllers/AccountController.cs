using Books.Mvc.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Net.Http;
using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Text;
using Books.API.Models.Dto;
using Microsoft.AspNetCore.Identity;
using Books.Mvc.Dto;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Books.Mvc.Controllers
{
    /// <summary>
    /// TODO :  IHttpClientFactory
    /// 
    /// https://learn.microsoft.com/en-us/aspnet/core/fundamentals/http-requests?view=aspnetcore-6.0
    /// https://learn.microsoft.com/en-us/dotnet/architecture/microservices/implement-resilient-applications/use-httpclientfactory-to-implement-resilient-http-requests
    /// </summary>
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register(string returnurl = null)
        {
            ViewData["ReturnUrl"] = returnurl;

            List<SelectListItem> listItems = new List<SelectListItem>();
            listItems.Add(new SelectListItem()
            {
                Value = "Admin",
                Text = "Admin"
            });
            listItems.Add(new SelectListItem()
            {
                Value = "User",
                Text = "User"
            });

            RegisterViewModel registerViewModel = new RegisterViewModel()
            {
                RoleList = listItems
            };

            return View(registerViewModel);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var person = new RegisterationRequestDto { Name = model.Name, Email = model.Email, Password = model.Password, Role =  model.RoleSelected };

                var json = JsonConvert.SerializeObject(person);
                var data = new StringContent(json, Encoding.UTF8, "application/json");


                string apiURL = "https://localhost:5001/";

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiURL);

                    //GET Method
                    HttpResponseMessage response = await client.PostAsync("api/users/register", data);
                    if (response.IsSuccessStatusCode)
                    {

                        var result = JsonConvert.DeserializeObject<APIResponse>(await response.Content.ReadAsStringAsync());

                        if (result.IsSuccess == true && result.ErrorMessages.Count == 0)
                        {
                            TempData[SD.Success] = "User created successfully";

                            return RedirectToAction("Login");
                        }
                        else
                        {
                            AddError(result);
                        }




                        return View();
                    }
                }
            }

            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string returnurl = null)
        {
            ViewData["ReturnUrl"] = returnurl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginRequestViewModel model)
        {
            if (ModelState.IsValid)
            {
                var person = new LoginRequestDto { UserName = model.Email, Password = model.Password};

                var json = JsonConvert.SerializeObject(person);
                var data = new StringContent(json, Encoding.UTF8, "application/json");


                string apiURL = "https://localhost:5001/";

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiURL);

                    //GET Method
                    HttpResponseMessage response = await client.PostAsync("api/users/login", data);
                    if (response.IsSuccessStatusCode)
                    {

                        var result = JsonConvert.DeserializeObject<APIResponse>(await response.Content.ReadAsStringAsync());

                        if (result.IsSuccess == true && result.ErrorMessages.Count == 0)
                        {
                            var jsono = (JObject)JsonConvert.DeserializeObject(result.Data.ToString());
                            string token = jsono["token"].Value<string>();

                            HttpContext.Session.SetString("JWToken", token);

                            return RedirectToAction("Index","Home");
                        }
                        else
                        {
                            AddError(result);
                        }




                        return View();
                    }
                }
            }
            return View();
        }

        private void AddError(APIResponse result)
        {
            foreach (var item in result.ErrorMessages)
            {
                ModelState.AddModelError(string.Empty, item);

            }
        }


        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
