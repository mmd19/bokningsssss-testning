using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Projektarbete_Bokningssystem.Pages
{
    [Authorize] // Endast inloggade användare kan se denna sidan
    public class HomeModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;

        public HomeModel(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public string UserName { get; set; }

        public async Task OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            UserName = user.UserName;
        }
    }
}
