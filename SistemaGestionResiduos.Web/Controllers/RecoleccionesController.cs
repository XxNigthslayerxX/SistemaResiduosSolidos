using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SistemaGestionResiduos.Application.Services;
using SistemaGestionResiduos.Domain.Entities;
using System.Threading.Tasks;

namespace SistemaGestionResiduos.Web.Controllers
{
    public class RecoleccionesController : Controller
    {
        private readonly RecoleccionService _recoleccionService;
        private readonly ContenedorService _contenedorService;

        public RecoleccionesController(RecoleccionService recoleccionService, ContenedorService contenedorService)
        {
            _recoleccionService = recoleccionService;
            _contenedorService = contenedorService;
        }

        public async Task<IActionResult> Index()
        {
            var recolecciones = await _recoleccionService.GetAllRecoleccionesAsync();
            return View(recolecciones);
        }

        public async Task<IActionResult> Create()
        {
            var contenedores = await _contenedorService.GetAllContenedoresAsync();
            ViewBag.Contenedores = new SelectList(contenedores, "Id", "Ubicacion");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Recoleccion recoleccion)
        {
            if (ModelState.IsValid)
            {
                await _recoleccionService.CreateRecoleccionAsync(recoleccion);
                return RedirectToAction(nameof(Index));
            }
            var contenedores = await _contenedorService.GetAllContenedoresAsync();
            ViewBag.Contenedores = new SelectList(contenedores, "Id", "Ubicacion");
            return View(recoleccion);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var recoleccion = await _recoleccionService.GetRecoleccionByIdAsync(id);
            if (recoleccion == null)
            {
                return NotFound();
            }
            var contenedores = await _contenedorService.GetAllContenedoresAsync();
            ViewBag.Contenedores = new SelectList(contenedores, "Id", "Ubicacion");
            return View(recoleccion);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Recoleccion recoleccion)
        {
            if (id != recoleccion.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _recoleccionService.UpdateRecoleccionAsync(recoleccion);
                return RedirectToAction(nameof(Index));
            }
            var contenedores = await _contenedorService.GetAllContenedoresAsync();
            ViewBag.Contenedores = new SelectList(contenedores, "Id", "Ubicacion");
            return View(recoleccion);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var recoleccion = await _recoleccionService.GetRecoleccionByIdAsync(id);
            if (recoleccion == null)
            {
                return NotFound();
            }
            return View(recoleccion);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _recoleccionService.DeleteRecoleccionAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
