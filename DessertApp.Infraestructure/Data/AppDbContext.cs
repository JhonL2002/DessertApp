using DessertApp.Infraestructure.IdentityModels;
using DessertApp.Models.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace DessertApp.Infraestructure.Data
{
    public class AppDbContext : IdentityDbContext<AppUser, AppRole, string>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            
        }

        //Configure DbSet properties
        public DbSet<Dessert> Desserts {  get; set; }
        public DbSet<DessertCategory> DessertCategories { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<DessertIngredient> DessertIngredients { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<SaleDetail> SaleDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            //Configure entities here to determine relationships and behaviors

            //Dessert
            builder.Entity<Dessert>(entity =>
            {
                entity.Property(d => d.Name)
                .IsRequired()
                .HasMaxLength(100);

                entity.Property(d =>d.Description)
                .HasMaxLength(500);

                entity.Property(d => d.Stock)
                .IsRequired();

                entity.Property(d => d.Price)
                .IsRequired()
                .HasPrecision(18,2);

                //One dessert can have one dessert category
                entity.HasOne(d => d.DessertCategory)
                //One dessert category belongs to many desserts
                .WithMany(dc => dc.Desserts)
                .HasForeignKey(d => d.DessertCategoryId)
                .OnDelete(DeleteBehavior.Restrict);
            });

            //DessertCategory
            builder.Entity<DessertCategory>(entity =>
            {
                entity.Property(dc => dc.Name)
                .IsRequired()
                .HasMaxLength(100);

                entity.Property(dc => dc.Description)
                .HasMaxLength(500);

                entity.HasMany(dc => dc.Desserts)
                .WithOne(d => d.DessertCategory)
                .HasForeignKey(d => d.DessertCategoryId);
            });

            //Ingredient
            builder.Entity<Ingredient>(entity =>
            {
                entity.HasKey(i => i.Id);

                entity.Property(i => i.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(i => i.Stock)
                    .IsRequired()
                    .HasDefaultValue(0);

                entity.Property(i => i.IsAvailable)
                    .IsRequired()
                    .HasDefaultValue(true);

                entity.Property(i => i.CostPerUnit)
                    .IsRequired()
                    .HasPrecision(18, 2);

                entity.Property(i => i.OrderingCost)
                    .IsRequired()
                    .HasPrecision(18, 2);

                entity.Property(i => i.MonthlyHoldingCostRate)
                    .IsRequired()
                    .HasPrecision(18, 4);

                entity.Property(i => i.AnnualDemand)
                    .IsRequired();

                // Ignore calculated properties
                entity.Ignore(i => i.AnnualHoldingCost);
                entity.Ignore(i => i.EconomicOrderQuantity);
                entity.Ignore(i => i.PeriodicOrderQuantity);
            });

            //DessertIngredient
            builder.Entity<DessertIngredient>(entity =>
            {
                entity.HasKey(di => new
                {
                    di.DessertId, di.IngredientId
                });

                entity.Property(di => di.QuantityRequired)
                .IsRequired()
                .HasDefaultValue(1);

                entity.HasOne<Dessert>()
                .WithMany()
                .HasForeignKey(di => di.DessertId)
                .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne<Ingredient>()
                .WithMany()
                .HasForeignKey(di => di.IngredientId)
                .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(di => di.Unit)
                .WithMany()
                .HasForeignKey(di => di.UnitId)
                .OnDelete(DeleteBehavior.Restrict);
            });

            //Sale
            builder.Entity<Sale>(entity =>
            {
                entity.Property(s => s.Date)
                .IsRequired();

                entity.Property(s => s.TotalAmount)
                .IsRequired()
                .HasPrecision(18, 2);

                entity.Property(s => s.PaymentMethod)
                .IsRequired()
                .HasMaxLength(50);

                entity.Property(s => s.CreatedByUserId)
                .IsRequired();

                entity.HasMany(s => s.SaleDetails)
                .WithOne(sd => sd.Sale)
                .HasForeignKey(sd => sd.SaleId)
                .OnDelete(DeleteBehavior.Cascade);
            });

            //SaleDetail
            builder.Entity<SaleDetail>(entity =>
            {
                entity.Property(sd => sd.Quantity)
                .IsRequired();

                entity.Property(sd => sd.Subtotal)
                .IsRequired()
                .HasPrecision(18, 2);

                entity.HasOne(sd => sd.Sale)
                .WithMany(s => s.SaleDetails)
                .HasForeignKey(sd => sd.SaleId)
                .OnDelete(DeleteBehavior.Cascade);


                entity.HasOne(sd => sd.Dessert)
                .WithMany()
                .HasForeignKey(sd => sd.DessertId)
                .OnDelete(DeleteBehavior.Restrict);
            });

            //MeasurementUnit
            builder.Entity<MeasurementUnit>(entity =>
            {
                entity.Property(mu => mu.Name)
                .IsRequired()
                .HasMaxLength(50);
            });

            base.OnModelCreating(builder);
        }
    }
}
