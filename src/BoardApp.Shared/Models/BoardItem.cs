using BoardApp.Shared.Models.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace BoardApp.Shared.Models
{
    public class BoardItem
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [MinLength(3, ErrorMessage = "Min lenght is 3.")]
        public string Summary { get; set; }
        public string Description { get; set; }
        public int Points { get; set; }
        public Status Status { get; set; } = Status.TODO;
        public DateTime CreateAt { get; set; } = DateTime.Now;
        public DateTime? DeleteAt { get; set; }
        public string Author { get; set; }
        public string File { get; set; }
    }
}
