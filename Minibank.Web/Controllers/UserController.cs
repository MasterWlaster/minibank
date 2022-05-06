using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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
        /// <param name="cancellationToken"></param>
        [HttpPost]
        public async Task Create(UserDto model, CancellationToken cancellationToken)
        {
            await _userService.CreateAsync(MapperUser.ToUser(model), cancellationToken);
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="model"></param>
        /// <param name="cancellationToken"></param>
        [HttpPut]
        public async Task Update(UserDto model, CancellationToken cancellationToken)
        {
            await _userService.UpdateAsync(model.Id, MapperUser.ToUser(model), cancellationToken);
        }

        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        [HttpDelete]
        public async Task Delete(int id, CancellationToken cancellationToken)
        {
            await _userService.DeleteAsync(id, cancellationToken);
        }
    }
}
