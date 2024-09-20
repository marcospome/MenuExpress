using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MenuExpress.Models
{
    public class OrderStatus
    {
        [Key]
        public int IdStatus { get; set; }

        [Required]
        [StringLength(50)]
        public string Status { get; set; }

        [StringLength(255)]
        public string Description { get; set; }

        [JsonIgnore]
        public ICollection<Order>? Orders { get; set; }

        public ICollection<OrderDetail> OrderDetails { get; set; }
    }

    public class Order
    {
        public Order()
        {
            OrderDetails = new List<OrderDetail>();
        }

        [Key]
        public int IdOrder { get; set; }

        public DateTime? AddDate { get; set; }

        [JsonIgnore]
        public int? IdUser { get; set; }

        [JsonIgnore]
        public int? IdStatus { get; set; }

        [Required]
        [StringLength(100)]
        public string ClientName { get; set; }

        [Required]
        [StringLength(100)]
        public string ClientLastName { get; set; }

        [Required]
        [StringLength(20)]
        public string ClientDNI { get; set; }

        [StringLength(255)]
        public string? ClientEmail { get; set; }

        [JsonIgnore]
        [ForeignKey("IdStatus")]
        public OrderStatus? Status { get; set; } // Hacer que la relación sea opcional

        [JsonIgnore]
        [ForeignKey("IdUser")]
        public User? User { get; set; } // Hacer que la relación sea opcional

        public ICollection<OrderDetail> OrderDetails { get; set; }
    }


    public class OrderDetail
    {
        [Key]
        public int IdOrderDetail { get; set; }

        [Required]
        public int Qty { get; set; }

        public DateTime? AddDate { get; set; } // Asegúrate de manejar esto adecuadamente.

        [StringLength(255)]
        public string? Note { get; set; }

        public int? IdProduct { get; set; }

        [JsonIgnore]
        [ForeignKey("Order")]
        public Order? Order { get; set; }

        [JsonIgnore]
        [ForeignKey("IdProduct")]
        public Product? Product { get; set; }
    }
}


