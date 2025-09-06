using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;

namespace proyectojurado.Pages
{
    public class AuthenticatedPage : PageModel
    {
        // Método que revisa si hay sesión activa
        public IActionResult CheckLogin()
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                // Si no hay sesión, redirige a /Login
                return RedirectToPage("/Login");
            }

            // Si hay sesión, devolvemos Page() para continuar normalmente
            return Page();
        }
    }
}
