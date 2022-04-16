using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Minibank.Core.Exceptions;
using Minibank.Web.Dto.Mapping;
using Minibank.Core.Domains.Users.Services;
using Minibank.Web.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Minibank.Web.Controllers
{
    [ApiController]
    [Route("user")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Create
        /// </summary>
        /// <param name="model"></param>
        [HttpPost]
        public async Task Create(UserDto model)
        {
            await _userService.CreateAsync(MapperUser.ToUser(model));
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="model"></param>
        [HttpPut]
        public async Task Update(UserDto model)
        {
            await _userService.UpdateAsync(model.Id, MapperUser.ToUser(model));
        }

        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="id"></param>
        [HttpDelete]
        public async Task Delete(int id)
        {
            await _userService.DeleteAsync(id);
        }
    }
}
