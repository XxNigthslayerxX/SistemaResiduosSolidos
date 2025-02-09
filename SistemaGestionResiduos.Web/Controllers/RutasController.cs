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
            Console.WriteLine($"\nIntentando crear ruta: {ruta.Nombre}");
            Console.WriteLine($"Datos de la ruta:");
            Console.WriteLine($"- Nombre: {ruta.Nombre}");
            Console.WriteLine($"- Descripción: {ruta.Descripcion}");
            Console.WriteLine($"- Hora Inicio: {ruta.HoraInicio}");
            Console.WriteLine($"- Hora Fin: {ruta.HoraFin}");
            Console.WriteLine($"- Días Servicio: {ruta.DiasServicio}");
            Console.WriteLine($"- Activo: {ruta.Activo}");

            if (!ModelState.IsValid)
            {
                var errors = string.Join("; ", ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage));
                Console.WriteLine($"ModelState errors: {errors}");
                return View(ruta);
            }

            try
            {
                Console.WriteLine("Llamando a CreateRutaAsync...");
                var rutaCreada = await _rutaService.CreateRutaAsync(ruta);
                Console.WriteLine($"Ruta creada exitosamente con ID: {rutaCreada.Id}");

                // Verificar que la ruta se guardó correctamente
                var rutaGuardada = await _rutaService.GetRutaByIdAsync(rutaCreada.Id);
                if (rutaGuardada != null)
                {
                    Console.WriteLine($"Verificación: Ruta encontrada en la base de datos con ID {rutaGuardada.Id}");
                }
                else
                {
                    Console.WriteLine("¡ADVERTENCIA! No se pudo encontrar la ruta recién creada en la base de datos");
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al crear ruta: {ex.Message}");
                Console.WriteLine($"StackTrace: {ex.StackTrace}");
                ModelState.AddModelError("", "Error al guardar la ruta.");
                return View(ruta);
            }
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
                Console.WriteLine($"ID no coincide: {id} vs {ruta.Id}");
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                var errors = string.Join("; ", ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage));
                Console.WriteLine($"ModelState errors: {errors}");
                return View(ruta);
            }

            try
            {
                Console.WriteLine($"Intentando actualizar ruta: {ruta.Nombre}");
                await _rutaService.UpdateRutaAsync(ruta);
                Console.WriteLine("Ruta actualizada exitosamente");
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al actualizar ruta: {ex.Message}");
                ModelState.AddModelError("", "Error al actualizar la ruta.");
                return View(ruta);
            }
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
