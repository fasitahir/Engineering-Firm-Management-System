using FinalProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FinalProject.Pages.forms.InventoryManager
{
    public class IM_ManageStockModel : PageModel
    {
        public Item item { get; set; }
        public Stock stock { get; set; }
        public void OnGet()
        {
        }
    }
}
