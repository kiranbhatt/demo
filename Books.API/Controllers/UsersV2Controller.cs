using Books.API.Models;
using Books.API.Models.Dto;
using Books.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Books.API.Controllers
{
    [Route("api/v{version:apiVersion}/users")]
    [ApiController]
    [ApiVersion("2.0")]
    public class UsersV2Controller : BaseApiController
    {

        private readonly IUsersService _usersService;
        private readonly ILogger<UsersController> _logger;
        protected APIResponse _aPIResponse;


        public UsersV2Controller(IUsersService usersService, ILogger<UsersController> logger)
        {
            _usersService = usersService ??
                throw new ArgumentNullException(nameof(usersService));
            _logger = logger;

            _aPIResponse = new APIResponse();
        }

        [HttpGet]
        [Route("{username}")]
        public async Task<MemberDto> GetbyUserName(string username)
        {
            return await _usersService.Get(username);
        }
    }
}
