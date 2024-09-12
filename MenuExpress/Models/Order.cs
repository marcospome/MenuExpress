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
        [JsonIgnore]
        public ICollection<OrderDetail>? OrderDetails { get; set; }
    }

    public class Order
    {
        [Key]
        public int IdOrder { get; set; }

        [Required]
        public DateTime AddDate { get; set; }

        public int? IdUser { get; set; }

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

        [Required]
        [StringLength(100)]
        public string ClientEmail { get; set; }

        [ForeignKey("IdStatus")]
        public OrderStatus Status { get; set; }

        [ForeignKey("IdUser")]
        public User User { get; set; }

        [JsonIgnore]
        public ICollection<OrderDetail>? OrderDetails { get; set; }
    }

    public class OrderDetail
    {
        [Key]
        public int IdOrderDetail { get; set; }

        [Required]
        public int Qty { get; set; }

        public int? IdStatus { get; set; }

        [Required]
        public DateTime AddDate { get; set; }

        [StringLength(255)]
        public string Note { get; set; }

        public int? IdOrder { get; set; }

        public int? IdProduct { get; set; }

        [ForeignKey("IdOrder")]
        public Order Order { get; set; }

        [ForeignKey("IdProduct")]
        public Product Product { get; set; }

        [ForeignKey("IdStatus")]
        public OrderStatus Status { get; set; }
    }
}
