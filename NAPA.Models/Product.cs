namespace NAPA.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Quantiy { get; set; }
        public int Price { get; set; }
        public decimal TotalPrice { get; set; }
        public string CreatedDate { get; set; }
    }
}