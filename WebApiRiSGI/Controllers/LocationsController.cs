using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiRiSGI.Models;

namespace WebApiRiSGI.Controllers
{
    [EnableCors("CorsRules")]
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class LocationsController : ControllerBase
    {
        public readonly SgiContext _dbcontext;

        public LocationsController(SgiContext _context)
        {
            _dbcontext = _context;
        }

        [HttpGet]
        [Route("ListLocations")]
        public IActionResult ListLocations()
        {
            List<Localidades> list = new List<Localidades>();

            try
            {
                list = _dbcontext.Localidades.ToList();

                return StatusCode(StatusCodes.Status200OK, new { message = "ok", response = list });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status200OK, new { message = ex.Message, response = list });
            }
        }

        [HttpGet]
        [Route("GetLocation")]
        public IActionResult GetLocation([FromQuery] int? LocalidadID, [FromQuery] string? Localidad)
        {
            IQueryable<Localidades> query = _dbcontext.Localidades.AsQueryable();

            if (LocalidadID != null)
            {
                query = query.Where(p => p.LocalidadId == LocalidadID);
            }

            if (!string.IsNullOrEmpty(Localidad))
            {
                query = query.Where(p => p.Localidad == Localidad);
            }

        
            Localidades olocalidad = query.FirstOrDefault();
            if (olocalidad != null)
            {
                return StatusCode(StatusCodes.Status200OK, new { message = "ok", response = olocalidad });
            }
            else
            {
                return BadRequest("Activo no encontrado");
            }
        }


        [HttpPost]
        [Route("SaveLocation")]
        public IActionResult SaveLocation([FromBody] Localidades objeto)
        {
            try
            {
                // Validate if non-null values from three fields don't exist in the database
                if (!_dbcontext.Localidades.Any(a =>
                    (objeto.Localidad != null && a.Localidad == objeto.Localidad)))
                {

                    _dbcontext.Localidades.Add(objeto);
                    _dbcontext.SaveChanges();

                    return StatusCode(StatusCodes.Status200OK, new { message = "ok" });
                }
                else
                {

                    return BadRequest("Esta Localidad ya se encuentra registrada.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }
    }
}
