using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mongo_JWT.JWTConfiguration;
using Mongo_JWT.Models.AuthenticationLog;
using Mongo_JWT.Models.Login;
using Mongo_JWT.Models.Register;
using Mongo_JWT.Services.Login;
using Mongo_JWT.Services.Register;

namespace Mongo_JWT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IdentityController : ControllerBase
    {
      //private readonly IRegisterService _regSvc;
        private readonly IJwtBuilder _jwtBuilder;
        private readonly IEncryptor _encryptor;
        private readonly ILoginService _loginsvc;
        

        public IdentityController(ILoginService loginsvc, IJwtBuilder jwtBuilder, IEncryptor encryptor)
        {
            _loginsvc = loginsvc;
            _jwtBuilder = jwtBuilder;
            _encryptor = encryptor;
           
            

        }
       
        [HttpPost("login")]
        public ActionResult<string> Login([FromBody] LoginModel user)
        {
            var u = _loginsvc.GetUser(user.Email);

            if (u == null)
            {
                return NotFound("User not found.");
            }

           

            var isValid = u.ValidatePassword(user.Password, _encryptor);

            if (!isValid)
            {
                return BadRequest("Could not authenticate user.");
            }

            var token = _jwtBuilder.GetToken(u.Id);
            
            var Response = new
            {
                Token = token,
               
            };
            return Ok(Response);

           
        }
        [HttpPost("register")]
        public ActionResult Register([FromBody] CustomerDetail user)
        {
            var u = _loginsvc.GetUser(user.Email);

            if (u != null)
            {
                return BadRequest("User already exists.");
            }

            user.SetPassword(user.Password, _encryptor);
            user.EncryptImage(user.Profile_Photo);
            _loginsvc.InsertUser(user);
            var Message = new
            {
                Code = "0",
                Message = "Success"

                };
                return Ok(Message);

            //return Ok();
        }
        
        [HttpGet("Login")]
        [AllowAnonymous]
        [Authorize]
        public IActionResult Validate([FromQuery(Name = "email")] string email)
        {
            string token = Request.Headers["Token"];
            var u = _loginsvc.GetUser(email);

            if (u == null)
            {
                return NotFound("User not found.");
            }

            var userId = _jwtBuilder.ValidateToken(token);

            if (userId != u.Id)
            {
                _loginsvc.UpdateTokenStatus(token);
                return BadRequest("Invalid token.");
            }
            var chk = _loginsvc.CheckAuthentication(userId,token);

            if (chk=="Failed")
            {
                return BadRequest("Invalid token.");
            }
            var ur = _loginsvc.GetUSerDetails(userId);
            if (ur == null)
            {
                return NotFound("User not found.");
            }
           
            return Ok(ur);
        }
        
    }
}
