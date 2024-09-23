using MenuExpress.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MenuExpress.Controllers.Auth
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly string _connectionString;
        private readonly IConfiguration _configuration;

        public UserController(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = configuration.GetConnectionString("connection");
        }

        [HttpPost("ValideUserLogin")]
        public IActionResult ValideUserLogin([FromBody] LoginRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
            {
                return BadRequest("El email y la contraseña son obligatorios.");
            }

            using (SqlConnection connection = new(_connectionString))
            {
                connection.Open();
                using (SqlCommand cmd = new("UserValidate", connection))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Email", request.Email);
                    cmd.Parameters.AddWithValue("@Password", request.Password);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read()) // Verifica si hay al menos una fila
                        {
                            var email = reader["Email"].ToString();
                            var roles = reader["Role"].ToString(); // Suponiendo que obtienes el rol del usuario

                            // Generar el token JWT
                            var token = GenerateJwtToken(email, roles); // Llama a la versión con roles
                            return Ok(new { Token = token });
                        }
                        else
                        {
                            return NotFound("No se logró iniciar sesión");
                        }
                    }
                }
            }
        }

        private string GenerateJwtToken(string email, string role)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Role, role) // Incluye el rol en las reclamaciones
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(60),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [HttpPost("CreateUser")]
        public IActionResult CreateUser([FromBody] User u)
        {
            if (u == null || string.IsNullOrEmpty(u.Email) || string.IsNullOrEmpty(u.Password))
            {
                return BadRequest("Los campos email y contraseña son obligatorios.");
            }

            using (SqlConnection connection = new(_connectionString))
            {
                connection.Open();
                using (SqlCommand cmd = new("AddUser", connection))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Name", u.Name);
                    cmd.Parameters.AddWithValue("@Active", u.Active);
                    cmd.Parameters.AddWithValue("@LastName", u.LastName);
                    cmd.Parameters.AddWithValue("@Password", u.Password); // Asegúrate de que la contraseña esté encriptada
                    cmd.Parameters.AddWithValue("@Email", u.Email);
                    cmd.ExecuteNonQuery();
                }
            }

            return CreatedAtAction(nameof(CreateUser), new { email = u.Email });
        }
    }
}
