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
            Console.WriteLine($"\nRutas disponibles para el dropdown:");
            foreach (var ruta in rutas)
            {
                Console.WriteLine($"- ID: {ruta.Id}, Nombre: {ruta.Nombre}");
            }
            
            var rutasSelectList = new SelectList(rutas, "Id", "Nombre");
            Console.WriteLine("\nSelectList creado:");
            foreach (var item in rutasSelectList)
            {
                Console.WriteLine($"- Value: {item.Value}, Text: {item.Text}, Selected: {item.Selected}");
            }
            
            ViewBag.Rutas = rutasSelectList;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Ubicacion,Capacidad,NivelLlenado,TipoResiduo,RutaId")] Contenedor contenedor)
        {
            Console.WriteLine($"\nIntentando crear contenedor en ubicación: {contenedor.Ubicacion}");
            Console.WriteLine($"Datos del contenedor:");
            Console.WriteLine($"- Ubicación: {contenedor.Ubicacion}");
            Console.WriteLine($"- Capacidad: {contenedor.Capacidad}");
            Console.WriteLine($"- Nivel de Llenado: {contenedor.NivelLlenado}");
            Console.WriteLine($"- Tipo de Residuo: {contenedor.TipoResiduo}");
            Console.WriteLine($"- RutaId: {contenedor.RutaId}");

            // Validar que se haya seleccionado una ruta
            var rutas = await _rutaService.GetAllRutasAsync();
            var rutaExiste = rutas.Any(r => r.Id == contenedor.RutaId);
            
            Console.WriteLine($"\nValidando RutaId: {contenedor.RutaId}");
            Console.WriteLine($"Ruta existe: {rutaExiste}");
            Console.WriteLine("Estado del ModelState:");
            foreach (var modelState in ModelState)
            {
                Console.WriteLine($"- Key: {modelState.Key}");
                Console.WriteLine($"  - Raw Value: {modelState.Value.RawValue}");
                Console.WriteLine($"  - Attempted Value: {modelState.Value.AttemptedValue}");
                Console.WriteLine($"  - Validation State: {modelState.Value.ValidationState}");
                foreach (var error in modelState.Value.Errors)
                {
                    Console.WriteLine($"  - Error: {error.ErrorMessage}");
                }
            }

            if (!rutaExiste)
            {
                Console.WriteLine($"Error: La ruta con ID {contenedor.RutaId} no existe");
                ModelState.AddModelError("RutaId", "Debe seleccionar una ruta válida");
            }

            if (!ModelState.IsValid)
            {
                var errors = string.Join("; ", ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage));
                Console.WriteLine($"ModelState errors: {errors}");
                
                Console.WriteLine("\nReconstruyendo SelectList para rutas:");
                foreach (var ruta in rutas)
                {
                    Console.WriteLine($"- ID: {ruta.Id}, Nombre: {ruta.Nombre}");
                }
                
                var rutasSelectList = new SelectList(rutas, "Id", "Nombre", contenedor.RutaId);
                Console.WriteLine("\nSelectList reconstruido:");
                foreach (var item in rutasSelectList)
                {
                    Console.WriteLine($"- Value: {item.Value}, Text: {item.Text}, Selected: {item.Selected}");
                }
                
                ViewBag.Rutas = rutasSelectList;
                return View(contenedor);
            }

            try
            {
                Console.WriteLine("Llamando a CreateContenedorAsync...");
                var contenedorCreado = await _contenedorService.CreateContenedorAsync(contenedor);
                Console.WriteLine($"Contenedor creado exitosamente con ID: {contenedorCreado.Id}");
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al crear contenedor: {ex.Message}");
                Console.WriteLine($"StackTrace: {ex.StackTrace}");
                ModelState.AddModelError("", "Error al guardar el contenedor.");
                ViewBag.Rutas = new SelectList(rutas, "Id", "Nombre", contenedor.RutaId);
                return View(contenedor);
            }
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
