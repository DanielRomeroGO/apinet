using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiNetDani.Models;
using System.Linq;

namespace ApiNetDani.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JugadorController : ControllerBase
    {
        public readonly IesdawdaniContext _dbcontext;

        public JugadorController(IesdawdaniContext dbcontext){
            _dbcontext = dbcontext;
        }

        [HttpGet]
        [Route("Lista")]
        public IActionResult Lista(){
            List<Jugador> lista = new List<Jugador>();
            try
            {
                 lista = _dbcontext.Jugador.ToList();

                return Ok(new { mensaje = "ok", response = lista});
            } 
            catch (Exception ex) {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
            }
        }

        [HttpGet]
        [Route("Get/{idJugador:int}")]
        public IActionResult Obtener(int idJugador)
        {
            try
            {
                Jugador oJugador = _dbcontext.Jugador.Include(c => c.oEquipo).FirstOrDefault(p => p.Id == idJugador);

                if (oJugador == null)
                {
                    return NotFound("Jugador con id " + idJugador + " no encontrado");
                }

                return Ok(new { mensaje = "ok", response = oJugador });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
            }
        }

        [HttpGet]
        [Route("ListarPorEdad")]
        public IActionResult ListarPrecio()
        {
            try
            {
                List<Jugador> lista = _dbcontext.Jugador.Include(c => c.oEquipo).OrderBy(r => r.Edad).ToList();
                return Ok(new { mensaje = "ok", response = lista });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
            }
        }

        [HttpPost]
        [Route("Introducir")]
        [Consumes("multipart/form-data")]
        public IActionResult Guardar([FromForm] JugadorFormData formData)
        {
            try
            {
                if (string.IsNullOrEmpty(formData.Nombre))
                {
                    return BadRequest(new { mensaje = "El campo nombre no puede estar vacío o nulo." });
                }

                if (formData.Edad == null || formData.Edad <= 0)
                {
                    return BadRequest(new { mensaje = "El campo Edad debe tener un valor mayor que 0." });
                }

                if (formData.IdEquipo != null && !_dbcontext.Equipo.Any(m => m.Id == formData.IdEquipo))
                {
                    return BadRequest(new { mensaje = "El campo IdEquipo no existe en la tabla Equipo." });
                }

                if (formData.Bandera != null && formData.Bandera.Length > 0)
                {
                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
                    var fileExtension = Path.GetExtension(formData.Bandera.FileName).ToLower();

                    if (!allowedExtensions.Contains(fileExtension))
                    {
                        return BadRequest(new { mensaje = "El formato de la imagen no es válido. Se admiten solo archivos con extensiones .jpg, .jpeg o .png." });
                    }
                }

                Jugador jugador = new Jugador
                {
                    Nombre = formData.Nombre,
                    Edad = formData.Edad,
                    IdEquipo = formData.IdEquipo,
                    Bandera = formData.Bandera.FileName,
                };

                if (formData.Bandera != null && formData.Bandera.Length > 0)
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        formData.Bandera.CopyTo(ms);
                        byte[] imageBytes = ms.ToArray();
                        jugador.Banderablob = imageBytes;
                    }
                }

                _dbcontext.Jugador.Add(jugador);
                _dbcontext.SaveChanges();

                return Ok(new { mensaje = "ok", Response = jugador });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
            }
        }

        public class JugadorFormData
        {
            public string Nombre { get; set; }
            public int Edad { get; set; }
            public IFormFile Bandera { get; set; }
            public int? IdEquipo { get; set; }
        }

        [HttpPut]
        [Route("EditarJugador/{idJugador:int}")]
        [Consumes("multipart/form-data")]
        public IActionResult EditarJugador(int idJugador, [FromForm] JugadorFormData formData)
        {
            try
            {
                if (formData == null)
                {
                    return BadRequest(new { mensaje = "Los datos del jugador no pueden ser nulos." });
                }

                if (idJugador <= 0)
                {
                    return BadRequest(new { mensaje = "El ID del jugador debe ser mayor que cero." });
                }

                Jugador oJugador = _dbcontext.Jugador.Find(idJugador);

                if (oJugador == null)
                {
                    return BadRequest(new { mensaje = "Jugador no encontrado" });
                }

                if (string.IsNullOrEmpty(formData.Nombre))
                {
                    return BadRequest(new { mensaje = "El campo Nombre no puede estar vacío o nulo." });
                }

                if (formData.Edad == null || formData.Edad <= 0)
                {
                    return BadRequest(new { mensaje = "El campo Edad debe tener un valor mayor que 0." });
                }

                if (formData.IdEquipo != null && !_dbcontext.Equipo.Any(m => m.Id == formData.IdEquipo))
                {
                    return BadRequest(new { mensaje = "El campo IdMarca no existe en la tabla Marcas." });
                }

                if (formData.Bandera != null && formData.Bandera.Length > 0)
                {
                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
                    var fileExtension = Path.GetExtension(formData.Bandera.FileName).ToLower();

                    if (!allowedExtensions.Contains(fileExtension))
                    {
                        return BadRequest(new { mensaje = "El formato de la imagen no es válido. Se admiten solo archivos con extensiones .jpg, .jpeg o .png." });
                    }
                }

                oJugador.Nombre = formData.Nombre;
                oJugador.Edad = formData.Edad;
                oJugador.IdEquipo = formData.IdEquipo;
                oJugador.Bandera = formData.Bandera.FileName;

                if (formData.Bandera != null && formData.Bandera.Length > 0)
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        formData.Bandera.CopyTo(ms);
                        byte[] imageBytes = ms.ToArray();
                        oJugador.Banderablob = imageBytes;
                    }
                }

                _dbcontext.Jugador.Update(oJugador);
                _dbcontext.SaveChanges();

                return Ok(new { mensaje = "ok", Response = oJugador });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = "Error al actualizar el jugador", error = ex.Message });
            }
        }

        [HttpDelete]
        [Route("Eliminar/{idJugador:int}")]
        public IActionResult Eliminar(int idJugador)
        {
            try
            {
                Jugador oJugador = _dbcontext.Jugador.Find(idJugador);

                if (oJugador == null)
                {
                    return NotFound("Jugador con id " + idJugador + " no encontrado");
                }

                _dbcontext.Jugador.Remove(oJugador);
                _dbcontext.SaveChanges();

                return Ok(new { mensaje = "ok" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
            }
        }

        [HttpGet]
        [Route("VerBandera/{id}")]
        public IActionResult VerBandera(int id)
        {
            try
            {
                Jugador jugador = _dbcontext.Jugador.Find(id);

                if (jugador == null || jugador.Bandera == null)
                {
                    return NotFound("Bandera no encontrada");
                }

                return new FileContentResult(jugador.Banderablob, "image/jpeg");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
            }
        }
    }
}
