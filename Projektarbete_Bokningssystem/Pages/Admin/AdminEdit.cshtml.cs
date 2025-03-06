using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Projektarbete_Bokningssystem.Data;
using Projektarbete_Bokningssystem.Models;

namespace Projektarbete_Bokningssystem.Pages.Admin
{
    [Authorize]
    public class AdminEditModel : PageModel
    {

        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public AdminEditModel(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty]
        public Booking Booking { get; set; }
        public SelectList RoomList { get; set; }
        public List<object> BookingEvents { get; set; }
        
        public void LoadBookingData()
        { 
            // Hämta rum för dropdown-listan
            RoomList = new SelectList(_context.StudyRooms, "Id", "Name");

            // Hämta bokningar för kalendern
            var bookings = _context.Bookings
                .Include(b => b.StudyRoom)
                .Where(b => b.Status == BookingStatus.Confirmed)
                .ToList();

            // Konvertera till FullCalendar-format
            BookingEvents = bookings.Select(b => new
            {
                id = b.Id,
                title = b.StudyRoom.Name + " (Bokat)",
                start = b.BookingDate.ToString("yyyy-MM-dd"),
                roomId = b.StudyRoomId,
                allDay = true
            }).ToList<object>();
        }
        public async Task<IActionResult> OnGetAsync(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Challenge();
            }

            // Kontrollera om användaren är admin
            var isAdmin = await _userManager.IsInRoleAsync(user, "Admin");

            // Om användaren inte är admin, kontrollera om bokningen tillhör användaren
            if (!isAdmin)
            {
                Booking = await _context.Bookings
                    .FirstOrDefaultAsync(b => b.Id == id && b.UserId == user.Id);
            }
            else
            {
                // Om admin, hämta bokningen utan att kontrollera userId
                Booking = await _context.Bookings
                    .FirstOrDefaultAsync(b => b.Id == id);
            }

            if (Booking == null)
            {
                return NotFound();
            }

            // Ladda rumslistan och bokningsdata
            LoadBookingData();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {

            // Hämta användaren
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Challenge();
            }

            // Kontrollera att bokningen tillhör användaren
            var originalBooking = await _context.Bookings
                .FirstOrDefaultAsync(b => b.Id == Booking.Id || b.UserId == user.Id);

            if (originalBooking == null)
            {
                return NotFound();
            }

            // Ta bort validering för dessa egenskaper
            ModelState.Remove("Booking.User");
            ModelState.Remove("Booking.StudyRoom");

            // Kontrollera om rummet redan är bokat det valda datumet (exklusive denna bokning)
            var existingBooking = await _context.Bookings
                .Where(b => b.Id != Booking.Id &&
                       b.StudyRoomId == Booking.StudyRoomId &&
                       b.BookingDate.Date == Booking.BookingDate.Date &&
                       b.Status == BookingStatus.Confirmed)
                .FirstOrDefaultAsync(); // Hämtar asynkront de första elementet i en sekvens som uppfyller ett villkor

            if (existingBooking != null)
            {
                ViewData["Message"] = "Detta rum är redan bokat för det valda datumet.";
                RoomList = new SelectList(_context.StudyRooms, "Id", "Name");
                return Page();
            }
            else
            {
                // Lägg till en loggutskrift här
                ViewData["Message"] = "Else körs";
                // Uppdatera endast de fält som behövs
                originalBooking.StudyRoomId = Booking.StudyRoomId;
                originalBooking.BookingDate = Booking.BookingDate;

                _context.Update(originalBooking);
                await _context.SaveChangesAsync();

                ViewData["Message"] = "Bokningen har uppdaterats!";
                // Efter att bokningen har sparats
                return RedirectToPage("/Bookings/MyBookings");
            }
        }
    }
}




       