using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NAPA.ViewModels
{
    public class EditProductVM
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        [RegularExpression("^[0-9]+\\.?[0-9]*$", ErrorMessage = "Sotuv qiymati bunaqa bo'lmaydi")]
        public string Price { get; set; }
    }
}
