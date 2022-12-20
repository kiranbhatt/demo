using Books.API.Entities;
using Books.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace Books.API.Contexts
{
    public class BookContext : IdentityDbContext<ApplicationUser, ApplicationRole,string>
    {
        public DbSet<Entities.Book> Books { get; set; }

        public DbSet<Entities.LocalUser> LocalUsers { get; set; }

        public DbSet<Entities.ApplicationUser> ApplicationUsers { get; set; }

        public DbSet<ApplicationRole> ApplicationRoles { get; set; }

        public BookContext(DbContextOptions<BookContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            AlterIdentityTables(modelBuilder);

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=BookDatabase.db");

            base.OnConfiguring(optionsBuilder);
        }

        private static void AlterIdentityTables(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("Identity");
            modelBuilder.Entity<ApplicationUser>(entity =>
            {
                entity.ToTable(name: "User");
            });
            modelBuilder.Entity<ApplicationRole>(entity =>
            {
                entity.ToTable(name: "Role");
            });
            modelBuilder.Entity<IdentityUserRole<string>>(entity =>
            {
                entity.ToTable("UserRoles");
            });
            modelBuilder.Entity<IdentityUserClaim<string>>(entity =>
            {
                entity.ToTable("UserClaims");
            });
            modelBuilder.Entity<IdentityUserLogin<string>>(entity =>
            {
                entity.ToTable("UserLogins");
            });
            modelBuilder.Entity<IdentityRoleClaim<string>>(entity =>
            {
                entity.ToTable("RoleClaims");
            });
            modelBuilder.Entity<IdentityUserToken<string>>(entity =>
            {
                entity.ToTable("UserTokens");
            });
        }
    }
}
