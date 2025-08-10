using Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext() => Database.EnsureCreated();

        public DbSet<Resource> Resources { get; set; }
        public DbSet<Unit> Units { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Balance> Balances { get; set; }
        public DbSet<Income> Incomes { get; set; }
        public DbSet<IncomeResource> IncomeResources { get; set; }
        public DbSet<Shipment> Shipments { get; set; }
        public DbSet<ShipmentResource> ShipmentResources { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Resource>().HasIndex(r => new { r.Name }).IsUnique();
            modelBuilder.Entity<Unit>().HasIndex(u => u.Name).IsUnique();
            modelBuilder.Entity<Client>().HasIndex(c => c.Name).IsUnique();
            modelBuilder.Entity<Income>().HasIndex(i => i.Number).IsUnique();
            modelBuilder.Entity<Shipment>().HasIndex(s => s.Number).IsUnique();
            modelBuilder.Entity<Balance>().HasIndex(b => new { b.ResourceId, b.UnitId }).IsUnique();
            modelBuilder.Entity<Balance>().ToTable(b => b.HasCheckConstraint("CK_Quantity", "\"Quantity\" >= 0"));

            modelBuilder.Entity<Balance>(entity =>
            {
                entity.HasOne(b => b.Resource)
                    .WithMany(r => r.Balances)
                    .HasForeignKey(b => b.ResourceId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(b => b.Unit)
                    .WithMany(u => u.Balances)
                    .HasForeignKey(b => b.UnitId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<IncomeResource>(entity =>
            {
                entity.HasOne(ir => ir.Income)
                    .WithMany(i => i.IncomeResources)
                    .HasForeignKey(ir => ir.IncomeId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(ir => ir.Resource)
                    .WithMany(r => r.IncomeResources)
                    .HasForeignKey(ir => ir.ResourceId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(ir => ir.Unit)
                    .WithMany(u => u.IncomeResources)
                    .HasForeignKey(ir => ir.UnitId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Shipment>(entity =>
            {
                entity.HasOne(s => s.Client)
                    .WithMany(c => c.Shipments)
                    .HasForeignKey(s => s.ClientId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<ShipmentResource>(entity =>
            {
                entity.HasOne(sr => sr.Shipment)
                    .WithMany(s => s.ShipmentResources)
                    .HasForeignKey(sr => sr.ShipmentId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(sr => sr.Resource)
                    .WithMany(r => r.ShipmentResources)
                    .HasForeignKey(sr => sr.ResourceId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(sr => sr.Unit)
                    .WithMany(u => u.ShipmentResources)
                    .HasForeignKey(sr => sr.UnitId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

            var connectionString = configuration.GetConnectionString("PostgreSQL");

            optionsBuilder.UseNpgsql(connectionString);
        }
    }
}
