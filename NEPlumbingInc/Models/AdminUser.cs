using System.ComponentModel.DataAnnotations;

namespace NEPlumbingInc.Models
{
    public class AdminUser
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Username { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;  // Stores hashed password

        public bool IsAuthenticated { get; set; } = false;
    }
}