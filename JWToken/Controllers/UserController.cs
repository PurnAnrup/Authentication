using JWToken.Data;
using JWToken.Model;
using JWToken.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Org.BouncyCastle.Crypto.Generators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace JWToken.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IConfiguration _config;
        static readonly log4net.ILog _log4net = log4net.LogManager.GetLogger(typeof(UserController));
        private IUserService _userService;
      
        public UserController(IConfiguration config,IUserService userService)
        {
            _config = config;
            _userService = userService;
            
        }


        // POST api/user/login
        ///<summary>
        ///Login Status
        /// </summary>
        /// <return>
        /// Returns Login Status
        /// </return>
        /// <remarks>
        /// Sample request
        /// POST / api/User
        /// 
        /// </remarks>
        /// <response code="201">Login Success</response>
        [AllowAnonymous]
        [HttpPost("Login")]
        [ApiConventionMethod(typeof(DefaultApiConventions),
            nameof(DefaultApiConventions.Post))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesDefaultResponseType]
        public IActionResult Login([FromBody] Login login)
        {
            _log4net.Info("Login initiated!");
            string pass = _userService.UserLogin(login);
            if(pass.Length<100)
            {
                _log4net.Error("Login Failed.");
                return BadRequest(pass);
            }
            else
            {
               
                Response.Cookies.Append("token", pass, new Microsoft.AspNetCore.Http.CookieOptions
                {
                    HttpOnly = true
                });
                _log4net.Info("Login Successful.");
                return Ok(pass);
            } 
        }

        // POST api/user/Signup
        ///<summary>
        ///Sign up
        /// </summary>
        /// <return>
        /// Sign up status
        /// </return>
        /// <remarks>
        /// Sample request
        /// POST / api/User
        /// 
        /// </remarks>
        /// <response code="201">Signed up successfully</response>
        [AllowAnonymous]
        [HttpPost("SignUp")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesDefaultResponseType]

        public IActionResult SignUp([FromBody] User user)
        {
            _log4net.Info("Signup initiated!");
            string pass = _userService.UserSignUp(user);
            if(pass== "Successfully Registered")
            {
                _log4net.Info("Signup Successful.");
                return Ok(pass);
            }
            else
            {
                _log4net.Error("Signup Failed.");
                return BadRequest(pass);
            }
        }


        /// <summary>
        /// Get User Data
        /// </summary>
        /// <return>
        /// Returns User Data
        /// </return>
        /// <remarks>
        /// Sample request
        /// GET /api/User
        /// 
        /// </remarks>
        /// <response code="200">User Data</response>

        [Authorize]
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        public ActionResult<User> UserData(int id)
        {
            _log4net.Info("Get User Data");
            User user = _userService.UserDetails(id);
            return user;

        }

        /// <summary>
        /// Logout
        /// </summary>
        /// <return>
        /// Logs out
        /// </return>
        /// <remarks>
        /// Sample request
        /// GET /api/User
        /// 
        /// </remarks>
        /// <response code="200">Successfully logged out</response>
        [HttpGet("Logout")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        public IActionResult Logout()
        {
            _log4net.Info("Logout Initiated!");
            Response.Cookies.Delete("token");
            _log4net.Info("Logout Successful");
            return Ok();

        }

        
    }
}
