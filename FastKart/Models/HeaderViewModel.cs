namespace FastKart.Models
{
    public class HeaderViewModel

    {
        public List<BasketViewModel> Basket { get; set; } 
        public int Count { get; set; }
        public double Sum { get; set; }
    }
}
