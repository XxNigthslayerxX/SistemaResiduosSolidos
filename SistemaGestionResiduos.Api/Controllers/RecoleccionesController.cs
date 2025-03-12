using Microsoft.AspNetCore.Mvc;
using SistemaGestionResiduos.Application.Services;
using SistemaGestionResiduos.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SistemaGestionResiduos.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RecoleccionesController : ControllerBase
    {
        private readonly RecoleccionService _recoleccionService;

        public RecoleccionesController(RecoleccionService recoleccionService)
        {
            _recoleccionService = recoleccionService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Recoleccion>>> GetRecolecciones()
        {
            var recolecciones = await _recoleccionService.GetAllRecoleccionesAsync();
            return Ok(recolecciones);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Recoleccion>> GetRecoleccion(int id)
        {
            var recoleccion = await _recoleccionService.GetRecoleccionByIdAsync(id);
            if (recoleccion == null)
            {
                return NotFound();
            }
            return Ok(recoleccion);
        }

        [HttpPost]
        public async Task<ActionResult<Recoleccion>> CreateRecoleccion(Recoleccion recoleccion)
        {
            var createdRecoleccion = await _recoleccionService.CreateRecoleccionAsync(recoleccion);
            return CreatedAtAction(nameof(GetRecoleccion), new { id = createdRecoleccion.Id }, createdRecoleccion);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRecoleccion(int id, Recoleccion recoleccion)
        {
            if (id != recoleccion.Id)
            {
                return BadRequest();
            }

            await _recoleccionService.UpdateRecoleccionAsync(recoleccion);
            return Ok(id);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRecoleccion(int id)
        {
            await _recoleccionService.DeleteRecoleccionAsync(id);
            return Ok(id);
        }
    }
}
