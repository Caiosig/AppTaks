using Domain.Enum;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entity
{
    public class WorkSpace
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [StringLength(45, MinimumLength = 3)]
        public string? Title { get; set; }

        [Required]
        public User? User { get; set; }
        public ICollection<ListCard> ListsCards { get; set; }
        public StatusItemEnum Status { get; set; } = StatusItemEnum.Active;
        public DateTime CreateAt { get; set; } = DateTime.Now;
    }
}
