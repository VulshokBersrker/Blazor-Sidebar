using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RazorSideBar.Pages;

public class SideBarModel : PageModel
{
    public bool isOpen { get; set; } = true;

    public void OnGet()
    {
        isOpen = !isOpen;
    }
    
}
