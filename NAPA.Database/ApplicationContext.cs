using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NAPA.Models;
using System;
using System.Reflection.Emit;
using System.Security.AccessControl;

namespace NAPA.Database
{
    public class ApplicationContext : IdentityDbContext<User, Role, int, UserClaim, UserRole, UserLogin, RoleClaim, UserToken>
    {
        public ApplicationContext(string connectionString) : base(GetOptions(connectionString))
        {
        }
        public DbSet<Product> Products { get; set; }
        public DbSet<Audit> AuditLogs { get; set; }
        private static DbContextOptions GetOptions(string connectionString)
        {
            return SqliteDbContextOptionsBuilderExtensions.UseSqlite(new DbContextOptionsBuilder(), connectionString).Options;
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<User>(b =>
            {
                b.HasKey(u => u.Id);
                b.HasIndex(u => u.NormalizedUserName).HasName("UserNameIndex").IsUnique();
                b.HasIndex(u => u.NormalizedEmail).HasName("EmailIndex");
                b.ToTable("Users");
                b.Property(u => u.ConcurrencyStamp).IsConcurrencyToken();
                b.Property(u => u.UserName).HasMaxLength(256);
                b.Property(u => u.NormalizedUserName).HasMaxLength(256);
                b.Property(u => u.Email).HasMaxLength(256);
                b.Property(u => u.NormalizedEmail).HasMaxLength(256);
                b.HasMany<UserClaim>().WithOne().HasForeignKey(uc => uc.UserId).IsRequired();
                b.HasMany<UserLogin>().WithOne().HasForeignKey(ul => ul.UserId).IsRequired();
                b.HasMany<UserToken>().WithOne().HasForeignKey(ut => ut.UserId).IsRequired();
                b.HasMany<UserRole>().WithOne().HasForeignKey(ur => ur.UserId).IsRequired();
            });

            builder.Entity<UserClaim>(b =>
            {
                b.HasKey(uc => uc.Id);
                b.ToTable("UserClaims");
            });

            builder.Entity<UserLogin>(b =>
            {
                b.HasKey(l => new { l.LoginProvider, l.ProviderKey });
                b.Property(l => l.LoginProvider).HasMaxLength(128);
                b.Property(l => l.ProviderKey).HasMaxLength(128);
                b.ToTable("UserLogins");
            });
            builder.Entity<Audit>(b =>
            {
                b.HasKey(t => new { t.Id });
                b.ToTable("product_audit");
            });

            builder.Entity<UserToken>(b =>
            {
                int maxKeyLength = 25;
                b.HasKey(t => new { t.UserId, t.LoginProvider, t.Name });
                b.Property(t => t.LoginProvider).HasMaxLength(maxKeyLength);
                b.Property(t => t.Name).HasMaxLength(maxKeyLength);
                b.ToTable("UserTokens");
            });

            builder.Entity<Role>(b =>
            {
                b.HasKey(r => r.Id);
                b.HasIndex(r => r.NormalizedName).HasName("RoleNameIndex").IsUnique();
                b.ToTable("Roles");
                b.Property(r => r.ConcurrencyStamp).IsConcurrencyToken();
                b.Property(u => u.Name).HasMaxLength(256);
                b.Property(u => u.NormalizedName).HasMaxLength(256);
                b.HasMany<UserRole>().WithOne().HasForeignKey(ur => ur.RoleId).IsRequired();
                b.HasMany<RoleClaim>().WithOne().HasForeignKey(rc => rc.RoleId).IsRequired();
            });

            builder.Entity<RoleClaim>(b =>
            {
                b.HasKey(rc => rc.Id);
                b.ToTable("RoleClaims");
            });

            builder.Entity<UserRole>(b =>
            {
                b.HasKey(r => new { r.UserId, r.RoleId });
                b.ToTable("UserRoles");
            });

            builder.Entity<Role>().HasData
            (
                new Role { Id = 1, Name = "Admin", NormalizedName = "Admin".ToUpper() },
                new Role { Id = 2, Name = "User", NormalizedName = "User".ToUpper() }
            );

            var hasher = new PasswordHasher<User>();
            User user1 = new User
            {
                Id = 1,
                Email = "test@gmail.com",
                CreatedDate = DateTime.Now
            };
            user1.PasswordHash = hasher.HashPassword(user1, "P@r0l2003");
            builder.Entity<User>().HasData(user1);
            builder.Entity<UserRole>().HasData
            (
                new UserRole { RoleId = 1, UserId = 1 }
            );

            builder.Entity<Product>().HasData
            (
                new Product
                {
                    Id = 1,
                    Name = "HDD 1TB",
                    Quantity = 55,
                    Price = 74.09,
                    LastChangedDate = DateTime.Now.ToString("dd.MM.yyyy HH:mm")
                },
                new Product
                {
                    Id = 2,
                    Name = "HDD SSD 512GB",
                    Quantity = 102,
                    Price = 190.99,
                    LastChangedDate = DateTime.Now.ToString("dd.MM.yyyy HH:mm")
                },
                new Product
                {
                    Id = 3,
                    Name = "RAM DDR4 16GB",
                    Quantity = 47,
                    Price = 80.32,
                    LastChangedDate = DateTime.Now.ToString("dd.MM.yyyy HH:mm")
                }
            );
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.LogTo(Console.WriteLine);
        }
        public virtual async Task<int> SaveChangesAsync(int userId = 1)
        {
            OnBeforeSaveChanges(userId);
            var result = await base.SaveChangesAsync();
            return result;
        }
        private void OnBeforeSaveChanges(int userId)
        {
            ChangeTracker.DetectChanges();
            var auditEntries = new List<AuditEntry>();
            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.Entity is Audit || entry.State == EntityState.Detached || entry.State == EntityState.Unchanged)
                    continue;
                var auditEntry = new AuditEntry(entry);
                auditEntry.TableName = entry.Entity.GetType().Name;
                auditEntry.UserId = userId;
                auditEntries.Add(auditEntry);
                foreach (var property in entry.Properties)
                {
                    string propertyName = property.Metadata.Name;
                    if (property.Metadata.IsPrimaryKey())
                    {
                        auditEntry.KeyValues[propertyName] = property.CurrentValue;
                        continue;
                    }
                    switch (entry.State)
                    {
                        case EntityState.Added:
                            auditEntry.AuditType = AuditType.Create;
                            auditEntry.NewValues[propertyName] = property.CurrentValue;
                            break;
                        case EntityState.Deleted:
                            auditEntry.AuditType = AuditType.Delete;
                            auditEntry.OldValues[propertyName] = property.OriginalValue;
                            break;
                        case EntityState.Modified:
                            if (property.IsModified)
                            {
                                auditEntry.ChangedColumns.Add(propertyName);
                                auditEntry.AuditType = AuditType.Update;
                                auditEntry.OldValues[propertyName] = property.OriginalValue;
                                auditEntry.NewValues[propertyName] = property.CurrentValue;
                            }
                            break;
                    }
                }
            }
            foreach (var auditEntry in auditEntries)
            {
                AuditLogs.Add(auditEntry.ToAudit());
            }
        }
    }
}