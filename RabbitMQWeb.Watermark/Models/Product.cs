using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RabbitMQWeb.Watermark.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [StringLength(100)]
        public string Name { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
        public int Stock { get; set; }
        [StringLength(100)]
        public string? ImageName { get; set; }
    }
}
