using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ProductWebApi.Models.Domains
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Product Name is required.")]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(255)]
        public string Description { get; set; }

        [Required(ErrorMessage = "Please enter a price.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0.")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Please enter quantity.")]
        [Range(0, int.MaxValue, ErrorMessage = "Quantity cannot be negative.")]
        public int Quantity { get; set; }

        [StringLength(50)]
        public string Brand { get; set; }

        [StringLength(50)]
        public string Category { get; set; }

       // [DataType(DataType.ImageUrl)]
       // public string ImageUrl { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

      //  [DataType(DataType.DateTime)]
      //  public DateTime? UpdatedAt { get; set; }

       // [StringLength(100)]
       // public string SKU { get; set; } // Stock Keeping Unit

        [Range(0, double.MaxValue, ErrorMessage = "Discount cannot be negative.")]
        [DisplayName("Discount")]
        public double DiscountPercentage { get; set; }

        public bool IsActive { get; set; } = true;

        [StringLength(100)]
        public string SupplierName { get; set; }
    }
}
