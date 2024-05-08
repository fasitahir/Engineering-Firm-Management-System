using FinalProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace FinalProject.Pages.forms.InventoryManager
{
    public class addStockModel : PageModel
    {
        private static SqlConnection con = Configuration.getInstance().getConnection();

        [BindProperty]
        public static List<Item> items { get; set; } = new List<Item>();
        [BindProperty]
        public Item item { get; set; } = new Item();
        public void OnGet()
        {
        }

        public IActionResult OnPostAddStock() 
        {
            if(items.Count > 0)
            {
                SqlCommand getStockId = new SqlCommand(@"SELECT ISNULL(MAX(StockID),0)+1 FROM Stock", con);
                int StockId = (int)getStockId.ExecuteScalar();

                foreach (var item in items)
                {

                    SqlCommand check = new SqlCommand(@"SELECT COUNT(*) FROM Item I WHERE I.ItemName Like @ItemName", con);
                    check.Parameters.AddWithValue("@ItemName", item.ItemName);
                    int count = (int)check.ExecuteScalar();
                    SqlCommand cmd;
                    if(count > 0 )
                    {
                        SqlCommand getId = new SqlCommand(@"SELECT ItemID FROM Item I WHERE I.ItemName LIKE @ItemName", con);
                        getId.Parameters.AddWithValue("@ItemName", item.ItemName);
                        int itemId = (int)getId.ExecuteScalar();

                        cmd = new SqlCommand(@"UPDATE Item 
                            SET AvailableQuantity = AvailableQuantity + @AvailableQuantity, SalePrice = @SalePrice
                            WHERE ItemID = @ItemID;
                            INSERT INTO Stock
                            VALUES (@StockID, @ItemID, null, null, @ArrivalDate, @CostPrice, @CurrentStockQuantity, @InitialQuantity)
                            ", con);
                        cmd.Parameters.AddWithValue("@ItemID", itemId);
                    }
                    else
                    {
                        cmd = new SqlCommand(@"INSERT INTO Item 
                            Values (@ItemName, @AvailableQuantity, @Description, @MeasurementUnit, @SalePrice, null, null)
                            INSERT INTO Stock
                            VALUES (@StockID, (SELECT MAX(ItemID) FROM Item), null, null, @ArrivalDate, @CostPrice, @CurrentStockQuantity, @InitialQuantity)
                            ", con);
                    }

                    cmd.Parameters.AddWithValue("@ItemName", item.ItemName);
                    cmd.Parameters.AddWithValue("@AvailableQuantity", item.CurrentQuantity);
                    cmd.Parameters.AddWithValue("@StockID", StockId);
                    if (string.IsNullOrEmpty(item.Description))
                    {
                        cmd.Parameters.AddWithValue("@Description", DBNull.Value);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@Description", item.Description);
                    }
                    cmd.Parameters.AddWithValue("@MeasurementUnit", item.MeasurementUnit);
                    cmd.Parameters.AddWithValue("@SalePrice", item.SalePrice);
                    cmd.Parameters.AddWithValue("@CostPrice", item.CostPrice);
                    cmd.Parameters.AddWithValue("@ArrivalDate", item.ArrivalDate);
                    cmd.Parameters.AddWithValue("@CurrentStockQuantity", item.CurrentQuantity);
                    cmd.Parameters.AddWithValue("@InitialQuantity", item.CurrentQuantity);
                    cmd.ExecuteNonQuery();

                }
                items.Clear();
                return RedirectToPage("/forms/InventoryManager/manageInventory");


            }
            else
            {
                return RedirectToPage();
            }
            
        }


        public void OnPostAddItem() 
        {
            string name = item.ItemName;
            string description = item.Description;
            string unit = item.MeasurementUnit;
            double cost = item.CostPrice;
            int quantity = item.CurrentQuantity;
            double sale = item.SalePrice;
            DateTime arrival = item.ArrivalDate;
            
            if(!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(unit) && cost != 0 && quantity != 0 && sale!=0 && arrival!= DateTime.MinValue)
            {
                items.Add(item);
            }
            else
            {
                Console.WriteLine("Please Fill the required boxes");
            }
        
        }


        public void OnPostItemRemove(string Name)
        {
            
            foreach (var i in items)
            {
                if(i.ItemName == Name)
                {
                    items.Remove(i);
                    break;
                }
            }
        }
    }
}
