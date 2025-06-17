using Domain.Enum;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entity
{
    public class ListCard
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [StringLength(45, MinimumLength = 3)]
        public string? Title { get; set; }

        public StatusItemEnum Status { get; set; } = StatusItemEnum.Active;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public WorkSpace? Workspace { get; set; }
        public ICollection<Cards>? Cards { get; set; }
    }
}
