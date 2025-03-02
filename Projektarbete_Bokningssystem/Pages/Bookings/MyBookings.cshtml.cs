using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Projektarbete_Bokningssystem.Data;
using Projektarbete_Bokningssystem.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Projektarbete_Bokningssystem.Pages.Bookings
{
    [Authorize]
    public class MyBookingsModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public MyBookingsModel(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IList<Booking> Bookings { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Challenge(); // Omdirigera till inloggning
            }

            // Hämta användarens bokningar
            Bookings = await _context.Bookings
                .Include(b => b.StudyRoom)
                .Where(b => b.UserId == user.Id && b.Status == BookingStatus.Confirmed)
                .OrderBy(b => b.BookingDate)
                .ToListAsync();

            // Logga antalet bokningar
            Console.WriteLine($"Antal bokningar: {Bookings.Count}");

            // Logga detaljer om varje bokning
            foreach (var booking in Bookings)
            {
                Console.WriteLine($"Bokning: {booking.StudyRoom?.Name ?? "Inget rum"}, Datum: {booking.BookingDate}");
            }

            return Page();
        }
        public async Task<IActionResult> OnPostAsync(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Challenge();
            }

            // Hitta bokningen
            var booking = await _context.Bookings
                .FirstOrDefaultAsync(b => b.Id == id && b.UserId == user.Id);

            if (booking == null)
            {
                return NotFound();
            }

            // Avboka (sätt status till Cancelled)
            booking.Status = BookingStatus.Cancelled;
            await _context.SaveChangesAsync();

            StatusMessage = "Bokningen har avbokats.";
            return RedirectToPage();
        }
    }
}