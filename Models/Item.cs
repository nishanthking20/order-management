using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Order_Management.Models
{
    public class Item
    {
        public int ItemId { get; set; }

        [Required(ErrorMessage = "Username is required")]
        [StringLength(100)]
        public string? ItemName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [StringLength(100)]
        public string? Image { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(100)]
        public string? Quantity { get; set; }

        
        [Required(ErrorMessage = "Confirm password is required")]
        [StringLength(100)]
        public string? Price { get; set; }

        [Required(ErrorMessage = "Confirm password is required")]
        [StringLength(100)]
        public string? Category { get; set; }

        public override string ToString()
        {
            return string.Format("Items: {0}, {1}, {2}, {3}, {4}, {5}", ItemId, ItemName, Image, Quantity, Price, Category);
        }
    }

}