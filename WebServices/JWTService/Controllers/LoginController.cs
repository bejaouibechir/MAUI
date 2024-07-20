using JWTService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JWTService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IContext _context;

        public LoginController(IContext context)
        {
            _context = context;
        }


        [HttpPost]
        public IActionResult Login([FromBody] LoginViewModel employee)
        {
            // This is just for example purposes. In real applications, validate the user against a database.
            if (_context.Employees.Any(e=>e.Login == employee.Login)
                && _context.Employees.Any(e => e.Password == employee.Password))
            {
                var current = _context.Employees
                    .SingleOrDefault(e => e.Login == employee.Login && e.Password==employee.Password);
                var tokenString = GenerateJWTToken(employee.Login, current.Role);
                return Ok(new { token = tokenString });
            }
            return Unauthorized();
        }

        private string GenerateJWTToken(string login,string role)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("ABCD1234EFGH5678IJKL9012MNOP3456"));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim("name", login),
                new Claim("role", role),
            };
           

            var token = new JwtSecurityToken(
                issuer: "PostmanIssuer",
                audience: "PostmanAudience",
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}


/*
 * admin jhon aaa
 {
    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1lIjoiamhvbiIsInJvbGUiOiJhZG1pbiIsImV4cCI6MTcxNzg1ODAxNCwiaXNzIjoiUG9zdG1hbklzc3VlciIsImF1ZCI6IlBvc3RtYW5BdWRpZW5jZSJ9.V2_XGUDg6c1m-ure18mVDWk0IVHZXBQznk8zFhgUU08"
}

 */

/*
 * moderator jhon1 aaa
 {
    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1lIjoiamhvbjEiLCJyb2xlIjoibW9kZXJhdG9yIiwiZXhwIjoxNzE3ODU4MDUxLCJpc3MiOiJQb3N0bWFuSXNzdWVyIiwiYXVkIjoiUG9zdG1hbkF1ZGllbmNlIn0.QjvaWT6OI7AoLkzwMcpMTHtQe_6geK7sEP_oRDnE9Mo"
}
 
 */

/*
 * employee2 password2
{
    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1lIjoiZW1wbG95ZWUyIiwicm9sZSI6InVzZXIiLCJleHAiOjE3MTc4NTgxMDMsImlzcyI6IlBvc3RtYW5Jc3N1ZXIiLCJhdWQiOiJQb3N0bWFuQXVkaWVuY2UifQ.gt3CwdnOhVaLNL0SRHeg1xKMMaOAljwvHNBIOgCdlp0"
}
 
 */