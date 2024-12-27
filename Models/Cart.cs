using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project.Models
{
    public class Cart
    {
        [Key]
        public int Id { get; set; }

        public int Quantity { get; set; }

        public string UserId { get; set; }

        [ForeignKey("Item")]
        public int ItemId { get; set; }

        public Item Items { get; set; }
    }
}
