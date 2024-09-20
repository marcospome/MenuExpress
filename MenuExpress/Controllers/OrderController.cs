using MenuExpress.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MenuExpress.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        public readonly string con;

        public OrderController(IConfiguration configuration)
        {
            con = configuration.GetConnectionString("connection");
        }

        [HttpPost("CreateOrder")]
        public IActionResult CreateOrder([FromBody] Order order)
        {
            using (SqlConnection connection = new(con))
            {
                connection.Open();

                // Insertar la orden principal
                int idOrder;
                using (SqlCommand cmd = new("CreateOrder", connection))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ClientName", order.ClientName);
                    cmd.Parameters.AddWithValue("@ClientLastName", order.ClientLastName);
                    cmd.Parameters.AddWithValue("@ClientDNI", order.ClientDNI);
                    cmd.Parameters.AddWithValue("@ClientEmail", order.ClientEmail);

                    // Obtener el IdOrder de la orden recién insertada
                    idOrder = Convert.ToInt32(cmd.ExecuteScalar());
                }

                // Insertar los detalles de la orden
                foreach (var detail in order.OrderDetails)
                {
                    using (SqlCommand detailCmd = new("CreateOrderDetail", connection))
                    {
                        detailCmd.CommandType = System.Data.CommandType.StoredProcedure;
                        detailCmd.Parameters.AddWithValue("@IdOrder", idOrder);
                        detailCmd.Parameters.AddWithValue("@Qty", detail.Qty);
                        detailCmd.Parameters.AddWithValue("@Note", detail.Note);
                        detailCmd.Parameters.AddWithValue("@IdProduct", detail.IdProduct);

                        detailCmd.ExecuteNonQuery();
                    }
                }

                connection.Close();
            }
            return Ok(); // Devuelve el resultado adecuado
        }


    }
}
