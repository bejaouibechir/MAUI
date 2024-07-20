using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

public static class JwtHelper
{
    private static string SecretKey = "ABCD1234EFGH5678IJKL9012MNOP3456";
    private static string Issuer = "PostmanIssuer";
    private static string Audience = "PostmanAudience";

    public static bool Check(string token,out ClaimsPrincipal principal)
    {
        principal = GetPrincipalFromToken(token);
        if (principal == null)
        {
                var username = GetClaimValue(principal, ClaimTypes.Name);
                var role = GetClaimValue(principal, ClaimTypes.Role);
            return true;
        }
        return false;
    }
    public static ClaimsPrincipal GetPrincipalFromToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(SecretKey);

        try
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = Issuer,
                ValidAudience = Audience,
                IssuerSigningKey = new SymmetricSecurityKey(key)
            };

            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken validatedToken);
            return principal;
        }
        catch
        {
            return null;
        }
    }
    public static string GetClaimValue(ClaimsPrincipal principal, string claimType)
    {
        var claim = principal?.Claims.FirstOrDefault(c => c.Type == claimType);
        return claim?.Value;
    }
}


/*
 using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace MyJwtExample.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class ValuesController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (token == null)
                return Unauthorized();

            var principal = JwtHelper.GetPrincipalFromToken(token);
            if (principal == null)
                return Unauthorized();

            var username = JwtHelper.GetClaimValue(principal, ClaimTypes.Name);
            var role = JwtHelper.GetClaimValue(principal, ClaimTypes.Role);

            if (role != "Admin")
                return Forbid();

            return Ok(new { Message = $"Authorized request successful for user {username}" });
        }
    }
}

 
 
 */
