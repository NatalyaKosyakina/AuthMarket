using AuthMarket.Abstractions;
using AuthMarket.Security;
using Microsoft.AspNetCore.Mvc;
using AuthMarket.Model;
using AllowAnonymousAttribute = HotChocolate.Authorization.AllowAnonymousAttribute;

namespace AuthMarket.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepo _userRepo;
        private readonly ITokenService _tokenService;

        public AuthController(IAuthRepo userRepo, ITokenService tokenService)
        {
            _userRepo = userRepo;
            _tokenService = tokenService;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Login")]
        public ActionResult Login([FromBody] UserAuthRequest userLogin)
        {
            try
            {
                var roleId = _userRepo.CheckRole(userLogin.Email, userLogin.Password);

                var user = new UserAuthRequest() { Email = userLogin.Email, UserRole = roleId };

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
        [Route("AddAdmin")]
        public ActionResult AddAdmin([FromBody] UserAuthRequest userLogin)
        {
            try
            {
                _userRepo.AddUser(userLogin.Email, userLogin.Password, UserRoleType.Admin);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
            return Ok();
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("AddUser")]
        public ActionResult AddUser([FromBody] UserAuthRequest userLogin)
        {
            try
            {
                _userRepo.AddUser(userLogin.Email, userLogin.Password, UserRoleType.User);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
            return Ok();
        }
    }
}
