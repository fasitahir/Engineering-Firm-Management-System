using FinalProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace FinalProject.Pages.forms.HR
{
    public class EditEmployeeModel : PageModel
    {

        [BindProperty]
        public Employee employee { get; set; } = new Employee();
        public static SqlConnection con = Configuration.getInstance().getConnection();
        public void OnGet(int id)
        {
            employee.EmployeeId = id;
            SqlCommand cmd = new SqlCommand(@"SELECT EmployeeID, FirstName, LastName, CNIC, PrimaryPhone, AlternatePhone,Address,Email
                FROM ViewEmployee
                WHERE EmployeeID = @EmployeeId", con);

            cmd.Parameters.AddWithValue("@EmployeeId", id);

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    employee.EmployeeId = reader.GetInt32(0);

                    // Populating the Person properties
                    employee.applicant.person.FirstName = reader.GetString(1);
                    employee.applicant.person.LastName = reader.IsDBNull(2) ? null : reader.GetString(2);
                    employee.applicant.person.CNIC = reader.GetString(3);
                    employee.applicant.person.PrimaryPhone = reader.GetString(4);
                    employee.applicant.person.AlternatePhone = reader.IsDBNull(5) ? null : reader.GetString(5);
                    employee.applicant.person.Address = reader.GetString(6);
                    employee.applicant.person.Email = reader.GetString(7);

                }
            }


        }

        public IActionResult OnPost(int id)
        {
            // Check if the model state is valid

            try
            {

                SqlCommand cmd = new SqlCommand(@"stp_UpdateEmployeePersonalInfo", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                // Set parameter values based on form inputs
                cmd.Parameters.AddWithValue("@FirstName", employee.applicant.person.FirstName);
                cmd.Parameters.AddWithValue("@LastName", employee.applicant.person.LastName != null ? employee.applicant.person.LastName : DBNull.Value);
                cmd.Parameters.AddWithValue("@CNIC", employee.applicant.person.CNIC);
                cmd.Parameters.AddWithValue("@PrimaryPhone", employee.applicant.person.PrimaryPhone);
                cmd.Parameters.AddWithValue("@IsActive", 1);
                cmd.Parameters.AddWithValue("@AlternatePhone", employee.applicant.person.AlternatePhone != null ? employee.applicant.person.AlternatePhone : DBNull.Value);
                cmd.Parameters.AddWithValue("@Address", employee.applicant.person.Address);
                cmd.Parameters.AddWithValue("@Email", employee.applicant.person.Email);
                cmd.Parameters.AddWithValue("@EmployeeId", id);

                cmd.ExecuteNonQuery();


                return RedirectToPage("/forms/HR/ManageEmployee");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            
            return Page();
        }

    }
}
