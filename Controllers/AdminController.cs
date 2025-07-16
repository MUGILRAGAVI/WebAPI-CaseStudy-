using DAL.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using DAL.DataAccess;

namespace ServiceLayer.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminRepository<Admin> _adminRepo;
        private readonly IConfiguration _configuration;

        public AdminController(IAdminRepository<Admin> adminRepo, IConfiguration configuration)
        {
            _adminRepo = adminRepo;
            _configuration = configuration;
        }

        [HttpPost("ValidateAdmin")]
        public IActionResult ValidateAdmin([FromBody] Admin admin)
        {
            var validatedAdmin = _adminRepo.ValidateAdmin(admin);
            if (validatedAdmin != null)
            {
                var token = GenerateToken(validatedAdmin);
                return Ok(new
                {
                    token,
                    user = new
                    {
                        email = validatedAdmin.Email,
                        role = validatedAdmin.Role
                    }
                });
            }

            return Unauthorized("Invalid credentials.");
        }


        //[NonAction]
        //private string GenerateToken(Admin admin)
        //{
        //    var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        //    var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        //    var claims = new[]
        //    {
        //        new Claim(ClaimTypes.Email, admin.Email)
        //    };

        //    var token = new JwtSecurityToken(
        //        issuer: _configuration["Jwt:Issuer"],
        //        audience: _configuration["Jwt:Audience"],
        //        claims: claims,
        //        expires: DateTime.Now.AddMinutes(30),
        //        signingCredentials: credentials
        //    );

        //    return new JwtSecurityTokenHandler().WriteToken(token);
        //}
        [NonAction]
        public string GenerateToken(Admin adminInfo)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var signingCredential = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var issuer = _configuration["Jwt:Issuer"];
            var audience = _configuration["Jwt:Audience"];

            var claims = new[]
            {
         new Claim(ClaimTypes.Role, adminInfo.Role)



     };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("your_secret_key_here"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "http://localhost:5238",
                audience: "http://localhost:5238",
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds
            );


            var token1 = new JwtSecurityToken(issuer, audience, claims, expires: DateTime.Now.AddMinutes(30), signingCredentials: signingCredential);
            return new JwtSecurityTokenHandler().WriteToken(token1);
        }
    }
}
