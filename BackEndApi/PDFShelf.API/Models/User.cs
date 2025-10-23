using System.ComponentModel.DataAnnotations;

namespace PDFShelf.Api.Models
{
    public class User
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required, MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required, MaxLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        //Role: "User", "Admin", etc.
        [Required, MaxLength(50)]
        public string Role { get; set; } = "User";
        
        // Storage usage tracking (em MB)
        public double StorageUsedMB { get; set; } = 0.0;

        // Foreign key to Plan 
        public int PlanId { get; set; } = 1; // default to Free plan id seed = 1
        public Plan? Plan { get; set; }

        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? LastLogin { get; set; }
    }
}
