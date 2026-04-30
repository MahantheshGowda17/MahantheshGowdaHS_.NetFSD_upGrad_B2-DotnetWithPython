using System.ComponentModel.DataAnnotations;

namespace EMS.API.Models
{
    public class AppUser
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Username { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        [Required]
        [MaxLength(20)]
        public string Role { get; set; } // Admin / Viewer

        public DateTime CreatedAt { get; set; }
    }
}
