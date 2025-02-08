using Microsoft.AspNetCore.Mvc;
using SistemaGestionResiduos.Application.Services;
using SistemaGestionResiduos.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SistemaGestionResiduos.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContenedoresController : ControllerBase
    {
        private readonly ContenedorService _contenedorService;

        public ContenedoresController(ContenedorService contenedorService)
        {
            _contenedorService = contenedorService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Contenedor>>> GetContenedores()
        {
            var contenedores = await _contenedorService.GetAllContenedoresAsync();
            return Ok(contenedores);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Contenedor>> GetContenedor(int id)
        {
            var contenedor = await _contenedorService.GetContenedorByIdAsync(id);
            if (contenedor == null)
            {
                return NotFound();
            }
            return Ok(contenedor);
        }

        [HttpPost]
        public async Task<ActionResult<Contenedor>> CreateContenedor(Contenedor contenedor)
        {
            var createdContenedor = await _contenedorService.CreateContenedorAsync(contenedor);
            return CreatedAtAction(nameof(GetContenedor), new { id = createdContenedor.Id }, createdContenedor);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateContenedor(int id, Contenedor contenedor)
        {
            if (id != contenedor.Id)
            {
                return BadRequest();
            }

            await _contenedorService.UpdateContenedorAsync(contenedor);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContenedor(int id)
        {
            await _contenedorService.DeleteContenedorAsync(id);
            return NoContent();
        }
    }
}
