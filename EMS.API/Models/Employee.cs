using System.ComponentModel.DataAnnotations;

namespace EMS.API.Models
{
    public class Employee
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(100)]
        public string LastName { get; set; }

        [Required]
        [MaxLength(200)]
        public string Email { get; set; }

        [Required]
        [MaxLength(10)]
        public string Phone { get; set; }

        [Required]
        [MaxLength(50)]
        public string Department { get; set; }

        [Required]
        [MaxLength(100)]
        public string Designation { get; set; }

        [Required]
        public decimal Salary { get; set; }

        [Required]
        public DateTime JoinDate { get; set; }

        [Required]
        [MaxLength(10)]
        public string Status { get; set; } // Active / Inactive

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}
