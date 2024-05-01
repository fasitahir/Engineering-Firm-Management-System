using FinalProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FinalProject.Pages.forms
{
    public class ApplyNowModel : PageModel
    {
        [BindProperty]
        public Person person { get; set; }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                string g = person.Gender;
                DateOnly d = person.DateOfBirth;
                string f = person.FirstName;

                return Page();
            }

            // Logic to handle form submission
            // Redirect or process the form data

            return RedirectToPage("/Index"); // Redirect to a different page after successful form submission
        }
    }
}
