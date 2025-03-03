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

            // Läs in TempData om det finns
            if (TempData["Message"] != null)
            {
                StatusMessage = TempData["Message"].ToString();
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

            try
            {
                // Ta bort bokningen från databasen
                _context.Bookings.Remove(booking);
                await _context.SaveChangesAsync(); // Spara ändringarna till databasen

                StatusMessage = "Bokningen har tagits bort.";
            }
            catch (Exception ex)
            {
                // Hantera eventuella fel, exempelvis logga
                StatusMessage = "Ett fel uppstod när bokningen skulle tas bort.";
            }

            return RedirectToPage();
        }
    }
}