using HotDesk.Models;
using HotDesk.Services;
using Microsoft.AspNetCore.Mvc;

namespace HotDesk.Controllers
{
    [ApiController]
    [Route("api/account")]
    public class AccountController : ControllerBase
    {

        private readonly IAccountServices _accountServices;

        public AccountController(IAccountServices accountServices)
        {
            accountServices = _accountServices;
        }


        [HttpPost("login")]
        public ActionResult Login([FromBody] LoginUserDto dto)
        {
            string JwtToken = _accountServices.GenerateJwt(dto);
            return Ok(JwtToken);
        }
    }
}
