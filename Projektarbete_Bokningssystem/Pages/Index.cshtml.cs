using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Projektarbete_Bokningssystem.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly SignInManager<IdentityUser> _signInManager;

    public IndexModel(ILogger<IndexModel> logger, SignInManager<IdentityUser> signInManager)
    {
        _logger = logger;
        _signInManager = signInManager;
    }

    public IActionResult OnGet()
    {
        //Kollar om användaren är inloggad
        if (_signInManager.IsSignedIn(User))
        {
            return RedirectToPage("/Home"); //Skickar inloggad användare till Home
        }

        return Page(); //Visar Index för utloggad användare
    }
}
