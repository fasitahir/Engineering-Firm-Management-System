using FinalProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace FinalProject.Pages.forms.Accountant
{
    public class OrdersModel : PageModel
    {
		[BindProperty]
		public Quotation quotation { get; set; } = new Quotation();
		[BindProperty]
		public List<Driver> drivers { get; set; } = new List<Driver>();
		public List<Quotation> quotations { get; set; } = new List<Quotation>();
		[BindProperty]
		public List<Vehicle> vehicles { get; set; } = new List<Vehicle>();
		[BindProperty]
		public string driverName { get; set; }
		[BindProperty]
		public string driverCnic { get; set; }
		[BindProperty]
		public string licenseNumber { get; set; }
		[BindProperty]
		public DateTime expiryDate { get; set; }
		[BindProperty]
		public string vehicleType { get; set; }
		[BindProperty]
		public string plateNumber { get; set; }
		[BindProperty]
		public string cnicDriver { get; set; }
		[BindProperty]
		public int quotationId { get; set; }
		[BindProperty]
		public string dCnic { get; set; }
		[BindProperty]
		public string dVehicle { get; set; }

        private static SqlConnection con = Configuration.getInstance().getConnection();

        public void OnGet()
		{
			try
			{
				SqlCommand cmd = new SqlCommand(@"Select DriverName, DriverCNIC, LicenseNumber, LicenseExpiryDate From Driver", con);

				using (SqlDataReader reader = cmd.ExecuteReader())
				{
					while (reader.Read())
					{
						Driver driver = new Driver();
						driver.driverName = reader.GetString(0);
						driver.driverCnic = reader.GetString(1); // Assuming driverCnic is a string in your class
						driver.licenseNumber = reader.GetString(2);
						driver.licenseExpiryDate = reader.GetDateTime(3);

						drivers.Add(driver);
					}
				}

				SqlCommand cmd_2 = new SqlCommand(@"Select q.QuotationID, q.ItemID,i.ItemName,c.clientID ,c.ClientName, q.ItemQuantity,q.TotalAmount,q.Discount,q.PayableAmount  
				From Quotation q 
				Join Client c On c.ClientID = q.ClientID 
				Join Item i On i.ItemID = q.ItemID;", con);
				using (SqlDataReader reader_2 = cmd_2.ExecuteReader())
				{
					while (reader_2.Read())
					{
						Quotation quotation = new Quotation();
						quotation.quotationID = reader_2.GetInt32(0);
						quotation.itemID = reader_2.GetInt32(1);
						quotation.itemName = reader_2.GetString(2);
						quotation.clientID = reader_2.GetInt32(3);
						quotation.clientName = reader_2.GetString(4);
						quotation.itemQuantity = reader_2.GetInt32(5);
						quotation.totalAmount = reader_2.GetDecimal(6);
						quotation.discount = reader_2.GetDecimal(7);
						quotation.payableAmount = reader_2.GetDecimal(8);

						quotations.Add(quotation);
					}
				}
				
				SqlCommand cmd_3 = new SqlCommand(@"Select VehicleID, d.DriverID, d.DriverCNIC,VehicleType,l.Value,PlateNumber 
				From Vehicle V 
				JOIN Driver d ON d.DriverID = V.DriverID 
				Join Lookup l On l.lookupID = VehicleType;", con);
				using (SqlDataReader reader_3 = cmd_3.ExecuteReader())
				{
					while (reader_3.Read())
					{
						Vehicle vehicle = new Vehicle();
						vehicle.vehicleId = reader_3.GetInt32(0);
						vehicle.driver.driverId = reader_3.GetInt32(1);
						vehicle.driver.driverCnic = reader_3.GetString(2);
						vehicle.vehicleType = reader_3.GetInt32(3);
						vehicle.vehicleTypeName = reader_3.GetString(4);
						vehicle.plateNumber = reader_3.GetString(5);
						vehicles.Add(vehicle);
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
		public IActionResult OnPostAddDriver()
		{
			
			SqlCommand cmd = new SqlCommand(@"Insert Into Driver Values (@DriverName, @DriverCnic, @LicenseNumber,Null, Null, @ExpiryDate)", con);

			cmd.Parameters.AddWithValue("@DriverName", driverName);
			cmd.Parameters.AddWithValue("@DriverCnic", driverCnic);
			cmd.Parameters.AddWithValue("@LicenseNumber", licenseNumber);
			cmd.Parameters.AddWithValue("@ExpiryDate", expiryDate);

			cmd.ExecuteNonQuery();
			return Page();
		}

		public IActionResult OnPostAddVehicle()
		{
			int vehicleTypeValue;
			if (vehicleType == "Truck")
			{
				vehicleTypeValue = 12;
			}
			else if (vehicleType == "Van")
			{
				vehicleTypeValue = 13;
			}
			else if (vehicleType == "Rickshaw")
			{
				vehicleTypeValue = 14;
			}
			else if (vehicleType == "PickUp")
			{
				vehicleTypeValue = 15;
			}
			else
			{
				vehicleTypeValue = 16;
			}
			var con = Configuration.getInstance().getConnection();
			SqlCommand cmd = new SqlCommand(@"Insert Into Vehicle (DriverID, VehicleType, PlateNumber) values ((Select DriverID From Driver Where DriverCNIC = @DriverCnic),@VehicleType, @PlateNumber)", con);

			cmd.Parameters.AddWithValue("@DriverCnic", cnicDriver);
			cmd.Parameters.AddWithValue("@VehicleType", vehicleTypeValue);
			cmd.Parameters.AddWithValue("@PlateNumber", plateNumber);
			cmd.ExecuteNonQuery();
			return Page();
		}
		public IActionResult OnPost()
		{
			SqlCommand cmd = new SqlCommand(@"Insert Into [Order] (QuotationID, ItemID, ClientID, UpdatedBy, UpdatedOn, RequiredDate, OrderDate, IsServed) Values (@QuotationID, @ItemID, @ClientID, Null, Null, GetDate(), GetDate(), 0); 
				Insert Into Delivery Values (@VehicleID,1,(Select DriverID From Driver Where DriverCNIC = @DriverCnic), @ClientID, 17, Null, Null, Null, Null);			", con);

			cmd.Parameters.AddWithValue("@QuotationID", quotation.quotationID);
			cmd.Parameters.AddWithValue("@ItemID", quotation.itemID);
			cmd.Parameters.AddWithValue("@VehicleID", dVehicle);
			cmd.Parameters.AddWithValue("@ClientID", quotation.clientID);
			cmd.Parameters.AddWithValue("@DriverCnic", dCnic);


			cmd.ExecuteNonQuery();
			return RedirectToPage();
		}
	}
}

