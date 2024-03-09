using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiRiSGI.Models;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.Extensions.Configuration;
using System;
using WebApiRiSGI.Authentication;
using Azure.Core;
using static WebApiRiSGI.Authentication.AdAuthenticationService;

namespace WebApiRiSGI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly string secretKey;
        private readonly SgiContext _dbcontext;

        public AuthenticationController(IConfiguration config, SgiContext dbcontext, AdAuthenticationService adAuthenticationService)
        {
            secretKey = config.GetSection("settings").GetSection("secretkey").ToString();
            _dbcontext = dbcontext;
            _adAuthenticationService = adAuthenticationService;
        }

        private readonly AdAuthenticationService _adAuthenticationService;

        [HttpPost("validate")]
        public IActionResult ValidateCredentials([FromBody] CredentialsModel credentials)
        {
            string username = credentials.Username;
            string password = credentials.Password;

            Usuarios user = _dbcontext.Usuarios.SingleOrDefault(u => u.UserLogin == username);

            bool isValid = _adAuthenticationService.ValidateCredentials(username, password);

            if (isValid && user != null)
            {
                var keyBytes = Encoding.ASCII.GetBytes(secretKey);
                var claims = new ClaimsIdentity();

                claims.AddClaim(new Claim(ClaimTypes.NameIdentifier, username));

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = claims,
                    Expires = DateTime.UtcNow.AddHours(20),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(keyBytes), SecurityAlgorithms.HmacSha256Signature)
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                var tokenConfig = tokenHandler.CreateToken(tokenDescriptor);

                string createdToken = tokenHandler.WriteToken(tokenConfig);
                var refreshToken = Guid.NewGuid().ToString();

                return StatusCode(StatusCodes.Status200OK, new { bearer = createdToken, refresh_token = refreshToken });
            }
            else
            {
                return StatusCode(StatusCodes.Status401Unauthorized, new { bearer = "" });
            }
        }

    public class CredentialsModel
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }
    }
    }

   