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


        // ------------------------------------------ Método para crear una orden ------------------------------------------ 

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
                    cmd.Parameters.AddWithValue("@ClientEmail", (object)order.ClientEmail ?? DBNull.Value); // Permite nulos.

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
                        detailCmd.Parameters.AddWithValue("@Note", (object)detail.Note ?? DBNull.Value); // Permite nulos.
                        detailCmd.Parameters.AddWithValue("@IdProduct", detail.IdProduct);

                        detailCmd.ExecuteNonQuery();
                    }
                }

                connection.Close();
            }
            return Ok(); // Devuelve el resultado adecuado
        }


        // ------------------------------------------ Método para editar algún item del a orden ------------------------------------------ 

        [HttpPut("orderDetailEdit/{id}")]
        public void EditOrderDetail([FromBody] OrderDetail od, int id)
        {
            using (SqlConnection connection = new SqlConnection(con))
            {
                connection.Open();

                // Usar SqlCommand para llamar al procedimiento almacenado
                using (SqlCommand cmd = new SqlCommand("UpdateOrderDetail", connection))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    // Agregar los parámetros del procedimiento almacenado
                    cmd.Parameters.AddWithValue("@IdOrder", id);
                    cmd.Parameters.AddWithValue("@IdOderDetail", od.IdOrderDetail);  // Usar el parámetro de ruta como IdOrderDetail
                    cmd.Parameters.AddWithValue("@Note", od.Note);
                    cmd.Parameters.AddWithValue("@Qty", od.Qty);

                    // Ejecutar el procedimiento
                    cmd.ExecuteNonQuery();
                }

                connection.Close();
            }
        }

        // ------------------------------------------ Método para eliminar "LOGICAMENTE" algún item del a orden ------------------------------------------ 

        [HttpPut("orderDetailDelete/{id}")]
        public void DeleteOrderDetail([FromBody] OrderDetail od, int id)
        {
            using (SqlConnection connection = new SqlConnection(con))
            {
                connection.Open();

                // Usar SqlCommand para llamar al procedimiento almacenado
                using (SqlCommand cmd = new SqlCommand("DeleteOrderDetail", connection))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    // Agregar los parámetros del procedimiento almacenado
                    cmd.Parameters.AddWithValue("@IdOrder", id);
                    cmd.Parameters.AddWithValue("@IdOderDetail", od.IdOrderDetail);  // Usar el parámetro de ruta como IdOrderDetail

                    // Ejecutar el procedimiento
                    cmd.ExecuteNonQuery();
                }

                connection.Close();
            }
        }


        // ------------------------------------------ Método para agreagar un nuevo item a la orden ------------------------------------------ 

        [HttpPut("orderDetailAddItem/{id}")]
        public IActionResult orderDetailAddItem([FromBody] OrderDetail od, int id)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(con))
                {
                    connection.Open();

                    // Usar SqlCommand para llamar al procedimiento almacenado
                    using (SqlCommand cmd = new SqlCommand("AddItemOrderDetail", connection))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        // Agregar los parámetros del procedimiento almacenado
                        cmd.Parameters.AddWithValue("@IdOrder", id);
                        cmd.Parameters.AddWithValue("@Qty", od.Qty);
                        cmd.Parameters.AddWithValue("@Note", od.Note ?? (object)DBNull.Value);  // Manejo de NULL ; 
                        cmd.Parameters.AddWithValue("@IdProduct", od.IdProduct);

                        // Ejecutar el procedimiento
                        cmd.ExecuteNonQuery();
                    }

                    connection.Close();
                }

                return Ok("Se agrego el nuevo item a la orden correctamente.");
            }
            catch (SqlException ex) // Evita qué el programa se detenga por un error SQL.
            {
                return StatusCode(500, "Ha ocurrido un inconveniente inesperado. Intente nuevamente más tarde.");
            }
            catch (Exception ex) // Evita qué el programa se detenga por un error del servidor.
            {
                return StatusCode(500, "Ha ocurrido un inconveniente inesperado. Intente nuevamente más tarde.");
            }
        }

    }
}
