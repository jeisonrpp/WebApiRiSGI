using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using WebApiRiSGI.Models;
using static WebApiRiSGI.Controllers.MovementsController;

namespace WebApiRiSGI.Controllers
{
    [EnableCors("CorsRules")]
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class DischargeController : ControllerBase
    {
        public readonly SgiContext _dbcontext;

        public DischargeController(SgiContext _context)
        {
            _dbcontext = _context;
        }


        [HttpGet]
        [Route("ListDischarges")]
        public IActionResult ListDischarges()
        {
            List<DescargosView> list = new List<DescargosView>();

            try
            {
                list = _dbcontext.DescargosView.ToList();

                return StatusCode(StatusCodes.Status200OK, new { message = "ok", response = list });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status200OK, new { message = ex.Message, response = list });
            }
        }

        [HttpGet]
        [Route("GetDischarges")]
        public IActionResult GetDischarges([FromQuery] string? Codigo, [FromQuery] string? ActivoPrincipal, [FromQuery] string? ActivoSecundario, [FromQuery] string? Serial)
        {
            IQueryable<DescargosView> query = _dbcontext.DescargosView.AsQueryable();

            if (!string.IsNullOrEmpty(ActivoPrincipal))
            {
                query = query.Where(p => p.ActivoPrincipal == ActivoPrincipal);
            }

            if (!string.IsNullOrEmpty(ActivoSecundario))
            {
                query = query.Where(p => p.ActivoSecundario == ActivoSecundario);
            }

            if (!string.IsNullOrEmpty(Serial))
            {
                query = query.Where(p => p.Serial == Serial);
            }


            if (!string.IsNullOrEmpty(Codigo))
            {
                query = query.Where(p => p.Codigo == Codigo);
            }


            DescargosView oDescargos = query.FirstOrDefault();
            if (oDescargos != null)
            {
                return StatusCode(StatusCodes.Status200OK, new { message = "ok", response = oDescargos });
            }
            else
            {
                return BadRequest("Activo no encontrado");
            }
        }

        [HttpPost]
        [Route("SaveDischarges")]
        public IActionResult SaveMovement([FromBody] Descargos oDescargos)
        {
            try
            {
                _dbcontext.Descargos.Add(oDescargos);
                _dbcontext.SaveChanges();

                var AsignacionesEntity = _dbcontext.Asignaciones.FirstOrDefault(a => a.ActivosId == oDescargos.ActivoId);

                if (AsignacionesEntity != null)
                {
                    _dbcontext.Asignaciones.Remove(AsignacionesEntity);
                    _dbcontext.SaveChanges();

                    return StatusCode(StatusCodes.Status200OK, new { message = "ok" });
                }
                else
                {
                    return BadRequest("Non-null values already exist in the database.");
                }
            }

            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }
        }
}
