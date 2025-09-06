using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
using proyectojurado.Data;
using proyectojurado.Models;

namespace proyectojurado.Pages
{
    public class GastosModel : PageModel
    {
        private readonly Database _db = new Database();

        [BindProperty]
        public string Descripcion { get; set; } = string.Empty;

        [BindProperty]
        public decimal Monto { get; set; }

        [BindProperty]
        public DateTime Fecha { get; set; } = DateTime.Today;

        public List<Gasto> MisGastos { get; set; } = new List<Gasto>();

        public string Message { get; set; } = string.Empty;

        public IActionResult OnGet()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return RedirectToPage("/Login");

            CargarGastos(userId.Value);
            return Page();
        }

        public IActionResult OnPost()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return RedirectToPage("/Login");

            using (var conn = _db.GetConnection())
            {
                conn.Open();
                string query = "INSERT INTO gastos(usuario_id, descripcion, monto, fecha) VALUES(@UsuarioId, @Descripcion, @Monto, @Fecha)";
                var cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@UsuarioId", userId.Value);
                cmd.Parameters.AddWithValue("@Descripcion", Descripcion);
                cmd.Parameters.AddWithValue("@Monto", Monto);
                cmd.Parameters.AddWithValue("@Fecha", Fecha);

                cmd.ExecuteNonQuery();
            }

            Message = "✅ Gasto registrado correctamente";
            CargarGastos(userId.Value);
            return Page();
        }

        private void CargarGastos(int userId)
        {
            MisGastos.Clear();
            using (var conn = _db.GetConnection())
            {
                conn.Open();
                string query = "SELECT id, descripcion, monto, fecha FROM gastos WHERE usuario_id=@UsuarioId ORDER BY fecha DESC";
                var cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@UsuarioId", userId);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    MisGastos.Add(new Gasto
                    {
                        Id = reader.GetInt32("id"),
                        UsuarioId = userId,
                        Descripcion = reader.GetString("descripcion"),
                        Monto = reader.GetDecimal("monto"),
                        Fecha = reader.GetDateTime("fecha")
                    });
                }
            }
        }
    }
}

