using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FinalProject.Pages.forms.InventoryManager
{
    public class manageInventoryModel : PageModel
    {
		public class InventoryItem
		{
			public string Name { get; set; }
			public string Description { get; set; }
			public double SalePrice { get; set; }
			public string MeasurementUnit { get; set; }
		}

		// Temporary data for demonstration
		public List<InventoryItem> InventoryItems { get; set; } = new List<InventoryItem>
		{
			new InventoryItem { Name = "Item 1", Description = "Description 1", SalePrice = 10.99, MeasurementUnit = "Unit 1" },
			new InventoryItem { Name = "Item 2", Description = "Description 2", SalePrice = 20.99, MeasurementUnit = "Unit 2" },
			new InventoryItem { Name = "Item 3", Description = "Description 3", SalePrice = 30.99, MeasurementUnit = "Unit 3" }
		};

		public void OnGet()
		{
			// Here you can perform any initialization logic
		}
	}
}
