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
        [BindProperty]
        public static List<int> ids { get; set; } = new List<int>();
        [BindProperty]
        public Item item { get; set; } = new Item();
        private static SqlConnection con = Configuration.getInstance().getConnection();
        static int stockId = 0;
        static int initialCount = 0;
        public void OnGet()
        {
            SqlCommand cmd = new SqlCommand(@"SELECT StockId From Stock", con);
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                ids.Clear();
                while (reader.Read())
                {
                    int StockId = reader.GetInt32(0);

                    ids.Add(StockId);

                }

            }
            ids.Distinct();
        }

        public void OnPost()
        {
            try
            {

                stockId = int.Parse(Request.Form["stockIdDropDown"]);
                SqlCommand cmd = new SqlCommand(@"SELECT ItemName, Description, SalePrice, CostPrice, InitialQuantity, MeasurementUnit
                From Stock S 
                JOIN Item I ON S.ItemID = I.ItemID 
                WHERE StockId = @StockId", con);
                cmd.Parameters.AddWithValue("@StockId", stockId);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    items.Clear();
                    while (reader.Read())
                    {
                        Item item = new Item();
                        item.ItemName = reader.GetString(0);
                        if (!reader.IsDBNull(1))
                        {
                            item.Description = reader.GetString(1);
                        }
                        item.SalePrice = Convert.ToDouble(reader.GetDecimal(2));
                        item.CostPrice = (float)reader.GetDecimal(3);
                        item.CurrentQuantity = reader.GetInt32(4);
                        item.MeasurementUnit = reader.GetString(5);

                        items.Add(item);

                    }
                    initialCount = items.Count();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }


        public IActionResult OnPostAddStock()
        {
            if (items.Count > 0)
            {
                using (SqlTransaction transaction = con.BeginTransaction())
                {
                    try
                    {
                        items.RemoveRange(0, initialCount);

                        int StockId = stockId;

                        foreach (var item in items)
                        {
                            SqlCommand check = new SqlCommand(@"SELECT COUNT(*) FROM Item I WHERE I.ItemName Like @ItemName", con, transaction);
                            check.Parameters.AddWithValue("@ItemName", item.ItemName);
                            int count = (int)check.ExecuteScalar();
                            SqlCommand cmd;
                            if (count > 0)
                            {
                                SqlCommand getId = new SqlCommand(@"SELECT ItemID FROM Item I WHERE I.ItemName LIKE @ItemName", con, transaction);
                                getId.Parameters.AddWithValue("@ItemName", item.ItemName);
                                int itemId = (int)getId.ExecuteScalar();

                                cmd = new SqlCommand(@"UPDATE Item 
                            SET AvailableQuantity = AvailableQuantity + @AvailableQuantity, SalePrice = @SalePrice
                            WHERE ItemID = @ItemID;
                            INSERT INTO Stock
                            VALUES (@StockID, @ItemID, null, null, @ArrivalDate, @CostPrice, @CurrentStockQuantity, @InitialQuantity)", con, transaction);
                                cmd.Parameters.AddWithValue("@ItemID", itemId);
                            }
                            else
                            {
                                cmd = new SqlCommand(@"INSERT INTO Item 
                            Values (@ItemName, @AvailableQuantity, @Description, @MeasurementUnit, @SalePrice, null, null)
                            INSERT INTO Stock
                            VALUES (@StockID, (SELECT MAX(ItemID) FROM Item), null, null, @ArrivalDate, @CostPrice, @CurrentStockQuantity, @InitialQuantity)", con, transaction);
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

                        // Commit the transaction
                        transaction.Commit();

                        items.Clear();
                        return RedirectToPage("/forms/InventoryManager/manageInventory");
                    }
                    catch (Exception ex)
                    {
                        // Rollback the transaction if an exception occurs
                        transaction.Rollback();
                        // Log or handle the exception
                        Console.WriteLine("Exception occurred: " + ex.Message);
                        return RedirectToPage("/Error");
                    }
                }
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

            if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(unit) && cost != 0 && quantity != 0 && sale != 0 && arrival != DateTime.MinValue)
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
            SqlCommand getItemId = new SqlCommand(@"SELECT S.ItemID 
                FROM Stock S 
                JOIN Item I On I.ItemID = S.ItemID 
                WHERE StockId = @StockId AND ItemName = @ItemName", con);
            getItemId.Parameters.AddWithValue("@StockId", stockId);
            getItemId.Parameters.AddWithValue("@ItemName", Name);
            int itemId = (int)getItemId.ExecuteScalar();


            SqlCommand cmd = new SqlCommand(@"DELETE 
                From Stock 
                WHERE ItemID = @ItemID AND StockId = @StockId", con);
            cmd.Parameters.AddWithValue("@StockId", stockId);
            cmd.Parameters.AddWithValue("@ItemID", itemId);
            cmd.ExecuteNonQuery();

            foreach (var i in items)
            {
                if (i.ItemName == Name)
                {
                    items.Remove(i);
                    break;
                }
            }
        }
    }
}
