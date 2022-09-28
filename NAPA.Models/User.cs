using Microsoft.AspNetCore.Identity;
 
namespace NAPA.Models
{
    public class User : IdentityUser
    {
        public int Year { get; set; }
    }
}