using AuthMarket.Abstractions;
using AuthMarket.Security;
using Microsoft.AspNetCore.Mvc;
using AuthMarket.Model;
using AllowAnonymousAttribute = HotChocolate.Authorization.AllowAnonymousAttribute;

namespace AuthMarket.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController(IAuthRepo userRepo, ITokenService tokenService) : ControllerBase
    {
        private readonly IAuthRepo _userRepo = userRepo;
        private readonly ITokenService _tokenService = tokenService;

        [AllowAnonymous]
        [HttpPost]
        [Route(template:"Login")]
        public ActionResult Login([FromBody] UserAuthRequest userLogin)
        {
            try
            {
                var roleId = _userRepo.CheckRole(userLogin.Email, userLogin.Password);

                var user = new UserAuthRequest() { Email = userLogin.Email };

                var token = _tokenService.CreateToken(user.Email, roleId.ToString());
                return Ok(token);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [Route(template: "AddUser")]
        public ActionResult AddUser([FromBody] UserAuthRequest userLogin)
        {
            try
            {
                return Ok(_userRepo.AddUser(userLogin.Email, userLogin.Password));
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
    }
}
