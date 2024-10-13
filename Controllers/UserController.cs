using Microsoft.AspNetCore.Mvc;
using WebAppSeminar4.Abstraction;
using WebAppSeminar4.DTO;
using WebAppSeminar4.Models;

namespace WebAppSeminar4.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        [HttpPost]
        public ActionResult<int> AddUser(UserDTO userDTO)
        {
            try
            {
                return Ok(_userRepository.AddUser(userDTO));
            }
            catch (Exception ex) 
            {
                return StatusCode(409, ex.Message);
            }
        }
        [HttpGet]
        public ActionResult<RoleId> CheckUser(LoginDTO loginDTO)
        {
            try
            {
                var roleId = _userRepository.CheckUser(loginDTO);
                return Ok(roleId);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);

            }
        }
        [HttpPost("login")]
        public ActionResult<string> Login(LoginDTO loginDTO)
        {
            var user = _userRepository.CheckUser(loginDTO.Name, loginDTO.Password);

            if (user == null)
                return Unauthorized();

            var token = GenerateJwtToken(user);
            return Ok(token);
        }
    }
}
