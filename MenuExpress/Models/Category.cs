using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MenuExpress.Models
{
    public class Category
    {
        public int IdCategory { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public int Deleted { get; set; }

        [JsonIgnore]
        public ICollection<Product>? Products { get; set; }
    }
}
