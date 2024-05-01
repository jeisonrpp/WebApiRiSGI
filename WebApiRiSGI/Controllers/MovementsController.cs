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
    //[Authorize]
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
            public MovimientoSaveModel Movimiento { get; set; }
            public string DomainUser { get; set; }
        }

        //        [HttpPost]
        //        [Route("SaveMovement")]
        //        public IActionResult SaveMovement([FromBody] MovimientosRequest oMovimiento)
        //        {
        //            try
        //            {

        //                string domainUser = oMovimiento.DomainUser;
        //                _dbcontext.Movimientos.Add(oMovimiento.Movimiento);
        //                _dbcontext.SaveChanges();


        //                int departmentId = _dbcontext.Areas
        //                    .Where(a => a.AreaId == oMovimiento.Movimiento.AreaId_Destino)
        //                    .Select(a => a.DepartamentoId)
        //                    .FirstOrDefault();

        //                int organId = _dbcontext.Departamentos
        //                    .Where(d => d.DepartamentoId == departmentId)
        //                    .Select(d => d.OrganoId)
        //                    .FirstOrDefault();

        //                var AsignacionesEntity = _dbcontext.Asignaciones.FirstOrDefault(d => d.ActivosId == oMovimiento.Movimiento.ActivoId); 

        //                if (AsignacionesEntity != null)
        //                {
        //                    AsignacionesEntity.DomainUser = oMovimiento.DomainUser;
        //                    AsignacionesEntity.DisplayName = oMovimiento.Movimiento.UsuarioDestino;
        //                    AsignacionesEntity.LocalidadId = oMovimiento.Movimiento.LocalidadId_Destino;
        //                    AsignacionesEntity.OrganoID = organId; 
        //                    AsignacionesEntity.AreaId = oMovimiento.Movimiento.AreaId_Destino; 
        //                    AsignacionesEntity.FechaAsignacion = DateTime.Now; 

        //                    _dbcontext.Asignaciones.Update(AsignacionesEntity);
        //                    _dbcontext.SaveChanges(); 


        //                    return StatusCode(StatusCodes.Status200OK, new { message = "ok" });

        //                }
        //                else
        //                {
        //                    return BadRequest("Non-null values already exist in the database.");
        //                }
        //            }

        //            catch (Exception ex)
        //            {
        //                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
        //            }        
        //}


        [HttpGet]
        [Route("GetMovementIdentifier")]
        public IActionResult GetMovementIdentifier()
        {
            Movimientos oMovimiento = new Movimientos();

            try
            {
                // Get the last movimiento ID
                int lastMovimientoId = _dbcontext.Movimientos
                    .OrderByDescending(m => m.MovimientoId)
                    .Select(m => m.MovimientoId)
                    .FirstOrDefault();

                // Generate new movimiento value
                string nuevoMovimiento = "MOV0" + (lastMovimientoId + 1).ToString().PadLeft(2, '0');

                // Assign the new movimiento value
                
                oMovimiento.Movimiento = nuevoMovimiento;

                if (oMovimiento != null)
                {
                    return StatusCode(StatusCodes.Status200OK, new { message = "ok", response = oMovimiento.Movimiento });
                }
                else
                {
                    return BadRequest("Activo no encontrado");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status200OK, new { message = ex.Message, response = oMovimiento.Movimiento });
            }
        }
        [HttpPost]
        [Route("SaveMovement")]
        public IActionResult SaveMovement([FromBody] MovimientosRequest oMovimiento)
        {
            try
            {
                //    // Get the last movimiento ID
                //    int lastMovimientoId = _dbcontext.Movimientos
                //        .OrderByDescending(m => m.MovimientoId)
                //        .Select(m => m.MovimientoId)
                //        .FirstOrDefault();

                //    // Generate new movimiento value
                //    string nuevoMovimiento = "MOV0" + (lastMovimientoId + 1).ToString().PadLeft(2, '0');

                //    // Assign the new movimiento value
                //    oMovimiento.Movimiento.Movimiento = nuevoMovimiento;

                Movimientos newMovimiento = new Movimientos();

                int localidaddestinoid = _dbcontext.Localidades
                  .Where(a => a.Localidad == oMovimiento.Movimiento.localidad_Destino)
                  .Select(a => a.LocalidadId)
                  .FirstOrDefault();

                int areadestinoid = _dbcontext.Areas
                .Where(a => a.AreaNombre == oMovimiento.Movimiento.area_Destino)
                .Select(a => a.AreaId)
                .FirstOrDefault();

                int localidadremitenteid = _dbcontext.Localidades
                .Where(a => a.Localidad == oMovimiento.Movimiento.localidad_Remitente)
                .Select(a => a.LocalidadId)
                .FirstOrDefault();

                int arearemitenteid = _dbcontext.Areas
                .Where(a => a.AreaNombre == oMovimiento.Movimiento.area_Remitente)
                .Select(a => a.AreaId)
                .FirstOrDefault();

                int departmentId = _dbcontext.Areas
                     .Where(a => a.AreaId == areadestinoid)
                     .Select(a => a.DepartamentoId)
                     .FirstOrDefault();

                int organId = _dbcontext.Departamentos
                    .Where(d => d.DepartamentoId == departmentId)
                    .Select(d => d.OrganoId)
                    .FirstOrDefault();

                var newMovimientoSave = new Movimientos
                {
                    Movimiento = oMovimiento.Movimiento.Movimiento,
                    ActivoId = oMovimiento.Movimiento.Activoid,
                    LocalidadId_Destino = localidaddestinoid,
                    AreaId_Destino = areadestinoid,
                    UsuarioDestino = oMovimiento.Movimiento.usuarioDestino,
                    LocalidadId_Remitente = localidadremitenteid,
                    AreaId_Remitente = arearemitenteid,
                    Observacion = oMovimiento.Movimiento.observacion,
                    UsuarioRemitente = oMovimiento.Movimiento.usuarioRemitente,
                    MovimientoTipo = "Definitivo",
                    Fecha = DateTime.Now


                };

                // Add movimiento to the Movimientos table
                _dbcontext.Movimientos.Add(newMovimientoSave);
                _dbcontext.SaveChanges();

                // Update AsignacionesEntity if found
                var AsignacionesEntity = _dbcontext.Asignaciones.FirstOrDefault(d => d.ActivosId == oMovimiento.Movimiento.Activoid);
                if (AsignacionesEntity != null)
                {
                    // Update AsignacionesEntity fields
                    AsignacionesEntity.DomainUser = oMovimiento.DomainUser;
                    AsignacionesEntity.DisplayName = oMovimiento.Movimiento.usuarioDestino;
                    AsignacionesEntity.LocalidadId = localidaddestinoid;
                    AsignacionesEntity.OrganoID = organId;
                    AsignacionesEntity.AreaId = areadestinoid;
                    AsignacionesEntity.FechaAsignacion = DateTime.Now;

                    // Save changes to AsignacionesEntity
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

        public class MovimientoSaveModel
        {
            public string Movimiento { get; set; }
            public int Activoid { get; set; }
            public string localidad_Destino { get; set; }
            public string area_Destino { get; set; }
            public string usuarioDestino { get; set; }
            public string localidad_Remitente { get; set; }
            public string area_Remitente { get; set; }
            public string observacion { get; set; }
            public string usuarioRemitente { get; set; }
            public string movimientoTipo { get; set; }
            public DateTime fecha { get; set; }
            public string domainUser { get; set; }

        }

    }
}
