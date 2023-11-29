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
    [Authorize]
    [ApiController]
    public class ActivesController : ControllerBase
    {
        public readonly SgiContext _dbcontext;

        public ActivesController(SgiContext _context) 
        {
            _dbcontext = _context; 
        }
        [HttpGet]
        [Route("spActives")]
        public IActionResult spActives(int id)
        {
      
          
            try
            {
                List<Activos> actives = new List<Activos>();

                SqlConnection con = (SqlConnection)_dbcontext.Database.GetDbConnection();
                SqlCommand command = con.CreateCommand();
                con.Open();
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandText = "spGetActivos";
                command.Parameters.Add("@activoid", System.Data.SqlDbType.Int).Value = id;
                SqlDataReader reader = command.ExecuteReader();
                while(reader.Read())
                {
                    Activos active = new Activos();
                    active.ActivoPrincipal = (string)reader["ActivoPrincipal"];
                    active.Descripcion = (string)reader["Acttivos"];
                    actives.Add(active);
                }
                con.Close();

                return StatusCode(StatusCodes.Status200OK, new { message = "ok", actives });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status200OK, new { message = ex.Message });
            }
        }


        [HttpGet]
        [Route("ListActives")]
        public IActionResult ListActives()
        {
            List<Activos> list = new List<Activos>();

            try
            {
                list = _dbcontext.Activos.ToList();

                return StatusCode(StatusCodes.Status200OK, new { message = "ok", response = list });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status200OK, new { message = ex.Message, response = list });
            }
        }

        [HttpGet]
        [Route("GetActive/{Activesid:int}")]
        public IActionResult GetActives(int Activesid)
        {
            Activos oActivos = _dbcontext.Activos.Find(Activesid);

            if (oActivos == null)
            {
                return BadRequest("Activo no encontrado");
            }

            try
            {
                oActivos = _dbcontext.Activos.Where(p => p.ActivosId == Activesid).FirstOrDefault();
             
                return StatusCode(StatusCodes.Status200OK, new { message = "ok", response = oActivos });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status200OK, new { message = ex.Message, response = oActivos });
            }
        }

        [HttpPost]
        [Route("SaveActive")]

        public IActionResult Save([FromBody] Activos objeto)
        {
            try
            {
                _dbcontext.Activos.Add(objeto);
                _dbcontext.SaveChanges();

                return StatusCode(StatusCodes.Status200OK, new { message = "ok"});
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status200OK, new { message = ex.Message });
            }
        }


        [HttpPut]
        [Route("EditActive")]

        public IActionResult Edit([FromBody] Activos objeto)
        {
            Activos oActivo = _dbcontext.Activos.Find(objeto.ActivosId);

            if (oActivo == null)
            {
                return BadRequest("Activo no encontrado");
            }

            try
            {
                oActivo.ActivoPrincipal = objeto.ActivoPrincipal is null? oActivo.ActivoPrincipal : objeto.ActivoPrincipal;
                oActivo.ActivoSecundario = objeto.ActivoSecundario is null? oActivo.ActivoSecundario : objeto.ActivoSecundario;
                oActivo.Serial = objeto.Serial is null? oActivo.Serial : objeto.Serial;
                oActivo.TipoActivo = objeto.TipoActivo is null? oActivo.TipoActivo : objeto.TipoActivo;
                oActivo.Descripcion = objeto.Descripcion is null? oActivo.Descripcion : objeto.Descripcion;
                oActivo.MarcaActivo = objeto.MarcaActivo is null? oActivo.MarcaActivo : objeto.MarcaActivo;
                oActivo.ModeloActivo = objeto.ModeloActivo is null? oActivo.ModeloActivo : objeto.ModeloActivo;

                _dbcontext.Activos.Update(oActivo);
                _dbcontext.SaveChanges();

                return StatusCode(StatusCodes.Status200OK, new { message = "ok" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status200OK, new { message = ex.Message });
            }
        }

        [HttpDelete]
        [Route("DeleteActive/{Activesid:int}")]
        public IActionResult DeleteActive(int Activesid)
        {
            Activos oActivo = _dbcontext.Activos.Find(Activesid);

            if (oActivo == null)
            {
                return BadRequest("Activo no encontrado");
            }

            try
            {
                _dbcontext.Activos.Remove(oActivo);
                _dbcontext.SaveChanges();

                return StatusCode(StatusCodes.Status200OK, new { message = "ok" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status200OK, new { message = ex.Message });
            }
        }
    }
}
