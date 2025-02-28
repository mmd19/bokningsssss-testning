using Microsoft.AspNetCore.Identity;

namespace Projektarbete_Bokningssystem.Models
{
    public class StudyRoom
    {
        public int Id { get; set; }
        public string Name { get; set; }

        // Navigationsegenskaper - ett rum kan ha många bokningar
        public ICollection<Booking> Bookings { get; set; }
    }   
}
