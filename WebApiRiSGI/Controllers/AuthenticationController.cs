using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiRiSGI.Models;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.Extensions.Configuration;
using System;

namespace WebApiRiSGI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly string secretKey;
        private readonly SgiContext _dbcontext;

        public AuthenticationController(IConfiguration config, SgiContext dbcontext)
        {
            secretKey = config.GetSection("settings").GetSection("secretkey").ToString();
            _dbcontext = dbcontext;
        }

        [HttpPost]
        [Route("Authenticate")]
        public IActionResult Authenticate([FromBody] Usuarios request)
        {
            // Retrieve the user from the database based on UserLogin
            Usuarios user = _dbcontext.Usuarios.SingleOrDefault(u => u.UserLogin == request.UserLogin);

            if (user != null && user.UserPass == request.UserPass)
            {
                var keyBytes = Encoding.ASCII.GetBytes(secretKey);
                var claims = new ClaimsIdentity();

                claims.AddClaim(new Claim(ClaimTypes.NameIdentifier, request.UserLogin));

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = claims,
                    Expires = DateTime.UtcNow.AddMinutes(5),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(keyBytes), SecurityAlgorithms.HmacSha256Signature)
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                var tokenConfig = tokenHandler.CreateToken(tokenDescriptor);

                string createdToken = tokenHandler.WriteToken(tokenConfig);

                return StatusCode(StatusCodes.Status200OK, new { token = createdToken });
            }
            else
            {
                return StatusCode(StatusCodes.Status401Unauthorized, new { token = "" });
            }
        }
    }
}
