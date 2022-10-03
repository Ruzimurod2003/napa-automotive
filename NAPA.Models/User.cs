using Microsoft.AspNetCore.Identity;

namespace NAPA.Models
{
    public class User : IdentityUser<int>
    {
        public DateTime CreatedDate { get; set; }
    }
}