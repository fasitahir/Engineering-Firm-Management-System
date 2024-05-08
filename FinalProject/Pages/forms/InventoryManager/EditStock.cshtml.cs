using FinalProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace FinalProject.Pages.forms.InventoryManager
{
    public class EditStockModel : PageModel
    {
        [BindProperty]
        public static List<Item> items { get; set; } = new List<Item>();
        public static List<int> ids { get; set; } = new List<int>();
        private static SqlConnection con = Configuration.getInstance().getConnection();
        public void OnGet()
        {
            SqlCommand cmd = new SqlCommand(@"SELECT StockId From Stock", con);
            using(SqlDataReader reader = cmd.ExecuteReader())
            {
                ids.Clear();
                while(reader.Read())
                {
                    int StockId = reader.GetInt32(0);

                    ids.Add(StockId);

                }

            }
        }

        public void OnPost() 
        {
            int stockId = int.Parse(Request.Form["stockIdDropDown"]);

        }
    }
}
