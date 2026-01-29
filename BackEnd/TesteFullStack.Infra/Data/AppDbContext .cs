using Microsoft.EntityFrameworkCore;
using TesteFullStack.Domain.Entities;

namespace TesteFullStack.Infra.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<Person> Persons { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Person>(entity =>
            {
                entity.ToTable("persons");

                entity.HasKey(x => x.Id);

                entity.Property(x => x.Name)
                .HasMaxLength(200);

                entity.Property(x => x.Age)
                .IsRequired();
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("categories");

                entity.HasKey(x => x.Id);

                entity.Property(x => x.Description)
                      .HasMaxLength(400)
                      .IsRequired(false);

                entity.Property(x => x.Purpose)
                      .HasMaxLength(20)
                      .IsRequired();
            });

            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.ToTable("transactions");
                entity.HasKey(x => x.Id);

                entity.Property(x => x.Description)
                      .HasMaxLength(400);

                entity.Property(x => x.Value)
                      .IsRequired();

                entity.Property(x => x.Type)
                      .HasMaxLength(10)
                      .IsRequired();

                entity.Property(x => x.PersonId)
                      .IsRequired();

                entity.Property(x => x.CategoryId)
                      .IsRequired();

                entity.HasOne(t => t.Person)
                    .WithMany()
                    .HasForeignKey(t => t.PersonId)
                    .OnDelete(DeleteBehavior.Cascade); 

                entity.HasOne(t => t.Category)
                      .WithMany()
                      .HasForeignKey(t => t.CategoryId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            base.OnModelCreating(modelBuilder);

        }
    }
}
