using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Projektarbete_Bokningssystem.Data;
using Projektarbete_Bokningssystem.Models;

namespace Projektarbete_Bokningssystem.Pages.Bookings
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public CreateModel(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty]
        public Booking Booking { get; set; }

        // Property för att lagra bokningsdata för kalendern
        public List<object> BookingEvents { get; set; }

        public SelectList RoomList { get; set; } //Bindas till en dropdown meny

        public void OnGet()
        {
            // Kolla om det finns några studierum i databasen
            if (!_context.StudyRooms.Any())
            {
                // Om inte, skapa tre grundläggande rum
                var rooms = new List<StudyRoom>
            {
                new StudyRoom { Name = "Studierum 1" },
                new StudyRoom { Name = "Studierum 2" },
                new StudyRoom { Name = "Studierum 3" }
            };
                _context.StudyRooms.AddRange(rooms);
                _context.SaveChanges();
            }

            // Hämta rum för dropdown-listan
            RoomList = new SelectList(_context.StudyRooms, "Id", "Name");

            // Sätt dagens datum som standard
            Booking = new Booking
            {
                BookingDate = DateTime.Today
            };

            // Hämta bokningar från databasen för kalendern
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

        public async Task<IActionResult> OnPostAsync()
        {
            // Hämta inloggad användare
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Challenge();
            }

            // Sätt användare för bokningen INNAN validering av formulär
            Booking.UserId = user.Id;
            Booking.CreatedAt = DateTime.Now;
            Booking.Status = BookingStatus.Confirmed;

            // Ta bort validering för egenskaper som kommer att hanteras automatiskt
            ModelState.Remove("Booking.User");
            ModelState.Remove("Booking.StudyRoom");


            // Kontrollera om rummet redan är bokat
            var existingBooking = await _context.Bookings
                .Where(b => b.StudyRoomId == Booking.StudyRoomId &&
                        b.BookingDate.Date == Booking.BookingDate.Date &&
                        b.Status == BookingStatus.Confirmed)
                .FirstOrDefaultAsync();

            if (existingBooking != null)
            {
                ModelState.AddModelError(string.Empty, "Detta rum är redan bokat för det valda datumet.");
                RoomList = new SelectList(_context.StudyRooms, "Id", "Name");
                return Page();
            }

            try
            {
                _context.Bookings.Add(Booking);
                await _context.SaveChangesAsync();
                ViewData["Message"] = "Bokning sparad!";
                ViewData["MessageType"] = "success";
                return Page();
            }
            catch (Exception ex)
            {
                // Optionally log the exception
                ViewData["Message"] = "Det uppstod ett fel vid bokningen.";
                ViewData["MessageType"] = "danger";
                return Page();
            }
        }
    }
}


