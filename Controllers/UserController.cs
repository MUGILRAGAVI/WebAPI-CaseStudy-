using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DAL.DataAccess;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ServiceLayer.DTO;

namespace ServiceLayer.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository<User> _userRepository;
        private readonly IConfiguration _configuration;

        public UserController(IUserRepository<User> userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }


        [HttpGet("GetAll")]

        public IActionResult GetAllUsers()
        {
            var users = _userRepository.GetAllUsers();
            return Ok(users);
        }

        [HttpGet("GetByEmail/{email}")]

        public IActionResult GetUserByEmail(string email)
        {
            var user = _userRepository.GetUserByEmail(email);
            if (user == null)
                return NotFound();
            return Ok(user);
        }

        //[Authorize(Roles = "User")]
        [HttpPost("Add")]

        public IActionResult AddUser([FromBody] User user)
        {
            var createdUser = _userRepository.AddUser(user);
            return CreatedAtAction(nameof(GetUserByEmail), new { email = createdUser.Email }, createdUser);
        }

        [HttpPost("AddDTO")]
        public IActionResult AddUser([FromBody] UserDTO dto)
        {
            var user = new User
            {
                FullName = dto.FullName,
                Email = dto.Email,
                PasswordHash = dto.PasswordHash,
                Role = dto.Role
            };

            var createdUser = _userRepository.AddUser(user);
            return CreatedAtAction(nameof(GetUserByEmail), new { email = createdUser.Email }, createdUser);
        }


        [HttpPut("Update")]

        public IActionResult UpdateUser([FromBody] User user)
        {
            var updatedUser = _userRepository.UpdateUser(user);
            return Ok(updatedUser);
        }

        [HttpDelete("DeleteByEmail/{email}")]
        public IActionResult DeleteByEmail(string email)
        {
            var user = _userRepository.GetUserByEmail(email);
            if (user == null)
                return NotFound("User not found");

            _userRepository.DeleteUser(user);
            return Ok("User deleted");
        }


        [HttpPost("ValidateUser")]
        public IActionResult Login([FromBody] UserLoginDTO loginDto)
        {
            if (string.IsNullOrEmpty(loginDto.Email) || string.IsNullOrEmpty(loginDto.Password))
                return BadRequest("Email and password are required.");

            var user = _userRepository.ValidateUser(loginDto.Email, loginDto.Password);

            if (user == null)
                return Unauthorized("Invalid credentials");

            // JWT Claims
            var claims = new[]
            {
        new Claim(ClaimTypes.Name, user.FullName),
        new Claim(ClaimTypes.Email, user.Email),
        new Claim(ClaimTypes.Role, user.Role)
    };

            // Create security key
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return Ok(new
            {
                Token = tokenString,
                User = new
                {
                    user.Id,
                    user.FullName,
                    user.Email,
                    user.Role
                }
            });
        }


    }
}
