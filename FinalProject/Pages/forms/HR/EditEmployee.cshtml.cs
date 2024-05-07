using FinalProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using System.Data.SqlClient;

namespace FinalProject.Pages.forms.HR
{
    public class EditEmployeeModel : PageModel
    {

        [BindProperty]
        public Employee employee { get; set; } = new Employee();
        public static SqlConnection con = Configuration.getInstance().getConnection();
         public void  OnGet(int id)
        {
            employee.EmployeeId = id;
            SqlCommand cmd = new SqlCommand(@"SELECT EmployeeID, FirstName, LastName, CNIC, PrimaryPhone, AlternatePhone,Address,Email
                FROM Employee E
                JOIN Person P ON P.PersonID = E.EmployeeID
                JOIN Lookup l ON E.DesignationID = l.lookupID
                WHERE E.EmployeeID = @EmployeeId", con);

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

                SqlCommand cmd = new SqlCommand(@"UPDATE Person
                                            SET FirstName = @FirstName, 
                                              LastName = @LastName, 
                                              CNIC = @CNIC, 
                                              PrimaryPhone = @PrimaryPhone, 
                                              AlternatePhone = @AlternatePhone, 
                                              Address = @Address, 
                                              Email = @Email
                                            WHERE PersonID = @EmployeeId", con);

                        // Set parameter values based on form inputs
                        cmd.Parameters.AddWithValue("@FirstName", employee.applicant.person.FirstName);
                cmd.Parameters.AddWithValue("@LastName", employee.applicant.person.LastName != null ? employee.applicant.person.LastName : DBNull.Value);
                cmd.Parameters.AddWithValue("@CNIC", employee.applicant.person.CNIC);
                cmd.Parameters.AddWithValue("@PrimaryPhone", employee.applicant.person.PrimaryPhone);

                cmd.Parameters.AddWithValue("@AlternatePhone", employee.applicant.person.AlternatePhone != null ? employee.applicant.person.AlternatePhone : DBNull.Value);
                cmd.Parameters.AddWithValue("@Address", employee.applicant.person.Address);
                cmd.Parameters.AddWithValue("@Email", employee.applicant.person.Email);
                cmd.Parameters.AddWithValue("@EmployeeId", id);

                // Execute the SQL command
                int rowsAffected = cmd.ExecuteNonQuery();
               
                // Check if any rows were affected by the update operation
                if (rowsAffected > 0)
                {
                    // If rows were affected, the update was successful
                    // Redirect to the ManageEmployees page
                    return RedirectToPage("/forms/HR/ManageEmployee");

                }
                else
                {
                    // If no rows were affected, indicate that the update failed
                    TempData["ErrorMessage"] = "Failed to update employee details.";
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            // If execution reaches here, an error occurred during the update operation
            // Return the current page to display the error message
            return Page();
        }

    }
}
