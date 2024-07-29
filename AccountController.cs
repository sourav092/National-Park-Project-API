using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NPWebAPI_Project_1.Models;
using NPWebAPI_Project_1.Repository.IRepository;

namespace NPWebAPI_Project_1.Controllers
{
    //[Route("api/user")]
    //[ApiController]
    //[Authorize]
    public class AccountController : Controller
    {
        private readonly IUserRepository _userRepository;
        public AccountController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        [HttpPost("register")]
        public IActionResult Register([FromBody]User user)
        {
            if(ModelState.IsValid)
            {
                var isUniqueUser = _userRepository.IsUniqueUser(user.UserName);
                if (!isUniqueUser) return BadRequest("User in Use !!!!");
                var userInfo= _userRepository.Register(user.UserName, user.Password);
                if (userInfo == null) return BadRequest();
            }
            return Ok(user);
        }
        [HttpPost("Authenticate")]
        public IActionResult Authenticate([FromBody]UserVM userVM )
        {
            var user = _userRepository.Authenticate(userVM.UserName,userVM.Password);
            if (user == null) return BadRequest("Wrong User/ Password!!");
            return Ok(user);
        }
    }
}
