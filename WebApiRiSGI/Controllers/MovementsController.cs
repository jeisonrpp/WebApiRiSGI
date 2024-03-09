using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiRiSGI.Models;

namespace WebApiRiSGI.Controllers
{
    [EnableCors("CorsRules")]
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class MovementsController : ControllerBase
    {
        public readonly SgiContext _dbcontext;

        public MovementsController(SgiContext _context)
        {
            _dbcontext = _context;
        }

        [HttpGet]
        [Route("ListMovements")]
        public IActionResult ListMovements()
        {
            List<MovimientoView> list = new List<MovimientoView>();

            try
            {
                list = _dbcontext.Movimientosview.ToList();

                return StatusCode(StatusCodes.Status200OK, new { message = "ok", response = list });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status200OK, new { message = ex.Message, response = list });
            }
        }

        [HttpGet]
        [Route("GetMovements")]
        public IActionResult GetMovements([FromQuery] string? Codigo, [FromQuery] string? ActivoPrincipal, [FromQuery] string? ActivoSecundario, [FromQuery] string? Serial)
        {
            IQueryable<MovimientoView> query = _dbcontext.Movimientosview.AsQueryable();

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

          
            MovimientoView oMovimiento = query.FirstOrDefault();
            if (oMovimiento != null)
            {
                return StatusCode(StatusCodes.Status200OK, new { message = "ok", response = oMovimiento });
            }
            else
            {
                return BadRequest("Activo no encontrado");
            }
        }

        public class MovimientosRequest
        {
            public Movimientos Movimiento { get; set; }
            public string DomainUser { get; set; }
        }

        [HttpPost]
        [Route("SaveMovement")]
        public IActionResult SaveMovement([FromBody] MovimientosRequest oMovimiento)
        {
            try
            {

                string domainUser = oMovimiento.DomainUser;
                _dbcontext.Movimientos.Add(oMovimiento.Movimiento);
                _dbcontext.SaveChanges();


                int departmentId = _dbcontext.Areas
                    .Where(a => a.AreaId == oMovimiento.Movimiento.AreaId_Destino)
                    .Select(a => a.DepartamentoId)
                    .FirstOrDefault();

                int organId = _dbcontext.Departamentos
                    .Where(d => d.DepartamentoId == departmentId)
                    .Select(d => d.OrganoId)
                    .FirstOrDefault();
    
                var AsignacionesEntity = _dbcontext.Asignaciones.FirstOrDefault(d => d.ActivosId == oMovimiento.Movimiento.ActivoId); 

                if (AsignacionesEntity != null)
                {
                    AsignacionesEntity.DomainUser = oMovimiento.DomainUser;
                    AsignacionesEntity.DisplayName = oMovimiento.Movimiento.UsuarioDestino;
                    AsignacionesEntity.LocalidadId = oMovimiento.Movimiento.LocalidadId_Destino;
                    AsignacionesEntity.OrganoID = organId; 
                    AsignacionesEntity.AreaId = oMovimiento.Movimiento.AreaId_Destino; 
                    AsignacionesEntity.FechaAsignacion = DateTime.Now; 

                    _dbcontext.Asignaciones.Update(AsignacionesEntity);
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
