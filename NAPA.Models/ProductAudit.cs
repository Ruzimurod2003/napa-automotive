namespace NAPA.Models
{
    public class ProductAudit
    {
        public int UserId { get; set; }
        public string Type { get; set; }
        public int ProductId { get; set; }
        public string AddedTime { get; set; }
    }
}