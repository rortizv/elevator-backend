using Microsoft.EntityFrameworkCore;

namespace ElevatorBackend.Models
{
    public partial class ElevatorDBContext : DbContext
    {
        public ElevatorDBContext(DbContextOptions<ElevatorDBContext> options) : base(options)
        {
        }
        public virtual DbSet<Elevator> Elevators { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Elevator>(entity => {
                entity.HasKey(k => k.Id);
                entity.Property(e => e.Name).IsRequired();
                entity.Property(e => e.Location).IsRequired();
                entity.Property(e => e.DoorsOpen).IsRequired();
            });
            OnModelCreatingPartial(modelBuilder);
        }
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
