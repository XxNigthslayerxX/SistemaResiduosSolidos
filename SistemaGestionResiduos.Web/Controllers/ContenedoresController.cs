using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SistemaGestionResiduos.Application.Services;
using SistemaGestionResiduos.Domain.Entities;
using System.Threading.Tasks;

namespace SistemaGestionResiduos.Web.Controllers
{
    public class ContenedoresController : Controller
    {
        private readonly ContenedorService _contenedorService;
        private readonly RutaService _rutaService;

        public ContenedoresController(ContenedorService contenedorService, RutaService rutaService)
        {
            _contenedorService = contenedorService;
            _rutaService = rutaService;
        }

        public async Task<IActionResult> Index()
        {
            var contenedores = await _contenedorService.GetAllContenedoresAsync();
            return View(contenedores);
        }

        public async Task<IActionResult> Create()
        {
            var rutas = await _rutaService.GetAllRutasAsync();
            ViewBag.Rutas = new SelectList(rutas, "Id", "Nombre");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Contenedor contenedor)
        {
            if (ModelState.IsValid)
            {
                await _contenedorService.CreateContenedorAsync(contenedor);
                return RedirectToAction(nameof(Index));
            }
            var rutas = await _rutaService.GetAllRutasAsync();
            ViewBag.Rutas = new SelectList(rutas, "Id", "Nombre");
            return View(contenedor);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var contenedor = await _contenedorService.GetContenedorByIdAsync(id);
            if (contenedor == null)
            {
                return NotFound();
            }
            var rutas = await _rutaService.GetAllRutasAsync();
            ViewBag.Rutas = new SelectList(rutas, "Id", "Nombre");
            return View(contenedor);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Contenedor contenedor)
        {
            if (id != contenedor.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _contenedorService.UpdateContenedorAsync(contenedor);
                return RedirectToAction(nameof(Index));
            }
            var rutas = await _rutaService.GetAllRutasAsync();
            ViewBag.Rutas = new SelectList(rutas, "Id", "Nombre");
            return View(contenedor);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var contenedor = await _contenedorService.GetContenedorByIdAsync(id);
            if (contenedor == null)
            {
                return NotFound();
            }
            return View(contenedor);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _contenedorService.DeleteContenedorAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
