using Microsoft.AspNetCore.Identity;

namespace Projektarbete_Bokningssystem.Models
{
    public class Booking
    {
        //Primary key
        public int Id { get; set; }

        // Foreign keys
        public string? UserId { get; set; }
        public int StudyRoomId { get; set; }

        // Datum för bokningen
        public DateTime BookingDate { get; set; }

        // När bokningen skapades
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Status
        public BookingStatus Status { get; set; } = BookingStatus.Confirmed;

        // Ärvda egenskaper
        public IdentityUser User { get; set; }
        public StudyRoom StudyRoom { get; set; }
    }

    public enum BookingStatus
    {
        Confirmed,
        Cancelled
    }
}

