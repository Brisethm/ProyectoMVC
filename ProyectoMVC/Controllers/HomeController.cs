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
        [HttpGet]
        public IActionResult Delete()
        {
            return View();
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Update()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Update(int CategoryID, string CategoryName, string Description)
        {
            string conexion = @"Server=localhost\SQLEXPRESS01;Database=northwind;Trusted_Connection=True;TrustServerCertificate=True;";
            string query = $"EXEC sp_u_categories @p_categoryID={CategoryID}, @p_categoryName='{CategoryName}', @Description='{Description}'";

            try
            {
                using (SqlConnection conexionmia = new SqlConnection(conexion))
                {
                    conexionmia.Open();
                    using (SqlCommand comando = new SqlCommand(query, conexionmia))
                    {
                        comando.ExecuteNonQuery();
                    }
                }
                ViewBag.Mensaje = "Categoría actualizada correctamente";
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = "Error al actualizar: " + ex.Message;
            }

            return View();
        }

        [HttpPost]
        public IActionResult Delete(int CategoryID)
        {
            string conexion = @"Server=localhost\SQLEXPRESS01;Database=northwind;Trusted_Connection=True;TrustServerCertificate=True;";
            string query = $"EXEC sp_d_categories @p_categoryID={CategoryID}";

            try
            {
                using (SqlConnection conexionmia = new SqlConnection(conexion))
                {
                    conexionmia.Open();
                    using (SqlCommand comando = new SqlCommand(query, conexionmia))
                    {
                        int filasAfectadas = comando.ExecuteNonQuery();

                        if (filasAfectadas > 0)
                            ViewBag.Mensaje = $"Categoría con ID {CategoryID} eliminada correctamente.";
                        else
                            ViewBag.Mensaje = $"No existe ninguna categoría con ID {CategoryID}.";
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = "Error al eliminar: " + ex.Message;
            }
            return View();
        }


        [HttpGet]
        public IActionResult Vw(int? CategoryID)
        {
            if (!CategoryID.HasValue)
                return View(null);

            string conexion = @"Server=localhost\SQLEXPRESS01;Database=northwind;Trusted_Connection=True;TrustServerCertificate=True;";
            string query = "SELECT CategoryID, CategoryName FROM vw_R_categories WHERE CategoryID = @CategoryID";

            List<Category> lista = new List<Category>();

            try
            {
                using (SqlConnection conexionmia = new SqlConnection(conexion))
                {
                    conexionmia.Open();
                    using (SqlCommand comando = new SqlCommand(query, conexionmia))
                    {
                        comando.Parameters.AddWithValue("@CategoryID", CategoryID.Value);
                        using (SqlDataReader reader = comando.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                lista.Add(new Category
                                {
                                    CategoryID = Convert.ToInt32(reader["CategoryID"]),
                                    CategoryName = reader["CategoryName"].ToString(),
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = "Error al consultar: " + ex.Message;
            }

            return View(lista);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
