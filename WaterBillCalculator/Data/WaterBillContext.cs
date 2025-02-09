using Microsoft.EntityFrameworkCore;

namespace WaterBillCalculator.Data
{
    public class WaterBillContext : DbContext
    {
        public WaterBillContext(DbContextOptions<WaterBillContext> options)
            : base(options)
        {
        }

        public virtual DbSet<BillDetails> Bills { get; set; }
        public virtual DbSet<MeterDetails> Meters { get; set; }
        public virtual DbSet<MeterReadings> Readings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BillDetails>()
                .HasKey(b => b.Id);
            
            // Define the relationship between BillDetails and MeterReadings
            modelBuilder.Entity<MeterReadings>()
                .HasOne<BillDetails>()
                .WithMany(b => b.MeterReadings)
                .HasForeignKey(mr => mr.BillId);

            // Define the relationship between MeterDetails and MeterReadings
            modelBuilder.Entity<MeterReadings>()
                .HasOne<MeterDetails>(m => m.Meter)
                .WithMany(m => m.MeterReadings)
                .HasForeignKey(mr => mr.MeterId);
            
            // Define the relationship between MeterDetails and Parent/Child MeterDetails
            modelBuilder.Entity<MeterDetails>()
                .HasOne(m => m.ParentMeter)
                .WithMany(m => m.ChildMeters)
                .HasForeignKey(m => m.ParentId)
                .OnDelete(DeleteBehavior.Restrict);
            
            // Seed data
            modelBuilder.Entity<MeterDetails>().HasData(
                new MeterDetails { Id = 1, MeterName = "Main Meter", MeterLocation = "Property", MeterNumber = "0001" },
                new MeterDetails
                {
                    Id = 2, MeterName = "Peters Upper Field", MeterLocation = "Upper Field", MeterNumber = "0002",
                    ParentId = 1
                },
                new MeterDetails
                {
                    Id = 3, MeterName = "Peters Lower Field", MeterLocation = "Lower Field", MeterNumber = "0003",
                    ParentId = 1
                },
                new MeterDetails
                    { Id = 4, MeterName = "Our Field", MeterLocation = "Our Field", MeterNumber = "0004", ParentId = 1 },
                new MeterDetails
                    { Id = 5, MeterName = "The Houses", MeterLocation = "Houses", MeterNumber = "0005", ParentId = 1 },
                new MeterDetails
                {
                    Id = 6, MeterName = "Riverbank Cottage", MeterLocation = "Riverbank Cottage", MeterNumber = "0006",
                    ParentId = 5
                },
                new MeterDetails
                {
                    Id = 7, MeterName = "Riverside Barn", MeterLocation = "Riverside Barn", MeterNumber = "0007",
                    ParentId = 5
                },
                new MeterDetails
                {
                    Id = 8, MeterName = "Waunwen Farm House", MeterLocation = "Waunwen Farm House", MeterNumber = "0008",
                    ParentId = 5
                });
        }
    }
}