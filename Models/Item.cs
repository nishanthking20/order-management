using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Order_Management.Models
{
    public class Item
    {
        public int ItemId { get; set; }

        [Required(ErrorMessage = "Itemname is required")]
        [StringLength(100)]
        public string? ItemName { get; set; }

        [Required(ErrorMessage = "Image is required")]
        [StringLength(10000)]
        public string? Image { get; set; }

        [Required(ErrorMessage = "Quantity is required")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "Price is required")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Category is required")]
        [StringLength(100)]
        public string? Category { get; set; }
        
        [NotMapped]
        public int MaxQuantity { get; set; }

        [NotMapped]
        public int ChangedQuantity { get; set; }
        // public override string ToString()
        // {
        //     return string.Format(
        //         "Items: {0}, {1}, {2}, {3}, {4}, {5}",
        //         ItemId,
        //         ItemName,
        //         Image,
        //         Quantity,
        //         Price,
        //         Category
        //     );
        // }
    }
}
