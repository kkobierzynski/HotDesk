using Microsoft.EntityFrameworkCore;

namespace HotDesk.Entities
{
    public class HotDeskDbContext : DbContext
    {
        private string connection = @"Server = (localdb)\mssqllocaldb; Database = HotDeskDb; Trusted_Connection = true";

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Desk> Desks { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .Property(x => x.Name)
                .IsRequired();

            modelBuilder.Entity<User>()
                .Property(x => x.Surname)
                .IsRequired();

            modelBuilder.Entity<User>()
                .Property(x => x.Email)
                .IsRequired();

            modelBuilder.Entity<Role>()
                .Property(x => x.RoleName)
                .IsRequired();

            modelBuilder.Entity<Reservation>()
                .Property(x => x.DeskId)
                .IsRequired();

            modelBuilder.Entity<Reservation>()
                .Property(x => x.StartDate)
                .IsRequired();

            modelBuilder.Entity<Reservation>()
                .Property(x => x.EndDate)
                .IsRequired();

            modelBuilder.Entity<Location>()
                .Property(x => x.Country)
                .IsRequired();

            modelBuilder.Entity<Location>()
                .Property(x => x.City)
                .IsRequired();

            modelBuilder.Entity<Location>()
                .Property(x => x.Address)
                .IsRequired();

            modelBuilder.Entity<Desk>()
                .Property(x => x.Description)
                .HasMaxLength(100);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(connection);
        }
    }
}
