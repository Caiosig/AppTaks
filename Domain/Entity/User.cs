using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entity
{
    public class User
    {
        [Key]
        [Required]
        public Guid Id { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 3)]
        [Column(TypeName ="NVARCHAR(50)")]
        public string? Name { get; set; }

        [StringLength(50, MinimumLength = 3)]
        [Column(TypeName = "NVARCHAR(50)")]
        public string? SurName { get; set; }

        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        public string? PasswordHash { get; set; }
        
        [Required]
        public string? UserName { get; set; }

        public ICollection<WorkSpace>? Workspaces { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreashTokenExpirationTime { get; set; }
    }
}
