using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NAPA.Models
{
    public class ProductAudit
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Type { get; set; }
        public int ProductId { get; set; }
        public string AddedTime { get; set; }
    }
}