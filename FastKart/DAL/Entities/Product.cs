using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations.Schema;

namespace FastKart.DAL.Entities
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }
        public double Price { get; set; }
        public string? ImageUrl { get; set; }
        [NotMapped]
        public IFormFile? ImageFile { get; set; }
        public int CategoryId { get; set; }
        public Category? Category { get; set; }
        [NotMapped]
        public List<SelectListItem>? CategoryListItems { get; set; }
    }
}
