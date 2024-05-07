using FinalProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace FinalProject.Pages.forms.InventoryManager
{
    public class manageInventoryModel : PageModel
    {
		[BindProperty]
		public List<Item> items { get; set; } = new List<Item> ();
		private static SqlConnection con = Configuration.getInstance().getConnection();
        
		public void OnGet()
		{
			SqlCommand cmd = new SqlCommand(@"SELECT ItemName, Description, SalePrice, MeasurementUnit FROM Item", con);
			using (SqlDataReader reader =cmd.ExecuteReader())
			{
				while(reader.Read())
				{
					Item item = new Item ();
					item.ItemName = reader.GetString(0);
                    if (!reader.IsDBNull(1)) // Check if the value is not NULL
                    {
                        item.Description = reader.GetString(1); 
                    }
                    else
                    {
                        item.Description = string.Empty; 
                    }

                    item.SalePrice = Convert.ToDouble(reader.GetDecimal(2));
                    item.MeasurementUnit = reader.GetString(3);

					items.Add(item);
                }
            }
		}
	}
}
