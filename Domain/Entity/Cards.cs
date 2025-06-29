﻿using Domain.Enum;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entity
{
    public class Cards
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [StringLength(45, MinimumLength = 3)]
        public string? Title { get; set; }

        [StringLength(120)]
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? DeadLine { get; set; }
        public ListCard? List { get; set; }
        public StatusCardEnum Status { get; set; }
    }
}
