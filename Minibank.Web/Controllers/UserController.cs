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
        public void Create(UserDto model)
        {
            _userService.Create(MapperUser.Map(model));
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="model"></param>
        [HttpPut]
        public void Update(UserDto model)
        {
            _userService.Update(model.Id, MapperUser.Map(model));
        }

        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="id"></param>
        [HttpDelete]
        public void Delete(int id)
        {
            _userService.Delete(id);
        }
    }
}
