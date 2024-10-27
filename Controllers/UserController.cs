using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
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
        private readonly IConfiguration _configuration;

        public UserController(IConfiguration _configuration, IUserRepository _userRepository)
        {
            this._configuration = _configuration;
            this._userRepository = _userRepository;
        }

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

        [HttpGet("checkuser")]
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

        [AllowAnonymous]
        [HttpPost("login")]
        public ActionResult Login([FromBody] LoginDTO loginDTO)
        {
            var user = _userRepository.Authenticate(loginDTO);

            if (user == null)
                return NotFound("Invalid credentials");

            var token = GenerateJwtToken(user);
            return Ok(token);

        }

        private string GenerateJwtToken(UserDTO user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Name),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            var token = new JwtSecurityToken(_configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(8),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);

        }
    }
}
