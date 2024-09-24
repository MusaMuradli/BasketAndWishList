using System.ComponentModel.DataAnnotations.Schema;

namespace FastKart.DAL.Entities
{
    public class Slider:BaseEntity
    {
        [NotMapped]
        public IFormFile ImageFile { get; set; }
        public string? ImageUrl { get; set; }
        public string OfferName { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

    }
}
