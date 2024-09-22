using System.ComponentModel.DataAnnotations;

namespace FastKart.DAL.Entities
{
    public class Category : BaseEntity
    {
        [Required, MaxLength(20)]
        public string Name { get; set; }
        public string? ImageUrl { get; set; }
        [MaxLength(100)]
        public  string Description { get; set; }
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
