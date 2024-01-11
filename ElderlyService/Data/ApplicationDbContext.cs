using ElderlyService.Models;
using Microsoft.EntityFrameworkCore;

namespace ElderlyService.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<Roles> Roles { get; set; }
        public DbSet<Users> Users { get; set; }
        public DbSet<Caregiver> Caregivers { get; set; }
        public DbSet<Reviews> Reviews { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Service> services { get; set; }
        public DbSet<Experience> Experiences { get; set; }
        public DbSet<Availability> Availabilities { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<ReviewsForWebsites> reviewsForWebsites { get; set; }
        public DbSet<CardData> Cards { get; set; }

        public List<Service> GetService()
        {
            return services.ToList();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Users>().HasIndex(x => x.Email).IsUnique();

            modelBuilder.Entity<Roles>()
                .HasMany(u => u.Users)
                .WithOne(r => r.Roles)
                .HasForeignKey(f => f.RoleId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Users>()
                .HasMany(u => u.Reviews)
                .WithOne(r => r.Users)
                .HasForeignKey(r => r.userId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Users>()
               .HasMany(u => u.caregivers)
               .WithOne(r => r.Users)
               .HasForeignKey(r => r.userId)
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Users>()
                .HasMany(u=>u.Appointments)
                .WithOne(r => r.Users)
                .HasForeignKey(r=>r.userId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Users>()
               .HasMany(u => u.ReviewsForWebsites)
               .WithOne(r => r.Users)
               .HasForeignKey(r => r.userId)
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Caregiver>()
                .HasMany(u => u.Reviews)
                .WithOne(r => r.Caregiver)
                .HasForeignKey(r => r.CaregiverId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Caregiver>()
                .HasMany(u => u.Appointments)
                .WithOne(r => r.Caregiver)
                .HasForeignKey(r => r.CaregiverId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Caregiver>()
                .HasMany(u => u.Experiences)
                .WithOne(r => r.Caregiver)
                .HasForeignKey(r => r.CaregiverId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Caregiver>()
                .HasMany(u => u.Availabilities)
                .WithOne(r => r.Caregiver)
                .HasForeignKey(r => r.CaregiverId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Caregiver>()
                .HasMany(u => u.payments)
                .WithOne(r => r.Caregiver)
                .HasForeignKey(r => r.CaregiverId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Service>()
                .HasMany(u => u.Caregivers)
                .WithOne(r => r.Service)
                .HasForeignKey(r => r.ServiceId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Roles>().HasData(
                new Roles { RoleId = "1", TypeOfUser = "Admin" },
                new Roles { RoleId = "2", TypeOfUser = "Caregiver" },
                new Roles { RoleId = "3", TypeOfUser = "User" }
                );

            modelBuilder.Entity<Users>().HasData(
                    new Users { 
                        userId = "1",
                        FirstName = "mo'men",
                        LastName = "Safi",
                        Password = "123456",
                        Email = "Safi.moumen90@gmail.com",
                        Phone = "0796959979",
                        City = "amman",
                        DateOfBirth = new DateTime(1999, 4, 5),
                        Gender = 1, 
                        RoleId = "1" 
                    });

            base.OnModelCreating(modelBuilder);
        }
    }
}
