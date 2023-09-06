using Microsoft.EntityFrameworkCore;

namespace VShop.ProductApi;

public class AppDbContext : DbContext
{
    const string DEFAULT_COLLATION = "utf8mb4";
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {

    }

    public DbSet<Category> Categories { get; set; }
    public DbSet<Product> Products { get; set; }

    //Fluent API
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //Category
        modelBuilder.Entity<Category>()
            .HasKey(e => e.CategoryId);

        modelBuilder.Entity<Category>()
            .Property(e => e.Name)
            .HasCharSet(DEFAULT_COLLATION)
            .HasMaxLength(100)
            .IsRequired();


        //Product
        modelBuilder.Entity<Product>()
            .Property(e => e.Name)
            .HasMaxLength(100)
            .HasCharSet(DEFAULT_COLLATION)
            .IsRequired();

        modelBuilder.Entity<Product>()
            .Property(e => e.Description)
            .HasMaxLength(255)
            .HasCharSet(DEFAULT_COLLATION)
            .IsRequired();

        modelBuilder.Entity<Product>()
            .Property(e => e.ImageURL)
            .HasMaxLength(255)
            .HasCharSet(DEFAULT_COLLATION)
            .IsRequired();

        modelBuilder.Entity<Product>()
            .Property(e => e.Price)
            .HasPrecision(12, 2);


        modelBuilder.Entity<Category>()
            .HasMany(c => c.Products)
            .WithOne(p => p.Category)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder
            .Entity<Category>()
            .HasData(new Category
            {
                CategoryId = 1,
                Name = "Material Escolar"
            }, new Category
            {
                CategoryId = 2,
                Name = "Acessórios"
            });

    }
}
