using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ProyectoMVC.Models;
using Microsoft.Data.SqlClient;


namespace ProyectoMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Update()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Update(int CategoryID, string CategoryName, string Description)
        {
            string conexion = "Server=localhost\\SQLEXPRESS;Database=Northwind;Trusted_Connection=True;TrustServerCertificate=True;";
            string query = $"EXEC sp_u_categories @p_categoryID={CategoryID}, @p_categoryName='{CategoryName}', @Description='{Description}'";

            try
            {
                using (SqlConnection conexionmia = new SqlConnection(conexion))
                {
                    conexionmia.Open();
                    Console.WriteLine("Se conectó para actualizar");

                    using (SqlCommand comando = new SqlCommand(query, conexionmia))
                    {
                        comando.ExecuteNonQuery();
                        Console.WriteLine("Se actualizó correctamente");
                    }
                }

                ViewBag.Mensaje = "Categoría actualizada correctamente";
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = "Error al actualizar: " + ex.Message;
            }

            return View(); // Vuelve a la misma vista Update
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
