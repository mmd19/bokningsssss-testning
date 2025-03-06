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
    [Authorize(Roles ="Admin")]
    public class AdminCreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public AdminCreateModel(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty]
        public Booking Booking { get; set; }

        [BindProperty]
        public string SelectedUserId { get; set; }

        public SelectList RoomList { get; set; }
        public SelectList UserList { get; set; }
        public List<object> BookingEvents { get; set; }

        public void LoadBookingData()
        {
            RoomList = new SelectList(_context.StudyRooms, "Id", "Name");
            UserList = new SelectList(_context.Users, "Id", "Email");

            var bookings = _context.Bookings
                .Include(b => b.StudyRoom)
                .Where(b => b.Status == BookingStatus.Confirmed)
                .ToList();

            BookingEvents = bookings.Select(b => new
            {
                id = b.Id,
                title = b.StudyRoom.Name + " (Bokat)",
                start = b.BookingDate.ToString("yyyy-MM-dd"),
                roomId = b.StudyRoomId,
                allDay = true
            }).ToList<object>();

        }
        public void OnGet()
        {
            Booking = new Booking
            {
               BookingDate = DateTime.Today
            };
             
            LoadBookingData();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (string.IsNullOrEmpty(SelectedUserId))
            {
                ModelState.AddModelError(string.Empty, "Du måste välja en användare.");
                LoadBookingData();
                return Page();
            }

            var user = await _userManager.FindByIdAsync(SelectedUserId);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Vald användare hittades inte.");
                LoadBookingData();
                return Page();
            }

            var userBookingsCount = await _context.Bookings
                .Where(b => b.UserId == user.Id && b.Status == BookingStatus.Confirmed)
                .CountAsync();

            if (userBookingsCount >= 3)
            {
                ModelState.AddModelError(string.Empty, "Denna användare har redan 3 bokningar.");
                LoadBookingData();
                return Page();
            }

            var existingBooking = await _context.Bookings
                .Where(b => b.StudyRoomId == Booking.StudyRoomId &&
                            b.BookingDate.Date == Booking.BookingDate.Date &&
                            b.Status == BookingStatus.Confirmed)
                .FirstOrDefaultAsync();

            if (existingBooking != null)
            {
                ModelState.AddModelError(string.Empty, "Detta rum är redan bokat för det valda datumet.");
                LoadBookingData();
                return Page();
            }

            Booking.UserId = user.Id;
            Booking.CreatedAt = DateTime.Now;
            Booking.Status = BookingStatus.Confirmed;

            ModelState.Remove("Booking.User");
            ModelState.Remove("Booking.StudyRoom");

            try
            {
                _context.Bookings.Add(Booking);
                await _context.SaveChangesAsync();
                TempData["Message"] = "Bokning skapad!";
                TempData["MessageType"] = "success";
                return RedirectToPage("/Admin/AllBookings");
            }
            catch
            {
                ViewData["Message"] = "Ett fel uppstod vid bokningen.";
                ViewData["MessageType"] = "danger";
                LoadBookingData();
                return Page();
            }
        }
    }
}
