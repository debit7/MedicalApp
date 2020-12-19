using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mongo_JWT.Models.Register;
using Mongo_JWT.Services.Register;

namespace Mongo_JWT.Controllers
{
    [Route("api")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        private readonly RegisterService _regSvc;

        public RegisterController(RegisterService registerService)
        {
            _regSvc = registerService;
        }

        [HttpPost("Register")]
        [AllowAnonymous]
        public IActionResult Register([FromBody] CustomerDetail customerDetail)
        {

            var list = _regSvc.Create(customerDetail);
            return Ok(list);
        }

        [HttpPost("Report")]
        [AllowAnonymous]
        public IActionResult Report([FromBody] CustomerDetail customerDetail)
        {

            var list = _regSvc.Read();
            return Ok(list);
        }
        //[HttpPost("Find")]
        //[AllowAnonymous]
        //public IActionResult Find([FromBody] CustomerDetail customerDetail)
        //{

        //    var list = _regSvc.Find(customerDetail.Id);
        //    return Ok(list);
        //}
        //[HttpPost("Update")]
        //[AllowAnonymous]
        //public IActionResult Update([FromBody] CustomerDetail customerDetail)
        //{
        //    _regSvc.Update(customerDetail);
        //    var Message = new
        //    {
        //        Code = "0",
        //        Message = "Success"

        //    };
        //    return Ok(Message);
        //}

        //[HttpPost("Delete")]
        //[AllowAnonymous]
        //public IActionResult Delete([FromBody] CustomerDetail customerDetail)
        //{
        //    _regSvc.Delete(customerDetail.Id);
        //    var Message = new
        //    {
        //        Code = "0",
        //        Message = "Success"

        //    };
        //    return Ok(Message);
        //}




    }
}
