using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using MySql.Data.MySqlClient;
using proyectojurado.Data;
using System;

namespace proyectojurado.Pages
{
    public class LoginModel : PageModel
    {
        private readonly Database _db = new Database();

        [BindProperty]
        public string Email { get; set; } = string.Empty;

        [BindProperty]
        public string Password { get; set; } = string.Empty;

        public string Message { get; set; } = string.Empty;

        public void OnGet() { }

        public IActionResult OnPost()
        {
            // Validar campos vacíos
            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
            {
                Message = "❌ Debes ingresar email y contraseña";
                return Page();
            }

            try
            {
                using (var conn = _db.GetConnection())
                {
                    conn.Open();

                    string query = "SELECT nombre FROM usuarios WHERE email=@Email AND contrasena=@Password LIMIT 1";
                    var cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Email", Email.Trim());
                    cmd.Parameters.AddWithValue("@Password", Password.Trim());

                    var reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        string nombre = reader.GetString("nombre");

                        // Guardar en sesión
                        HttpContext.Session.SetString("UsuarioNombre", nombre);

                        return RedirectToPage("/Index");
                    }
                    else
                    {
                        Message = "❌ Usuario o contraseña incorrectos";
                        return Page();
                    }
                }
            }
            catch (MySqlException ex)
            {
                Message = $"❌ Error de base de datos: {ex.Message}";
                return Page();
            }
            catch (Exception ex)
            {
                Message = $"❌ Error inesperado: {ex.Message}";
                return Page();
            }
        }
    }
}
