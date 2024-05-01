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
    public class DepartmentController : ControllerBase
    {
        public readonly SgiContext _dbcontext;

        public DepartmentController(SgiContext _context)
        {
            _dbcontext = _context;
        }

        [HttpGet]
        [Route("ListOrganos")]
        public IActionResult ListOrganos()
        {
            List<Organos> list = new List<Organos>();

            try
            {
                list = _dbcontext.Organos.ToList();

                return StatusCode(StatusCodes.Status200OK, new { message = "ok", response = list });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status200OK, new { message = ex.Message, response = list });
            }
        }

        [HttpGet]
        [Route("ListDepartments")]
        public IActionResult ListDepartments()
        {
            List<Departamentos> list = new List<Departamentos>();

            try
            {
                list = _dbcontext.Departamentos.ToList();

                return StatusCode(StatusCodes.Status200OK, new { message = "ok", response = list });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status200OK, new { message = ex.Message, response = list });
            }
        }

        [HttpGet]
        [Route("ListAreas")]
        public IActionResult ListAreas()
        {
            List<Areas> list = new List<Areas>();

            try
            {
                list = _dbcontext.Areas.ToList();

                return StatusCode(StatusCodes.Status200OK, new { message = "ok", response = list });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status200OK, new { message = ex.Message, response = list });
            }
        }

        [HttpGet]
        [Route("GetOrganos")]
        public IActionResult GetOrganos([FromQuery] int? OrganoID, [FromQuery] string? Organo)
        {
            IQueryable<Organos> query = _dbcontext.Organos.AsQueryable();

            if (OrganoID != null)
            {
                query = query.Where(p => p.OrganoId == OrganoID);
            }

            if (!string.IsNullOrEmpty(Organo))
            {
                query = query.Where(p => p.Organo1 == Organo);
            }


            Organos oOrgano = query.FirstOrDefault();
            if (oOrgano != null)
            {
                return StatusCode(StatusCodes.Status200OK, new { message = "ok", response = oOrgano });
            }
            else
            {
                return BadRequest("Organo no encontrado");
            }
        }
        [HttpGet]
        [Route("GetDepartament")]
        public IActionResult GetDepartament([FromQuery] int? DepartamentoID, [FromQuery] string? Departamento, [FromQuery] int? LocalidadID)
        {
            IQueryable<Departamentos> query = _dbcontext.Departamentos.AsQueryable();

            if (DepartamentoID != null)
            {
                query = query.Where(p => p.DepartamentoId == DepartamentoID);
            }

            if (!string.IsNullOrEmpty(Departamento))
            {
                query = query.Where(p => p.DepartamentoNombre == Departamento);
            }

            if (LocalidadID != null)
            {
                query = query.Where(p => p.LocalidadId == LocalidadID);
            }

            List<Departamentos> oDepartamentos = query.ToList();
            if (oDepartamentos.Count > 0)
            {
                return StatusCode(StatusCodes.Status200OK, new { message = "ok", response = oDepartamentos });
            }
            else
            {
                return BadRequest("Departamento no encontrado");
            }
        }


        [HttpGet]
        [Route("GetAreas")]
        public IActionResult GetAreas([FromQuery] int? AreaID, [FromQuery] string? Area, [FromQuery] int? DepartamentoID)
        {
            IQueryable<Areas> query = _dbcontext.Areas.AsQueryable();

            if (AreaID != null)
            {
                query = query.Where(p => p.AreaId == AreaID);
            }

            if (!string.IsNullOrEmpty(Area))
            {
                query = query.Where(p => p.AreaNombre == Area);
            }

            if (DepartamentoID != null)
            {
                query = query.Where(p => p.DepartamentoId == DepartamentoID);
            }

            List<Areas> oAreas = query.ToList();
            if (oAreas.Count > 0)
            {
                return StatusCode(StatusCodes.Status200OK, new { message = "ok", response = oAreas });
            }
            else
            {
                return BadRequest("Areas no encontrado");
            }
        }

        [HttpPost]
        [Route("SaveOrgano")]
        public IActionResult SaveOrgano([FromBody] Organos objeto)
        {
            try
            {
                // Validate if non-null values from three fields don't exist in the database
                if (!_dbcontext.Organos.Any(a =>
                    (objeto.Organo1 != null && a.Organo1 == objeto.Organo1)))
                {

                    _dbcontext.Organos.Add(objeto);
                    _dbcontext.SaveChanges();

                    return StatusCode(StatusCodes.Status200OK, new { message = "ok" });
                }
                else
                {

                    return BadRequest("Este Organo ya se encuentra registrado.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }

        [HttpPost]
        [Route("SaveDepartament")]
        public IActionResult SaveDepartament([FromBody] Departamentos objeto)
        {
            try
            {
                // Validate if non-null values from three fields don't exist in the database
                if (!_dbcontext.Departamentos.Any(a =>
                    (objeto.DepartamentoNombre != null && a.DepartamentoNombre == objeto.DepartamentoNombre)))
                {

                    _dbcontext.Departamentos.Add(objeto);
                    _dbcontext.SaveChanges();

                    return StatusCode(StatusCodes.Status200OK, new { message = "ok" });
                }
                else
                {

                    return BadRequest("Este Departamento ya se encuentra registrado.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }

        [HttpPost]
        [Route("SaveAreas")]
        public IActionResult SaveAreas([FromBody] Areas objeto)
        {
            try
            {
                // Validate if non-null values from three fields don't exist in the database
                if (!_dbcontext.Areas.Any(a =>
                    (objeto.AreaNombre != null && a.AreaNombre == objeto.AreaNombre)))
                {

                    _dbcontext.Areas.Add(objeto);
                    _dbcontext.SaveChanges();

                    return StatusCode(StatusCodes.Status200OK, new { message = "ok" });
                }
                else
                {

                    return BadRequest("Esta Area ya se encuentra registrada.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }
    }
}
