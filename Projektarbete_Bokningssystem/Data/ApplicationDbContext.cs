using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Projektarbete_Bokningssystem.Models;

namespace Projektarbete_Bokningssystem.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    public DbSet<StudyRoom> StudyRooms { get; set; }
    public DbSet<Booking> Bookings { get; set; }

    // Skapa tre olika studierum
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Konfigurera relationer
        modelBuilder.Entity<Booking>()
            .HasOne(b => b.User)
            .WithMany()
            .HasForeignKey(b => b.UserId);

        modelBuilder.Entity<Booking>()
            .HasOne(b => b.StudyRoom)
            .WithMany(r => r.Bookings)
            .HasForeignKey(b => b.StudyRoomId);

        // Skapa tre studierum
        modelBuilder.Entity<StudyRoom>().HasData(
            new StudyRoom { Id = 1, Name = "Studierum 1" },
            new StudyRoom { Id = 2, Name = "Studierum 2" },
            new StudyRoom { Id = 3, Name = "Studierum 3" }
        );
    }
}

