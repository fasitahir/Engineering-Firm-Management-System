using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FinalProject.Models;
using System.Data.SqlClient;

namespace FinalProject.Pages.forms.Accountant
{
    public class QuotationModel : PageModel
    {
        [BindProperty]
        public List<Item> items { get; set; } = new List<Item>();
        [BindProperty]
        public static List<Item> addedItems { get; set; } = new List<Item>();
        [BindProperty]
        public List<Client> clients { get; set; } = new List<Client>();

        public Item item { get; set; } = new Item();
        [BindProperty]
        public string ItemNameExtract {  get; set; }

        public string clientName;
        public string clientCity;
        public string clientEmail;
        public string clientPhone;
        private static SqlConnection con = Configuration.getInstance().getConnection();
        public void OnGet()
        {
            try
            {
                
                SqlCommand cmd = new SqlCommand(@"Select ItemName, AvailableQuantity, [Description], MeasurementUnit, SalePrice  From Item", con);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Item item = new Item();
                        item.ItemName = reader.GetString(0);
                        item.AvailableQuantity = reader.GetInt32(1);
                        item.Description = reader.GetString(2);
                        item.MeasurementUnit = reader.GetString(3);
                        item.SalePrice = Convert.ToDouble(reader.GetDecimal(4));

                        items.Add(item);
                    }
                }
                
                SqlCommand cmd_2 = new SqlCommand(@"SELECT CONCAT(ClientName, ' ', City) AS ClientDetails FROM Client;", con);
                using (SqlDataReader reader_2 = cmd_2.ExecuteReader())
                {
                    while (reader_2.Read())
                    {
                       Client client = new Client(); 
                        client.clientDetail = reader_2.GetString(0);                     
                        clients.Add(client);
                    }
                }
            }
            catch (Exception ex)
            {
                // Improved error handling
                Console.WriteLine(ex.Message);  // Log the error for debugging
                TempData["ErrorMessage"] = "An error occurred while retrieving employee data.";  // Set a user-friendly error message
            }
        }
        public IActionResult OnPostAddClient()
        {
            SqlCommand cmd = new SqlCommand(@"INSERT INTO Client (ClientName,City, ClientEmail, PhoneNumber)
                                    VALUES (@ClientName, @City, @ClientEmail, @PhoneNumber)", con);
            string newName = clientName;
            string newEmail = clientEmail;
            // Set parameter values
            cmd.Parameters.AddWithValue("@ClientName",clientName );
            cmd.Parameters.AddWithValue("@City",clientCity );
            cmd.Parameters.AddWithValue("@ClientEmail",clientEmail);
            cmd.Parameters.AddWithValue("@PhoneNumber",clientPhone);

            cmd.ExecuteNonQuery();


            return Page();
        }

        public IActionResult OnPostAddItem()
        {
            string value = ItemNameExtract;
            SqlCommand cmd = new SqlCommand(@"SELECT ItemName, Description, MeasurementUnit, SalePrice, AvailableQuantity
                    From Item I
                    WHERE ItemName LIKE @ItemName", con);
            cmd.Parameters.AddWithValue("@ItemName", value);
            using(SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read()) 
                {
                    item.ItemName = reader.GetString(0);
                    item.Description = reader.GetString(1);
                    item.MeasurementUnit = reader.GetString(2);
                    item.SalePrice = Convert.ToDouble(reader.GetDecimal(3));
                    item.AvailableQuantity = reader.GetInt32(4);




                    string name = item.ItemName;
                    string? description = item.Description;
                    string unit = item.MeasurementUnit;
                    double cost = item.CostPrice;
                    int? quantity = item.AvailableQuantity;
                    double sale = item.SalePrice;
                    DateTime arrival = item.ArrivalDate;

                    if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(unit) && quantity != 0 && sale != 0)
                    {
                        addedItems.Add(item);
                    }
                    else
                    {
                        Console.WriteLine("Please Fill the required boxes");
                    }
                }
            }
            
            
            return RedirectToPage();
        }

        public IActionResult OnPostItemRemove(string Name)
        {

            foreach (var i in addedItems)
            {
                if (i.ItemName == Name)
                {
                    addedItems.Remove(i);
                    break;
                }
            }
            return RedirectToPage();
        }
    }
}
