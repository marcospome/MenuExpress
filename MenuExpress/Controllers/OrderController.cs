using MenuExpress.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
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

            // ------------------------------------------ Método para traer todas las ordenes ------------------------------------------ 

        }
        [HttpGet("getAllOrders")]
        public IEnumerable<Order> GetAllActions()
        {
            List<Order> orders = new();
            using (SqlConnection connection = new(con)) // Utilizo la conexión declarada en el appsettings.json
            {
                connection.Open(); // Inicio la conexión
                using (SqlCommand cmd = new("GetOrder", connection)) // GetOrder, el PRC que hace un select * from a la tabla Order por base.
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read()) // While para asegurarnos que se ejecute unicamente si levanta datos.
                        {
                            Order A = new Order
                            {
                                IdOrder = reader["IdOrder"] != DBNull.Value ? Convert.ToInt32(reader["IdOrder"]) : 0,
                                AddDate = reader["AddDate"] != DBNull.Value ? Convert.ToDateTime(reader["AddDate"]) : DateTime.MinValue,
                                IdUser = reader["IdUser"] != DBNull.Value ? Convert.ToInt32(reader["IdUser"]) : 0,
                                IdStatus = reader["IdStatus"] != DBNull.Value ? Convert.ToInt32(reader["IdStatus"]) : 0,
                                ClientName = reader["ClientName"] != DBNull.Value ? reader["ClientName"].ToString() : string.Empty,
                                ClientLastName = reader["ClientLastName"] != DBNull.Value ? reader["ClientLastName"].ToString() : string.Empty,
                                ClientDNI = reader["ClientDNI"] != DBNull.Value ? reader["ClientDNI"].ToString() : string.Empty,
                                ClientEmail = reader["ClientEmail"] != DBNull.Value ? reader["ClientEmail"].ToString() : string.Empty
                            };
                            orders.Add(A);

                        }
                    }
                }
                connection.Close(); // Finalizo la conexión
            }
            return orders;
        }

        // ------------------------------------------ Método para traer todo el detalle de las ordenes por idorder ------------------------------------------ 

        [HttpGet("getOrderDetails/{idOrder}")]
        public IActionResult GetOrderDetails(int idOrder)
        {
            try
            {
                List<OrderDetail> orderDetails = new(); // Lista para almacenar los detalles de la orden
                using (SqlConnection connection = new(con)) // Se obtiene la conexión desde la cadena de conexión definida en el appsettings.json
                {
                    connection.Open(); // Inicia la conexión con la base de datos
                    using (SqlCommand cmd = new SqlCommand("GetOrderDetail", connection)) // Se crea el comando para ejecutar el procedimiento almacenado GetOrderDetail
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure; // Define que es un procedimiento almacenado
                        cmd.Parameters.Add(new SqlParameter("@IdOrder", idOrder)); // Se agrega el parámetro de entrada IdOrder

                        using (SqlDataReader reader = cmd.ExecuteReader()) // Ejecuta el comando y obtiene los resultados mediante un SqlDataReader
                        {
                            while (reader.Read()) // Procesa cada fila de la consulta
                            {
                                OrderDetail detail = new OrderDetail // Mapea los valores del SqlDataReader a la entidad OrderDetail
                                {
                                    IdOrderDetail = reader["IdOrderDetail"] != DBNull.Value ? Convert.ToInt32(reader["IdOrderDetail"]) : 0, // Asigna IdOrderDetail
                                    Qty = reader["Qty"] != DBNull.Value ? Convert.ToInt32(reader["Qty"]) : 0, // Asigna Qty (cantidad)
                                    AddDate = reader["AddDate"] != DBNull.Value ? Convert.ToDateTime(reader["AddDate"]) : (DateTime?)null, // Asigna AddDate o null
                                    Note = reader["Note"] != DBNull.Value ? reader["Note"].ToString() : string.Empty, // Asigna la Nota si no es null
                                    IdProduct = reader["IdProduct"] != DBNull.Value ? Convert.ToInt32(reader["IdProduct"]) : (int?)null // Asigna IdProduct o null
                                };
                                orderDetails.Add(detail); // Añade el detalle a la lista
                            }
                        }
                    }

                    connection.Close(); // Cierra la conexión con la base de datos
                }

                return Ok(orderDetails); // Devuelve la lista de detalles de la orden con estado 200 OK
            }
            catch (SqlException ex) // Evita que el programa se detenga por un error SQL.
            {
                return StatusCode(500, "Ha ocurrido un inconveniente inesperado con la base de datos. Intente nuevamente más tarde.");
            }
            catch (Exception ex) // Evita que el programa se detenga por un error del servidor.
            {
                return StatusCode(500, "Ha ocurrido un inconveniente inesperado. Intente nuevamente más tarde.");
            }
        }

        // ------------------------------------------ Método para traer las ordenes con detalle por iduser ------------------------------------------ 


        [HttpGet("getAllOrdersByUser/{idUser}")]
        public IActionResult GetAllOrdersByUser(int idUser)
        {
            try
            {
                List<Order> orders = new List<Order>();

                using (SqlConnection connection = new SqlConnection(con))
                {
                    connection.Open(); // Inicia la conexión

                    // Llama al PRC que trae las órdenes por IdUser
                    using (SqlCommand cmd = new SqlCommand("GetOrderByUser", connection))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@IdUser", idUser)); // Pasa el parámetro IdUser

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            // Procesa los resultados
                            while (reader.Read())
                            {
                                Order order = new Order
                                {
                                    IdOrder = reader["IdOrder"] != DBNull.Value ? Convert.ToInt32(reader["IdOrder"]) : 0,
                                    AddDate = reader["AddDate"] != DBNull.Value ? Convert.ToDateTime(reader["AddDate"]) : DateTime.MinValue,
                                    ClientName = reader["ClientName"] != DBNull.Value ? reader["ClientName"].ToString() : string.Empty,
                                    ClientLastName = reader["ClientLastName"] != DBNull.Value ? reader["ClientLastName"].ToString() : string.Empty,
                                    ClientDNI = reader["ClientDNI"] != DBNull.Value ? reader["ClientDNI"].ToString() : string.Empty,
                                    ClientEmail = reader["ClientEmail"] != DBNull.Value ? reader["ClientEmail"].ToString() : string.Empty,
                                    OrderDetails = new List<OrderDetail>() // Inicializa la lista de detalles
                                };

                                // Añade la orden a la lista
                                orders.Add(order);
                            }
                        }
                    }

                    // Recorre las órdenes para obtener sus detalles después de cerrar el primer reader
                    foreach (var order in orders)
                    {
                        using (SqlCommand detailCmd = new SqlCommand("GetOrderDetail", connection))
                        {
                            detailCmd.CommandType = System.Data.CommandType.StoredProcedure;
                            detailCmd.Parameters.Add(new SqlParameter("@IdOrder", order.IdOrder)); // Pasa el IdOrder para obtener sus detalles

                            using (SqlDataReader detailReader = detailCmd.ExecuteReader())
                            {
                                while (detailReader.Read())
                                {
                                    OrderDetail detail = new OrderDetail
                                    {
                                        IdOrderDetail = detailReader["IdOrderDetail"] != DBNull.Value ? Convert.ToInt32(detailReader["IdOrderDetail"]) : 0,
                                        Qty = detailReader["Qty"] != DBNull.Value ? Convert.ToInt32(detailReader["Qty"]) : 0,
                                        AddDate = detailReader["AddDate"] != DBNull.Value ? Convert.ToDateTime(detailReader["AddDate"]) : DateTime.MinValue,
                                        IdProduct = detailReader["IdProduct"] != DBNull.Value ? Convert.ToInt32(detailReader["IdProduct"]) : 0,
                                        Note = detailReader["Note"] != DBNull.Value ? detailReader["Note"].ToString() : string.Empty
                                    };

                                    // Añadir el detalle a la lista de detalles de la orden
                                    order.OrderDetails.Add(detail);
                                }
                            }
                        }
                    }
                }

                // Devuelve la lista de órdenes con sus respectivos detalles
                return Ok(orders);
            }
            catch (SqlException ex) // Evita que el programa se detenga por un error SQL.
            {
                return StatusCode(500, "Ha ocurrido un inconveniente inesperado con la base de datos. Intente nuevamente más tarde.");
            }
            catch (Exception ex) // Evita que el programa se detenga por un error del servidor.
            {
                return StatusCode(500, "Ha ocurrido un inconveniente inesperado. Intente nuevamente más tarde.");
            }
        }




    }
}