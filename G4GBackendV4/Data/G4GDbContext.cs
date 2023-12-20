using G4GBackendV4.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data;
using System.Reflection.Emit;

namespace G4GBackendV4.Data
{
    public class G4GDbContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public G4GDbContext(IConfiguration configuration)
        {
            _configuration = configuration;
            //base.Database.EnsureDeleted();
            //base.Database.EnsureCreated();
        }

        public DbSet<User>? Users { get; set; }
        public DbSet<Category>? Categories { get; set; }
        public DbSet<SubCategory>? SubCategories { get; set; }
        public DbSet<Content>? Contents { get; set; }
        public DbSet<Comment>? Comments { get; set; }
        public DbSet<Role>? Roles { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(q => q.Id);
                entity.Property(q => q.Id)
                    .ValueGeneratedOnAdd();

                entity.Property(q => q.Username)
                    .IsRequired();

                entity.Property(q => q.PasswordHash)
                    .IsRequired();
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(q => q.Id);
                entity.Property(q => q.Id).ValueGeneratedOnAdd();

                entity.Property(q => q.Name).IsRequired();

                entity.HasMany(q => q.SubCategories)
                    .WithOne(q => q.Category)
                    .HasForeignKey(q => q.CategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<Comment>(entity =>
            {
                entity.HasKey(q => q.Id);
                entity.Property(q => q.Id).ValueGeneratedOnAdd();

                entity.Property(q => q.Text).IsRequired();

                entity.Property(q => q.Posted).IsRequired();

                entity.HasOne(q => q.User)
                    .WithMany(q => q.Comments)
                    .HasForeignKey(q => q.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(q => q.Content)
                    .WithMany(q => q.Comments)
                    .HasForeignKey(q => q.ContentId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<Content>(entity =>
            {
                entity.HasKey(q => q.Id);
                entity.Property(q => q.Id).ValueGeneratedOnAdd();

                entity.Property(q => q.Headline).IsRequired();

                entity.Property(q => q.Text).IsRequired();

                entity.Property(q => q.Views);


                entity.Property(q => q.Posted).IsRequired();

                entity.HasOne(q => q.Subcategory)
                    .WithMany(q => q.Contents)
                    .HasForeignKey(q => q.SubcategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<SubCategory>(entity =>
            {
                entity.HasKey(q => q.Id);
                entity.Property(q => q.Id).ValueGeneratedOnAdd();

                entity.Property(q => q.Name).IsRequired();

                entity.Property(q => q.Icon).IsRequired();
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasKey(q => q.Id);
                entity.Property(q => q.Id).ValueGeneratedOnAdd();

                entity.Property(q => q.Name).IsRequired();

                entity.HasMany(q => q.Users).WithMany(q => q.Roles);
            });
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_configuration.GetConnectionString("G4GDb"));
        }
    }
}