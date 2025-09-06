using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace proyectojurado.Pages
{
    public class LogoutModel : PageModel
    {
        public IActionResult OnGet()
        {
            // ⚡ Limpiar la sesión
            HttpContext.Session.Clear();

            // Redirigir a Login
            return RedirectToPage("/Login");
        }
    }
}
