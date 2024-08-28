using MenuExpress.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MenuExpress.Controllers
{
    [Route("api/product/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        public readonly string con;

        public ProductController(IConfiguration configuration)
        {
            con = configuration.GetConnectionString("connection");
        }


        // Obtiene todas las categorías cargadas en la base.
        [HttpGet("getAllProducts")]
        public IEnumerable<Product> GetAllActions()
        {
            List<Product> products = new();
            using (SqlConnection connection = new(con)) // Utilizo la conexión declarada en el appsettings.json
            {
                connection.Open(); // Inicio la conexión
                using (SqlCommand cmd = new("GetProduct", connection)) // GetCategory, el PRC que hace un select * from a la tabla Category por base.
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read()) // While para asegurarnos que se ejecute unicamente si levanta datos.
                        {
                            Product A = new Product // Creo el JSON con las variables que tiene que tomar. (DEBE TENER EL MISMO NOMBRE LA VARIABLE QUE POR BASE, RESPETANDO MAYUSCULAS, ETC.)
                            {
                                IdProduct = Convert.ToInt32(reader["IdProduct"]),
                                Name = reader["Name"].ToString(),
                                Deleted = Convert.ToInt32(reader["Deleted"]),
                                Description = reader["Description"].ToString(),
                                Price = Convert.ToDecimal(reader["Price"]),
                                AddDate = Convert.ToDateTime(reader["AddDate"]),
                                Image = reader["Image"].ToString(),
                                IdCategory = Convert.ToInt32(reader["IdCategory"])
                            };
                            products.Add(A);
                        }
                    }
                }
                connection.Close(); // Finalizo la conexión
            }
            return products;
        }
    }
}
