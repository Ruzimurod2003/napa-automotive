using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NAPA.Models;

namespace NAPA.Database
{
    public class ApplicationContext : IdentityDbContext<User>
    {
        public ApplicationContext(string connectionString) : base(GetOptions(connectionString))
        {
        }
        
        private static DbContextOptions GetOptions(string connectionString)
        {
            return SqlServerDbContextOptionsExtensions.UseSqlServer(new DbContextOptionsBuilder(), connectionString).Options;
        }
    }
}