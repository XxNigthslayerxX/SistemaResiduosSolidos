using Microsoft.AspNetCore.Mvc;

namespace SistemaGestionResiduos.Web.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            if (!string.IsNullOrWhiteSpace(username) && !string.IsNullOrWhiteSpace(password))
            {
                return RedirectToAction("Index", "Inicio");
            }

            ViewBag.Error = "Usuario o contraseña inválido.";
            return View();
        }
    }
}