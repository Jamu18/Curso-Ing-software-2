using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
using proyectojurado.Data;

namespace proyectojurado.Pages
{
    public class RegisterModel : PageModel
    {
        private readonly Database _db = new Database();

        [BindProperty]
        public string Nombre { get; set; } = string.Empty;

        [BindProperty]
        public string Email { get; set; } = string.Empty;

        [BindProperty]
        public string Password { get; set; } = string.Empty;

        [BindProperty]
        public string Moneda { get; set; } = "USD";

        public string Message { get; set; } = string.Empty;

        public void OnGet() { }

        public IActionResult OnPost()
        {
            using (var conn = _db.GetConnection())
            {
                conn.Open();

                // Verificar si el email ya existe
                string checkQuery = "SELECT COUNT(*) FROM usuarios WHERE email=@Email";
                MySqlCommand checkCmd = new MySqlCommand(checkQuery, conn);
                checkCmd.Parameters.AddWithValue("@Email", Email);

                long count = (long)checkCmd.ExecuteScalar();
                if (count > 0)
                {
                    Message = "❌ Este email ya está registrado";
                    return Page();
                }

                // Insertar nuevo usuario
                string query = "INSERT INTO usuarios(nombre, email, contrasena, moneda) VALUES(@Nombre, @Email, @Password, @Moneda)";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Nombre", Nombre);
                cmd.Parameters.AddWithValue("@Email", Email);
                cmd.Parameters.AddWithValue("@Password", Password); // luego puedes encriptar
                cmd.Parameters.AddWithValue("@Moneda", Moneda);

                cmd.ExecuteNonQuery();
                Message = "✅ Usuario registrado correctamente";
            }

            return Page();
        }
    }
}




