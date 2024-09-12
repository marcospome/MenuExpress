using MenuExpress.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MenuExpress.Controllers.Auth
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        public readonly string con;

        public UserController(IConfiguration configuration)
        {
            con = configuration.GetConnectionString("connection");
        }

        [HttpPost("ValideUserLogin")]
        public IActionResult ValideUserLogin([FromBody] LoginRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
            {
                return BadRequest("El email y la contraseña son obligatorios.");
            }

            using (SqlConnection connection = new(con))
            {
                connection.Open();
                using (SqlCommand cmd = new("UserValidate", connection))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Email", request.Email);
                    cmd.Parameters.AddWithValue("@Password", request.Password);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows) // Si hay filas en el resultado
                        {
                            return Ok("Sesión iniciada con éxito"); // Mensaje de éxito
                        }
                        else
                        {
                            return NotFound("No se logró iniciar sesión"); // Mensaje de error
                        }
                    }
                }
            }
        }



        [HttpPost("CreateUser")]
        public void CreateUser([FromBody] User u)
        {
            using (SqlConnection connection = new(con))
            {
                connection.Open();
                using (SqlCommand cmd = new("AddUser", connection))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Name", u.Name);
                    cmd.Parameters.AddWithValue("@Active", u.Active);
                    cmd.Parameters.AddWithValue("@LastName", u.LastName);
                    cmd.Parameters.AddWithValue("@Password", u.Password);
                    cmd.Parameters.AddWithValue("@Email", u.Email);
                    cmd.ExecuteNonQuery();
                }
                connection.Close();
            }
        }


    }
}
