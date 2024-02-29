using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiNetDani.Models;

namespace ApiNetDani.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EquipoController : ControllerBase
    {
        private readonly IesdawdaniContext _dbcontext;


        public EquipoController(IesdawdaniContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        [HttpGet]
        [Route("Lista")]
        public IActionResult Lista() {
            try
            {
                List<Equipo> lista = _dbcontext.Equipo.ToList();
                return Ok(new { mensaje = "ok", response = lista });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
            }
        }

        [HttpGet]
        [Route("Get/{idEquipo:int}")]
        public IActionResult Obtener(int idEquipo)
        {
            try
            {
                Equipo oEquipo = _dbcontext.Equipo.FirstOrDefault(p => p.Id == idEquipo);

                if (oEquipo == null)
                {
                    return NotFound("Equipo con id " + idEquipo + " no encontrado");
                }

                return Ok(new { mensaje = "ok", response = oEquipo });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message } );
            }
        }
    }
}
