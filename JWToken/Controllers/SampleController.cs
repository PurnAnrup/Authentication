using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace JWToken.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SampleController : ControllerBase
    {
        [HttpGet]
        [Authorize(Roles = "admin")]
        public ActionResult<IEnumerable<string>> Load()
        {
            return new string[] { "value1", "value2" };
        }
        [HttpGet("{id}")]
        [Authorize]
        public ActionResult<string> LoadOne(int id)
        {
            return "value";
        }

    }
}
