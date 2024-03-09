using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static WebApiRiSGI.Authentication.AdAuthenticationService;
using WebApiRiSGI.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.EntityFrameworkCore;
using WebApiRiSGI.Models;

namespace WebApiRiSGI.Controllers
{
    [EnableCors("CorsRules")]
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly AdAuthenticationService _adAuthenticationService;
        public readonly SgiContext _dbcontext;

        public UsersController(AdAuthenticationService adAuthenticationService, SgiContext dbcontext)
        {
            _adAuthenticationService = adAuthenticationService;
            _dbcontext = dbcontext;
        }


        [HttpGet("userinfo/{username}")]
        public IActionResult GetUserInfo(string username)
        {
            UserInfoModel userInfo = _adAuthenticationService.GetUserInfo(username);

            if (userInfo != null)
            {
                return Ok(userInfo);
            }
            else
            {
                return NotFound($"User with username {username} not found.");
            }
        }

        [HttpGet]
        [Route("ListUsers")]
        public IActionResult ListUsers()
        {
            List<Usuarios> list = new List<Usuarios>();

            try
            {
                list = _dbcontext.Usuarios.ToList();

                return StatusCode(StatusCodes.Status200OK, new { message = "ok", response = list });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status200OK, new { message = ex.Message, response = list });
            }
        }

        [HttpGet]
        [Route("ListRoles")]
        public IActionResult ListRoles()
        {
            List<Roles> list = new List<Roles>();

            try
            {
                list = _dbcontext.Roles.ToList();

                return StatusCode(StatusCodes.Status200OK, new { message = "ok", response = list });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status200OK, new { message = ex.Message, response = list });
            }
        }


        [HttpGet]
        [Route("ListUserRoles")]
        public IActionResult ListUserRoles()
        {
            List<RolesUsuarios> list = new List<RolesUsuarios>();

            try
            {
                list = _dbcontext.RolesUsuarios.ToList();

                return StatusCode(StatusCodes.Status200OK, new { message = "ok", response = list });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status200OK, new { message = ex.Message, response = list });
            }
        }

        [HttpGet]
        [Route("GetUsers")]
        public IActionResult GetUsers([FromQuery] string? UserLogin)
        {
            IQueryable<Usuarios> query = _dbcontext.Usuarios.AsQueryable();

            if (!string.IsNullOrEmpty(UserLogin))
            {
                query = query.Where(p => p.UserLogin == UserLogin);
            }

          
            Usuarios oUsuarios = query.FirstOrDefault();
            if (oUsuarios != null)
            {
                return StatusCode(StatusCodes.Status200OK, new { message = "ok", response = oUsuarios });
            }
            else
            {
                return BadRequest("Activo no encontrado");
            }
        }


       
    }


}


