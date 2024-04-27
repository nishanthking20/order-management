using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Order_Management.Models
{
    public class User
    {

        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public required string Name { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public required string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(12, MinimumLength = 6)]
        public required string Password { get; set; }

        [NotMapped]
        [Compare("Password")]
        [Required]
        [DataType(DataType.Password)]
        [StringLength(12, MinimumLength = 6)]
        public required string ConfirmPassword { get; set; }

        public override string ToString()
        {
            return string.Format("User: {0}, {1}, {2}, {3}, {4}", Id, Name, Email, Password, ConfirmPassword);  
        }
    }
}