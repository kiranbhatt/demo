using Books.API.Models;
using Books.API.Models.Dto;
using Books.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Books.API.Controllers
{
    [Route("api/v{version:apiVersion}/users")]
    [ApiVersion("1.0")]
    [Authorize(Roles = "Admin")]
    public class UsersController : BaseApiController
    {
        private readonly IUsersService _usersService;
        private readonly ILogger<UsersController> _logger;
        protected APIResponse _aPIResponse;


        public UsersController(IUsersService usersService, ILogger<UsersController> logger)
        {
            _usersService = usersService ??
                throw new ArgumentNullException(nameof(usersService));
            _logger = logger;

            _aPIResponse = new APIResponse();
        }

        [HttpGet]
        public async Task<IEnumerable<MemberDto>> GetAll()
        {
            return await _usersService.GetAll();
        }

        /// <summary>
        /// The Route data by default is string. So no need of {username : string}
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{username}")]
        public async Task<MemberDto> GetbyUserName(string username)
        {
            return await _usersService.Get(username);
        }

        [HttpPut]
        public async Task<ActionResult<APIResponse>> UpdateUser(MemberUpdateDto memberUpdateDto)
        {
            var isUpdated = await _usersService.UpdateUser(memberUpdateDto);

            if (isUpdated)
            {
                _aPIResponse.StatusCode = System.Net.HttpStatusCode.NoContent;
                _aPIResponse.IsSuccess = true;
            }

            _aPIResponse.StatusCode = System.Net.HttpStatusCode.NotFound;

            return Ok(_aPIResponse);
        }
    }
}
