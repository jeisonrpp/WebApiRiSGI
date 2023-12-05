using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiRiSGI.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Authorization;

namespace WebApiRiSGI.Controllers
{
    [EnableCors("CorsRules")]
    [Route("api/[controller]")]
  //  [Authorize]
    [ApiController]
    public class SGIAssetsController : ControllerBase
    {
        public readonly SgiContext _dbcontext;

        public SGIAssetsController(SgiContext _context) 
        {
            _dbcontext = _context; 
        }
    
        [HttpGet]
        [Route("ListAssets")]
        public IActionResult ListAssets()
        {
            List<ActivosView> list = new List<ActivosView>();

            try
            {
                list = _dbcontext.ActivosView.ToList();

                return StatusCode(StatusCodes.Status200OK, new { response = list, message = "ok" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status200OK, new {  response = list, message = ex.Message });
            }
        }

        [HttpGet]
        [Route("ListBrands")]
        public IActionResult ListBrands()
        {
            List<Marcas> list = new List<Marcas>();

            try
            {
                list = _dbcontext.Marcas.ToList();

                return StatusCode(StatusCodes.Status200OK, new { message = "ok", response = list });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status200OK, new { message = ex.Message, response = list });
            }
        }

        [HttpGet]
        [Route("ListModels")]
        public IActionResult ListModels()
        {
            List<Modelos> list = new List<Modelos>();

            try
            {
                list = _dbcontext.Modelos.ToList();

                return StatusCode(StatusCodes.Status200OK, new { message = "ok", response = list });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status200OK, new { message = ex.Message, response = list });
            }
        }

