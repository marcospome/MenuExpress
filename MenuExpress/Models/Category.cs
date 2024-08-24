using System.ComponentModel.DataAnnotations;

namespace MenuExpress.Models
{
    public class Category
    {
        [Key]
        public int IdCategory { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public int Deleted { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}
