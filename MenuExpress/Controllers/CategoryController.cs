﻿using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using MenuExpress.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MenuExpress.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        public readonly string con;

        public CategoryController(IConfiguration configuration)
        {
            con = configuration.GetConnectionString("connection");
        }


        // Obtiene todas las categorías cargadas en la base.
        [HttpGet("getAllCategories")]
        public IEnumerable<Category> GetAllActions()
        {
            List<Category> categories = new();
            using (SqlConnection connection = new(con)) // Utilizo la conexión declarada en el appsettings.json
            {
                connection.Open(); // Inicio la conexión
                using (SqlCommand cmd = new("GetCategory", connection)) // GetCategory, el PRC que hace un select * from a la tabla Category por base.
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read()) // While para asegurarnos que se ejecute unicamente si levanta datos.
                        {
                            Category A = new Category // Creo el JSON con las variables que tiene que tomar. (DEBE TENER EL MISMO NOMBRE LA VARIABLE QUE POR BASE, RESPETANDO MAYUSCULAS, ETC.)
                            {
                                IdCategory = Convert.ToInt32(reader["IdCategory"]),
                                Name = reader["Name"].ToString(),
                                Deleted = Convert.ToInt32(reader["Deleted"])
                            };
                            categories.Add(A);
                        }
                    }
                }
                connection.Close(); // Finalizo la conexión
            }
            return categories;
        }

        [HttpPost("CreateCategory")]
        public void CreateCategory([FromBody] Category c)
        {
            using (SqlConnection connection = new(con))
            {
                connection.Open();
                using (SqlCommand cmd = new("CreateCategory", connection))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Name", c.Name);
                    cmd.Parameters.AddWithValue("@Deleted", c.Deleted);
                    cmd.ExecuteNonQuery();
                }
                connection.Close();
            }
        }


        [HttpPut("categoryEdit/{id}")]
        public void EditCategory([FromBody] Category c, int id)  // si pongo List<Action> puedo enviar varios elementos en un solo json.
        {
            using (SqlConnection connection = new(con))
            {
                connection.Open();
                using (SqlCommand cmd = new("UpdateCategory", connection))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IdCategory", id);
                    cmd.Parameters.AddWithValue("@Name", c.Name);
                    cmd.Parameters.AddWithValue("@Deleted", c.Deleted);
                    cmd.ExecuteNonQuery();
                }
                connection.Close();
            }
        }

        [HttpDelete("categoryDelete/{id}")]
        public void DeleteProduct(int id)  // si pongo List<Action> puedo enviar varios elementos en un solo json.
        {
            using (SqlConnection connection = new(con))
            {
                connection.Open();
                using (SqlCommand cmd = new("DeleteCategory", connection))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IdCategory", id);
                    cmd.ExecuteNonQuery();
                }
                connection.Close();
            }
        }
    }
}
