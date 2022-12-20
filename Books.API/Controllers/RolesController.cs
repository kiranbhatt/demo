using Books.API.Models;
using Books.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Books.API.Controllers
{
    /// <summary>
    /// https://codewithmukesh.com/blog/permission-based-authorization-in-aspnet-core/
    /// </summary>
    
    [Route("api/roles")]
    [Authorize(Roles = "SuperAdmin")]
    public class RolesController : BaseApiController
    {
        private readonly IRolesService _rolesService;
        protected APIResponse _aPIResponse;

        public RolesController(IRolesService rolesService)
        {
            _rolesService = rolesService;

            _aPIResponse = new APIResponse();
        }

        [HttpGet]
        public async Task<ActionResult<APIResponse>> Get()
        {
            var roles = await _rolesService.GetRolesAsync();

            _aPIResponse.StatusCode = System.Net.HttpStatusCode.OK;
            _aPIResponse.IsSuccess = true;
            _aPIResponse.Data = roles;

            return Ok(_aPIResponse);
        }

        [HttpPost]
        public async Task<ActionResult<APIResponse>> AddRole(string roleName)
        {
            throw new NotImplementedException();

        }
    }
}
