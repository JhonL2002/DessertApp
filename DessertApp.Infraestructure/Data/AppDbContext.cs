using DessertApp.Infraestructure.IdentityModels;
using DessertApp.Models.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

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
        public DbSet<InventoryAnalysis> InventoryAnalyses { get; set; }
        public DbSet<DessertIngredient> DessertIngredients { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<SaleDetail> SaleDetails { get; set; }
        public DbSet<MeasurementUnit> Units { get; set; }
        public DbSet<IngredientUnit> IngredientUnits { get; set; }
        public DbSet<PurchaseOrder> PurchaseOrders { get; set; }
        public DbSet<PurchaseOrderDetail> PurchaseOrderDetails { get; set; }
        public DbSet<PendingReplenishment> PendingReplenishments { get; set; }

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
                    .HasDefaultValue(0);

                entity.Property(i => i.IsAvailable)
                    .IsRequired()
                    .HasDefaultValue(false);

                entity.HasOne(i => i.IngredientUnit)
                    .WithOne(i => i.Ingredient)
                    .HasForeignKey<IngredientUnit>(iu => iu.IngredientId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            //IngredientUnit
            builder.Entity<IngredientUnit>(entity =>
            {
                entity.HasKey(iu => iu.Id);

                entity.HasOne(iu => iu.Unit)
                    .WithMany()
                    .HasForeignKey(iu => iu.UnitId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(iu => iu.Ingredient)
                    .WithOne(i => i.IngredientUnit)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.Property(iu => iu.CostPerUnit)
                    .HasPrecision(18, 2);

                entity.Property(iu => iu.ItemsPerUnit)
                    .HasDefaultValue(0);

                entity.Property(iu => iu.MonthlyHoldingCostRate)
                    .HasPrecision(18, 2);

                entity.Property(iu => iu.OrderingCost)
                    .HasPrecision(18, 2);
            });

            //DessertIngredient
            builder.Entity<DessertIngredient>(entity =>
            {
                entity.HasKey(di => di.Id);

                entity.Property(di => di.QuantityRequired)
                    .IsRequired()
                    .HasPrecision(18, 2);

                entity.HasOne(di => di.Dessert)
                    .WithMany(d => d.DessertIngredients)
                    .HasForeignKey(di => di.DessertId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(di => di.Unit)
                    .WithMany()
                    .HasForeignKey(mu => mu.UnitId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(di => di.Ingredient)
                    .WithMany()
                    .HasForeignKey(di => di.IngredientId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            //PurchaseOrder
            builder.Entity<PurchaseOrder>(entity =>
            {
                entity.HasKey(po => po.Id);

                entity.Property(po => po.TotalCost)
                    .HasPrecision(18, 2);
            });

            //PurchaseOrderDetail
            builder.Entity<PurchaseOrderDetail>(entity =>
            {
                entity.HasKey(pod => pod.Id);

                entity.HasOne(pod => pod.PurchaseOrder)
                    .WithMany(po => po.OrderDetails)
                    .HasForeignKey(pod => pod.PurchaseOrderId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(pod => pod.Ingredient)
                    .WithMany()
                    .HasForeignKey(pod => pod.IngredientId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.Property(pod => pod.Subtotal)
                    .HasPrecision(18, 2);

                entity.Property(pod => pod.UnitCost)
                    .HasPrecision(18, 2);
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
                entity.HasKey(mu => mu.Id);

                entity.Property(mu => mu.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            //UnitConversion
            builder.Entity<UnitConversion>(entity =>
            {
                entity.HasKey(uc => uc.Id);

                entity.HasOne(uc => uc.FromUnit)
                    .WithMany()
                    .HasForeignKey(uc => uc.FromUnitId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(uc => uc.ToUnit)
                    .WithMany()
                    .HasForeignKey(uc => uc.ToUnitId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.Property(uc => uc.ConversionFactor)
                    .HasPrecision(18, 2);

                entity.Property(uc => uc.IsReversible)
                    .HasDefaultValue(true);

                entity.HasData(
                    new UnitConversion { Id = 1, FromUnitId = 2, ToUnitId = 5, ConversionFactor = 1000, IsReversible = true }, // Kg -> g
                    new UnitConversion { Id = 2, FromUnitId = 5, ToUnitId = 2, ConversionFactor = 0.001m, IsReversible = true }, // g -> Kg

                    new UnitConversion { Id = 3, FromUnitId = 1, ToUnitId = 6, ConversionFactor = 1000, IsReversible = true }, // l -> ml
                    new UnitConversion { Id = 4, FromUnitId = 6, ToUnitId = 1, ConversionFactor = 0.001m, IsReversible = true }, // ml -> l 

                    new UnitConversion { Id = 5, FromUnitId = 3, ToUnitId = 7, ConversionFactor = 30, IsReversible = true}, // bucket -> unit
                    new UnitConversion { Id = 6, FromUnitId = 7, ToUnitId = 3, ConversionFactor = 0.033m, IsReversible = true } // unit -> bucket
                );
            });

            //InventoryAnalysis
            builder.Entity<InventoryAnalysis>(entity =>
            {
                entity.HasKey(ia => ia.Id);

                entity.HasOne(ia => ia.Ingredient)
                    .WithMany(i => i.InventoryAnalyses)
                    .HasForeignKey(iu => iu.IngredientId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.Property(ia => ia.IngredientName)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(ia => ia.OrderFrequency)
                    .HasPrecision(18, 2);

                entity.Property(ia => ia.OptimalOrderingPeriod)
                    .HasPrecision(18, 2);

                entity.Property(ia => ia.OrderingCost)
                    .HasPrecision(18, 2);

                entity.Property(ia => ia.AnnualDemand)
                    .HasPrecision(18, 2);

                entity.Property(ia => ia.CostOfMaintainingUnitsInInventory)
                    .HasPrecision(18, 2);
            });
            base.OnModelCreating(builder);
        }
    }
}