        [HttpGet]
        [Route("ListAssetsTypes")]
        public IActionResult ListAssetsTypes()
        {
            List<TipoActivo> list = new List<TipoActivo>();

            try
            {
                list = _dbcontext.TipoActivo.ToList();

                return StatusCode(StatusCodes.Status200OK, new { message = "ok", response = list });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status200OK, new { message = ex.Message, response = list });
            }
        }


        [HttpGet]
        [Route("GetAssets")]
        public IActionResult GetAssets([FromQuery] string? ActivoPrincipal, [FromQuery] string? ActivoSecundario, [FromQuery] string? Serial, [FromQuery] string? Localidad, [FromQuery] string? Departamento, [FromQuery] string? Area)
        {
            IQueryable<ActivosView> query = _dbcontext.ActivosView.AsQueryable();

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


            if (!string.IsNullOrEmpty(Localidad))
            {
                query = query.Where(p => p.Localidad == Localidad);
            }

            if (!string.IsNullOrEmpty(Departamento))
            {
                query = query.Where(p => p.DepartamentoNombre == Departamento);
            }

            if (!string.IsNullOrEmpty(Area))
            {
                query = query.Where(p => p.AreaNombre == Area);
            }

            ActivosView oActivos = query.FirstOrDefault();
            if (oActivos != null)
            {
                return StatusCode(StatusCodes.Status200OK, new { message = "ok", response = oActivos });
            }
            else
            {
                return BadRequest("Activo no encontrado");
            }           
        }

        [HttpGet]
        [Route("GetBrands")]
        public IActionResult GetBrands([FromQuery] int? MarcaID, [FromQuery] string? Marca)
        {
            IQueryable<Marcas> query = _dbcontext.Marcas.AsQueryable();

            if (MarcaID != null)
            {
                query = query.Where(p => p.MarcaId == MarcaID);
            }

            if (!string.IsNullOrEmpty(Marca))
            {
                query = query.Where(p => p.Marca1 == Marca);
            }


            Marcas oMarca = query.FirstOrDefault();
            if (oMarca != null)
            {
                return StatusCode(StatusCodes.Status200OK, new { message = "ok", response = oMarca });
            }
            else
            {
                return BadRequest("Marca no encontrada");
            }
        }

        [HttpGet]
        [Route("GetModels")]
        public IActionResult GetModels([FromQuery] int? ModeloID, [FromQuery] string? Modelo)
        {
            IQueryable<Modelos> query = _dbcontext.Modelos.AsQueryable();

            if (ModeloID != null)
            {
                query = query.Where(p => p.ModeloId == ModeloID);
            }

            if (!string.IsNullOrEmpty(Modelo))
            {
                query = query.Where(p => p.Modelo1 == Modelo);
            }


            Modelos oModelo = query.FirstOrDefault();
            if (oModelo != null)
            {
                return StatusCode(StatusCodes.Status200OK, new { message = "ok", response = oModelo });
            }
            else
            {
                return BadRequest("Modelo no encontrado");
            }
        }

        [HttpGet]
        [Route("GetAssetsType")]
        public IActionResult GetAssetsType([FromQuery] int? TipoID, [FromQuery] string? Tipo)
        {
            IQueryable<TipoActivo> query = _dbcontext.TipoActivo.AsQueryable();

            if (TipoID != null)
            {
                query = query.Where(p => p.TipoId == TipoID);
            }

            if (!string.IsNullOrEmpty(Tipo))
            {
                query = query.Where(p => p.TipoNombre == Tipo);
            }


            TipoActivo oTipo = query.FirstOrDefault();
            if (oTipo != null)
            {
                return StatusCode(StatusCodes.Status200OK, new { message = "ok", response = oTipo });
            }
            else
            {
                return BadRequest("Tipo de Activo no encontrado");
            }
        }

        [HttpPost]
        [Route("SaveAssets")]
        public IActionResult SaveAssets([FromBody] Activos objeto)
        {
            try
            {
                // Validate if non-null values from three fields don't exist in the database
                if (!_dbcontext.Activos.Any(a =>
                    (objeto.ActivoPrincipal != null && a.ActivoPrincipal == objeto.ActivoPrincipal) ||
                    (objeto.ActivoSecundario != null && a.ActivoSecundario == objeto.ActivoSecundario) ||
                    (objeto.Serial != null && a.Serial == objeto.Serial)))
                {
 
                    _dbcontext.Activos.Add(objeto);
                    _dbcontext.SaveChanges();

        
                    Asignaciones asignacion = new Asignaciones
                    {
                        ActivosId = objeto.ActivosId, 
                        DomainUser = "rmiranda",
                        DisplayName = "Ramón Antonio Miranda Angeles",//que traiga por default nombre encargado de almacen
                        LocalidadId = 1,  
                        OrganoID = 3,
                        AreaId = 1,
                        FechaAsignacion = DateTime.Now
                    };

                    _dbcontext.Asignaciones.Add(asignacion);
                    _dbcontext.SaveChanges();  // Save changes for the second insertion

                    return StatusCode(StatusCodes.Status200OK, new { message = "ok" });
                }
                else
                {
    
                    return BadRequest("Este Activo ya se encuentra registrado.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }

        [HttpPost]
        [Route("SaveBrand")]
        public IActionResult SaveBrand([FromBody] Marcas objeto)
        {
            try
            {
                // Validate if non-null values from three fields don't exist in the database
                if (!_dbcontext.Marcas.Any(a =>
                    (objeto.Marca1 != null && a.Marca1 == objeto.Marca1)))
                {

                    _dbcontext.Marcas.Add(objeto);
                    _dbcontext.SaveChanges();                  

                    return StatusCode(StatusCodes.Status200OK, new { message = "ok" });
                }
                else
                {

                    return BadRequest("Esta Marca ya se encuentra registrada.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }

        [HttpPost]
        [Route("SaveModel")]
        public IActionResult SaveModel([FromBody] Modelos objeto)
        {
            try
            {
                // Validate if non-null values from three fields don't exist in the database
                if (!_dbcontext.Modelos.Any(a =>
                    (objeto.Modelo1 != null && a.Modelo1 == objeto.Modelo1)))
                {

                    _dbcontext.Modelos.Add(objeto);
                    _dbcontext.SaveChanges();

                    return StatusCode(StatusCodes.Status200OK, new { message = "ok" });
                }
                else
                {

                    return BadRequest("Este Modelo ya se encuentra registrada.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }

        [HttpPost]
        [Route("SaveAssetsType")]
        public IActionResult SaveAssetsType([FromBody] TipoActivo objeto)
        {
            try
            {
                // Validate if non-null values from three fields don't exist in the database
                if (!_dbcontext.TipoActivo.Any(a =>
                    (objeto.TipoNombre != null && a.TipoNombre == objeto.TipoNombre)))
                {

                    _dbcontext.TipoActivo.Add(objeto);
                    _dbcontext.SaveChanges();

                    return StatusCode(StatusCodes.Status200OK, new { message = "ok" });
                }
                else
                {

                    return BadRequest("Este Tipo de Activo ya se encuentra registrada.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }

        [HttpPut]
        [Route("EditAssets")]
        public IActionResult EditAssets([FromBody] Activos objeto)
        {
            Activos oActivo = null;

            // Check which property is provided and find the corresponding record
            if (objeto.ActivoPrincipal != null)
            {
                oActivo = _dbcontext.Activos.FirstOrDefault(a => a.ActivoPrincipal == objeto.ActivoPrincipal);
            }
            else if (objeto.ActivoSecundario != null)
            {
                oActivo = _dbcontext.Activos.FirstOrDefault(a => a.ActivoSecundario == objeto.ActivoSecundario);
            }
            else if (objeto.Serial != null)
            {
                oActivo = _dbcontext.Activos.FirstOrDefault(a => a.Serial == objeto.Serial);
            }

            if (oActivo == null)
            {
                return BadRequest("Activo no encontrado");
            }

            try
            {
                // Update only the fields that have been modified
                oActivo.ActivoPrincipal = objeto.ActivoPrincipal ?? oActivo.ActivoPrincipal;
                oActivo.ActivoSecundario = objeto.ActivoSecundario ?? oActivo.ActivoSecundario;
                oActivo.Serial = objeto.Serial ?? oActivo.Serial;
                oActivo.TipoActivo = objeto.TipoActivo ?? oActivo.TipoActivo;
                oActivo.Descripcion = objeto.Descripcion ?? oActivo.Descripcion;
                oActivo.MarcaActivo = objeto.MarcaActivo ?? oActivo.MarcaActivo;
                oActivo.ModeloActivo = objeto.ModeloActivo ?? oActivo.ModeloActivo;

                _dbcontext.Activos.Update(oActivo);
                _dbcontext.SaveChanges();

                return StatusCode(StatusCodes.Status200OK, new { message = "ok" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status200OK, new { message = ex.Message });
            }
        }



        //[HttpDelete]
        //[Route("DeleteActive/{ActivoPrincipal}")]
        //public IActionResult DeleteActive(string ActivoPrincipal)
        //{
        //    Activos oActivo = _dbcontext.Activos.Find(ActivoPrincipal);

        //    if (oActivo == null)
        //    {
        //        return BadRequest("Activo no encontrado");
        //    }

        //    try
        //    {
        //        _dbcontext.Activos.Remove(oActivo);
        //        _dbcontext.SaveChanges();

        //        return StatusCode(StatusCodes.Status200OK, new { message = "ok" });
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(StatusCodes.Status200OK, new { message = ex.Message });
        //    }
        //}
    }
}
