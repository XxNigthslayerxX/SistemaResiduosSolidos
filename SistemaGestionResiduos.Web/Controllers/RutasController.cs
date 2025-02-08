using Microsoft.AspNetCore.Mvc;
using SistemaGestionResiduos.Application.Services;
using SistemaGestionResiduos.Domain.Entities;
using System.Threading.Tasks;

namespace SistemaGestionResiduos.Web.Controllers
{
    public class RutasController : Controller
    {
        private readonly RutaService _rutaService;

        public RutasController(RutaService rutaService)
        {
            _rutaService = rutaService;
        }

        public async Task<IActionResult> Index()
        {
            var rutas = await _rutaService.GetAllRutasAsync();
            return View(rutas);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Ruta ruta)
        {
            if (ModelState.IsValid)
            {
                await _rutaService.CreateRutaAsync(ruta);
                return RedirectToAction(nameof(Index));
            }
            return View(ruta);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var ruta = await _rutaService.GetRutaByIdAsync(id);
            if (ruta == null)
            {
                return NotFound();
            }
            return View(ruta);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Ruta ruta)
        {
            if (id != ruta.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _rutaService.UpdateRutaAsync(ruta);
                return RedirectToAction(nameof(Index));
            }
            return View(ruta);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var ruta = await _rutaService.GetRutaByIdAsync(id);
            if (ruta == null)
            {
                return NotFound();
            }
            return View(ruta);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _rutaService.DeleteRutaAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
