using Microsoft.AspNetCore.Mvc;
using SistemaGestionResiduos.Application.Services;
using SistemaGestionResiduos.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SistemaGestionResiduos.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RutasController : ControllerBase
    {
        private readonly RutaService _rutaService;

        public RutasController(RutaService rutaService)
        {
            _rutaService = rutaService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Ruta>>> GetRutas()
        {
            var rutas = await _rutaService.GetAllRutasAsync();
            return Ok(rutas);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Ruta>> GetRuta(int id)
        {
            var ruta = await _rutaService.GetRutaByIdAsync(id);
            if (ruta == null)
            {
                return NotFound();
            }
            return Ok(ruta);
        }

        [HttpPost]
        public async Task<ActionResult<Ruta>> CreateRuta(Ruta ruta)
        {
            var createdRuta = await _rutaService.CreateRutaAsync(ruta);
            return CreatedAtAction(nameof(GetRuta), new { id = createdRuta.Id }, createdRuta);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRuta(int id, Ruta ruta)
        {
            if (id != ruta.Id)
            {
                return BadRequest();
            }

            await _rutaService.UpdateRutaAsync(ruta);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRuta(int id)
        {
            await _rutaService.DeleteRutaAsync(id);
            return Ok();
        }
    }
}
