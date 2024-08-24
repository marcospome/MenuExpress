using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MenuExpress.Models
{
    public class Product
    {
        [Key]
        public int IdProduct { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public int Deleted { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        [Column(TypeName = "decimal(10, 2)")]
        public decimal Price { get; set; }

        public DateTime AddDate { get; set; }

        [StringLength(500)]
        public string Image { get; set; }

        public int IdCategory { get; set; }

        [ForeignKey("IdCategory")]
        public Category Category { get; set; }
    }
}
