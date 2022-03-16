using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Minibank.Web.Controllers
{
    [ApiController]
    [Route("user")]
    public class UserController : ControllerBase
    {
        [HttpPost]
        public void Create()
        {

        }

        [HttpPut]
        public void Edit()
        {

        }

        [HttpDelete]
        public void Delete(int id)
        {
            //todo: check if accounts exist, throw ValidationExceptions
        }
    }
}
