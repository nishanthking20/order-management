using System.ComponentModel.DataAnnotations;
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
        [StringLength(100, MinimumLength = 8)]
        public required string Password { get; set; }

        
        public int VerificationCode { get; set; }
        
        public bool IsEmailVerified { get; set; }

        // [Required]
        // [DataType(DataType.Password)]
        // [StringLength(100, MinimumLength = 6)]
        // public required string ConfirmPassword { get; set; }

        // public override string ToString()
        // {
        //     return string.Format("User: {0}, {1}, {2}", Name, Email, Password);  
        // }
    }
}