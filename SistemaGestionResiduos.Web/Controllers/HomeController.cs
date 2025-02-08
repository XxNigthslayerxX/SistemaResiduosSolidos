using Microsoft.AspNetCore.Mvc;
using SistemaGestionResiduos.Application.Services;
using SistemaGestionResiduos.Web.Models;
using System.Diagnostics;

namespace SistemaGestionResiduos.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly RutaService _rutaService;
        private readonly ContenedorService _contenedorService;
        private readonly RecoleccionService _recoleccionService;

        public HomeController(RutaService rutaService, ContenedorService contenedorService, RecoleccionService recoleccionService)
        {
            _rutaService = rutaService;
            _contenedorService = contenedorService;
            _recoleccionService = recoleccionService;
        }

        public async Task<IActionResult> Index()
        {
            var rutas = await _rutaService.GetAllRutasAsync();
            var contenedores = await _contenedorService.GetAllContenedoresAsync();
            var recolecciones = await _recoleccionService.GetAllRecoleccionesAsync();

            ViewBag.TotalRutas = rutas.Count();
            ViewBag.TotalContenedores = contenedores.Count();
            ViewBag.TotalRecolecciones = recolecciones.Count();
            ViewBag.PesoTotalRecolectado = recolecciones.Sum(r => r.PesoRecolectado);

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
