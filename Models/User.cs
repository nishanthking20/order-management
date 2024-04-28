using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Order_Management.Models
{
    public class User
    {

        public int Id { get; set; }

        [Required(ErrorMessage = "Username is required")]
        [StringLength(100)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [StringLength(12, MinimumLength = 6)]
        public string Password { get; set; }

        [NotMapped]
        [Compare("Password")]
        [Required(ErrorMessage = "Confirm password is required")]
        [DataType(DataType.Password)]
        [StringLength(12, MinimumLength = 6)]
        public string ConfirmPassword { get; set; }

        public override string ToString()
        {
            return string.Format("User: {0}, {1}, {2}, {3}, {4}", Id, Name, Email, Password, ConfirmPassword);
        }
    }
}
