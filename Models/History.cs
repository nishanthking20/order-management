using System.ComponentModel.DataAnnotations;

namespace Order_Management.Models
{
    public class History
    {
        public History(int userId, DateTime date, string? ItemName, decimal Price, int TransactionId) 
        {
            this.userId = userId;
            this.date = date;
            this.ItemName = ItemName;
            this.Price = Price;
            this.TransactionId = TransactionId;
        }

        // [Required]
        [Key]
        public int sno { get; set;}
        public int userId { get; set;}

        [DataType(DataType.Date)]

        public DateTime date { get; set;}
        public string? ItemName { get; set;}
        public decimal Price { get; set;}
        public int TransactionId { get; set;}

        public override string ToString()
        {
            return string.Format(
                "Items: {0}, {1}, {2}, {3}, {4} ",
                userId,
                date,
                ItemName,
                Price,
                TransactionId
            );
        }

    }
}