using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc;
namespace Order_Management.Models
{
    public class User
    {

        public int Id { get; set; }

        [Required(ErrorMessage = "Username is required")]
        [BindProperty]
        [StringLength(100)]
        public required string Name { get; set; }

        [Required(ErrorMessage ="Email is required")]
        [BindProperty]
        [EmailAddress]
        [StringLength(100)]
        public required string Email { get; set; }

        [Required(ErrorMessage ="Password is required")]
        [BindProperty]
        [DataType(DataType.Password)]
        [StringLength(12, MinimumLength = 6)]
        public required string Password { get; set; }

        
        public int VerificationCode { get; set; }
        
        public bool IsEmailVerified { get; set; }

        [NotMapped]
        [Compare("Password")]
        [Required(ErrorMessage ="Confirm password is required")]
        [BindProperty]
        [DataType(DataType.Password)]
        [StringLength(12, MinimumLength = 6)]
        public required string ConfirmPassword { get; set; }

        public override string ToString()
        {
            return string.Format("User: {0}, {1}, {2}, {3}, {4}, {5}, {6}", Id, Name, Email, Password, ConfirmPassword, IsEmailVerified, VerificationCode);  
        }
    }
}