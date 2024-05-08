using FinalProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Hosting;
using System.Data.SqlClient;

namespace FinalProject.Pages.forms.InventoryManager
{
    public class manageInventoryModel : PageModel
    {
		[BindProperty]
		public List<Item> items { get; set; } = new List<Item> ();
        [BindProperty]
        public Item item { get; set; } = new Item();
		private static SqlConnection con = Configuration.getInstance().getConnection();
        public int id { get; set; }
		public void OnGet()
		{
			SqlCommand cmd = new SqlCommand(@"SELECT ItemName, Description, SalePrice, MeasurementUnit, ItemID FROM Item", con);
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
                    item.ItemID = reader.GetInt32(4);
					items.Add(item);
                }
            }
		}
        
        public IActionResult OnPost()
        {
            string name = item.ItemName;
            string? description = item.Description;
            string unit = item.MeasurementUnit;
            double sale = item.SalePrice;
            int itemID = item.ItemID;
            if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(unit) && sale != 0 )
            {
                try
                {
                    SqlCommand cmd = new SqlCommand(@"stp_UpdateItem", con);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ItemName", item.ItemName);
                    cmd.Parameters.AddWithValue("@MeasurementUnit", item.MeasurementUnit);
                    cmd.Parameters.AddWithValue("@SalePrice", item.SalePrice);
                    cmd.Parameters.AddWithValue("@ItemID", item.ItemID);
                    if (string.IsNullOrEmpty(item.Description))
                    {
                        cmd.Parameters.AddWithValue("@Description", DBNull.Value);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@Description", item.Description);
                    }
                    cmd.Parameters.AddWithValue("@UpdatedBy", DBNull.Value);
                    cmd.Parameters.AddWithValue("@UpdatedOn", DBNull.Value);
                    cmd.ExecuteNonQuery();
                }
                catch(Exception ex) 
                {
                    Console.WriteLine(ex.Message);
                }
                
            }
            else
            {
                Console.WriteLine("Failed to update! Fill all required input fields");
            }
            return RedirectToPage();
        }
	}
}
