using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RazorSideBar.Pages;

public class IndexModel : PageModel
{
    public bool isOpen { get; set;} = true;
    public void OnGet() { }

    public string setDisplay()
    {
        isOpen = !isOpen;
        Console.WriteLine("example");
        return "success";
    }
}
