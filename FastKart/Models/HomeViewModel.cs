using FastKart.Areas.AdminPanel.Controllers;
using FastKart.DAL.Entities;

namespace FastKart.Models
{
    public class HomeViewModel
    {
        public List<Category> Categories { get; set; } = new List<Category>();
        public List<Product> Products { get; set; } = new List<Product>();
        public Slider Sliders { get; set; }
    }
}
