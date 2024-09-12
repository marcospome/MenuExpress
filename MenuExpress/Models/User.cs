using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MenuExpress.Models
{
    public class Rol
    {
        [Key]
        [JsonIgnore]
        public int IdRol { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [JsonIgnore]
        public virtual ICollection<UserRole> UserRoles { get; set; }
        [JsonIgnore]
        public virtual ICollection<RolePermission> RolePermissions { get; set; }
    }

    public class RolePermission
    {
        [Key]
        [JsonIgnore]
        public int IdRolePermission { get; set; }

        [Required]
        public int IdRol { get; set; }

        [Required]
        public int IdPermission { get; set; }

        [ForeignKey("IdRol")]
        public virtual Rol Rol { get; set; }

        [ForeignKey("IdPermission")]
        public virtual Permission Permission { get; set; }
    }

    public class User
    {
        [Key]
        [JsonIgnore]
        public int IdUser { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(100)]
        public string LastName { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        [StringLength(100)]
        public string Email { get; set; }

        [Required]
        public int Active { get; set; }

        [Required]
        public DateTime AddDate { get; set; }

    }

    /* CLASE PARA VALIDAR INICIO DE SESIÓN */

    public class LoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class UserRole
    {
        [Key]
        [JsonIgnore]
        public int IdUserRole { get; set; }

        [Required]
        public int IdUser { get; set; }

        [Required]
        public int IdRol { get; set; }

        [ForeignKey("IdUser")]
        public virtual User User { get; set; }

        [ForeignKey("IdRol")]
        public virtual Rol Rol { get; set; }
    }

    public class Permission
    {
        [Key]
        [JsonIgnore]
        public int IdPermission { get; set; }
        public string Name { get; set; }    

        // Define other properties for the Permission model
    }
}
