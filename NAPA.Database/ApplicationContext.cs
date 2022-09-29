using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NAPA.Models;
using System.Reflection.Emit;

namespace NAPA.Database
{
    public class ApplicationContext : IdentityDbContext<User, Role, int, UserClaim, UserRole, UserLogin, RoleClaim, UserToken>
    {
        public ApplicationContext(string connectionString) : base(GetOptions(connectionString))
        {
        }
        public DbSet<Product> Products { get; set; }
        private static DbContextOptions GetOptions(string connectionString)
        {
            return SqlServerDbContextOptionsExtensions.UseSqlServer(new DbContextOptionsBuilder(), connectionString).Options;
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Role>().HasData
            (
                new Role { Id = 1, Name = "Admin", NormalizedName = "Admin".ToUpper() },
                new Role { Id = 2, Name = "User", NormalizedName = "User".ToUpper() }
            );
            var hasher = new PasswordHasher<User>();
            builder.Entity<User>().HasData(new User
            {
                Id = 1,
                UserName = "admin",
                NormalizedUserName = "admin",
                Email = "ruzimurodabdunazarov2003@gmail.com",
                NormalizedEmail = "ruzimurodabdunazarov2003@gmail.com",
                EmailConfirmed = false,
                PasswordHash = hasher.HashPassword(null, "Admin123#"),
                SecurityStamp = string.Empty
            });

            builder.Entity<UserClaim>().HasKey(i => i.Id);
            builder.Entity<UserLogin>().HasNoKey();
            builder.Entity<UserRole>().HasNoKey();
            builder.Entity<UserToken>().HasNoKey();
            builder.Entity<Product>().HasData
            (
                new Product
                {
                    Id = 1,
                    Name = "HDD 1TB",
                    Quantity = 55,
                    Price = 74.09,
                    CreatedDate = DateTime.Now.ToString("dd.MM.yyyy HH:mm")
                },
                new Product
                {
                    Id = 2,
                    Name = "HDD SSD 512GB",
                    Quantity = 102,
                    Price = 190.99,
                    CreatedDate = DateTime.Now.ToString("dd.MM.yyyy HH:mm")
                },
                new Product
                {
                    Id = 3,
                    Name = "RAM DDR4 16GB",
                    Quantity = 47,
                    Price = 80.32,
                    CreatedDate = DateTime.Now.ToString("dd.MM.yyyy HH:mm")
                }
            );
        }
    }
}