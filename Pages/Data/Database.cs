using MySql.Data.MySqlClient;
using System;

namespace proyectojurado.Data
{
    public class Database
    {
        private readonly string connectionString = "server=localhost;database=control_gastos;user=root;password=mysql;";

        public MySqlConnection GetConnection()
        {
            try
            {
                var connection = new MySqlConnection(connectionString);
                return connection;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Error al conectar a la base de datos: " + ex.Message);
                throw;
            }
        }
    }
}






