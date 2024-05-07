using FinalProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.NetworkInformation;

namespace FinalProject.Pages.forms.HR
{
    public class ManageEmployeeModel : PageModel
    {
        public string SelectedText { get; set; } = "View All Employees";
        [BindProperty]
        public List<Employee> employees { get; set; } = new List<Employee>();
        public static SqlConnection con = Configuration.getInstance().getConnection();
        
        public void OnGet(string status)
        {
            if (string.IsNullOrEmpty(status) || status == "all")
            {
                GetAllEmployees();
                SelectedText = "View All Employees";
            }
            else
            {
                OnGetFilter(status);
            }
        }


        public void OnGetFilter(string status)
        {
            // Filter applicants based on status
            GetFilteredEmployees(status);
            SelectedText = status switch
            {
                "HR" => "View HR",
                "Worker" => "View Worker",
                "Accountant" => "View Accountant",
                "Invenotory" => "View Inventory Manager",
                "Past" => "View Past Employees",
                "All" => "View all Employees",
                _ => "View all Employees"
            };
        }

        private void GetAllEmployees() 
        {
            try
            {
                SqlCommand cmd = new SqlCommand(@"SELECT EmployeeID, EmployeeNumber, FirstName + ' ' + ISNULL(LastName, ' ') As FullName, CNIC, PrimaryPhone, Email, l.Value AS Designation, IsActive
                FROM Employee E
                JOIN Person P ON P.PersonID = E.EmployeeID
                JOIN Lookup l ON E.DesignationID = l.lookupID 
                WHERE IsActive = 1", con);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Employee e = new Employee();
                        e.EmployeeId = reader.GetInt32(0);
                        e.EmployeeNo = reader.GetString(1);
                        e.applicant.person.FirstName = reader.GetString(2);
                        e.applicant.person.CNIC = reader.GetString(3);
                        e.applicant.person.PrimaryPhone = reader.GetString(4);
                        e.applicant.person.Email = reader.GetString(5);
                        e.DesignationName = reader.GetString(6);
                        e.IsActive = reader.GetBoolean(7);
                        employees.Add(e);
                    }
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private void GetFilteredEmployees(string status)
        {
            try
            {

                SqlCommand cmd;
                if (status == "HR")
                {
                    cmd = new SqlCommand(@"SELECT EmployeeID, EmployeeNumber, FirstName + ' ' + ISNULL(LastName, ' ') As FullName, CNIC, PrimaryPhone, Email, l.Value AS Designation, IsActive
                        FROM Employee E
                        JOIN Person P ON P.PersonID = E.EmployeeID
                        JOIN Lookup l ON E.DesignationID = l.lookupID
                        WHERE IsActive = 1 AND l.lookupID = 5", con);

                }
                else if (status == "Worker")
                {
                    cmd = new SqlCommand(@"SELECT EmployeeID, EmployeeNumber, FirstName + ' ' + ISNULL(LastName, ' ') As FullName, CNIC, PrimaryPhone, Email, l.Value AS Designation, IsActive
                        FROM Employee E
                        JOIN Person P ON P.PersonID = E.EmployeeID
                        JOIN Lookup l ON E.DesignationID = l.lookupID
                        WHERE IsActive = 1 AND l.lookupID = 3", con);
                }
                else if(status == "Accountant")
                {
                    //All selected
                    cmd = new SqlCommand(@"SELECT EmployeeID, EmployeeNumber, FirstName + ' ' + ISNULL(LastName, ' ') As FullName, CNIC, PrimaryPhone, Email, l.Value AS Designation, IsActive
                        FROM Employee E
                        JOIN Person P ON P.PersonID = E.EmployeeID
                        JOIN Lookup l ON E.DesignationID = l.lookupID
                        WHERE IsActive = 1 AND l.lookupID = 4", con);
                }

                else if (status == "Inventory")
                {
                    //All selected
                    cmd = new SqlCommand(@"SELECT EmployeeID, EmployeeNumber, FirstName + ' ' + ISNULL(LastName, ' ') As FullName, CNIC, PrimaryPhone, Email, l.Value AS Designation, IsActive
                        FROM Employee E
                        JOIN Person P ON P.PersonID = E.EmployeeID
                        JOIN Lookup l ON E.DesignationID = l.lookupID
                        WHERE IsActive = 1 AND l.lookupID = 6", con);
                }
                else
                {
                    //All selected
                    cmd = new SqlCommand(@"SELECT EmployeeID, EmployeeNumber, FirstName + ' ' + ISNULL(LastName, ' ') As FullName, CNIC, PrimaryPhone, Email, l.Value AS Designation, IsActive
                        FROM Employee E
                        JOIN Person P ON P.PersonID = E.EmployeeID
                        JOIN Lookup l ON E.DesignationID = l.lookupID
                        WHERE IsActive = 0", con);
                }

               
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Employee e = new Employee();
                        e.EmployeeId = reader.GetInt32(0);
                        e.EmployeeNo = reader.GetString(1);
                        e.applicant.person.FirstName = reader.GetString(2);
                        e.applicant.person.CNIC = reader.GetString(3);
                        e.applicant.person.PrimaryPhone = reader.GetString(4);
                        e.applicant.person.Email = reader.GetString(5);
                        e.DesignationName = reader.GetString(6);
                        e.IsActive = reader.GetBoolean(7);
                        employees.Add(e);
                    }
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }


        public async Task<IActionResult> OnPostDeleteEmployee(int employeeId)
        {
            try
            {


                SqlCommand cmd = new SqlCommand("UPDATE Employee SET IsActive = 0 WHERE EmployeeID = @EmployeeId", con);
                
                    cmd.Parameters.AddWithValue("@EmployeeId", employeeId);
                    await con.OpenAsync();

                    int rowsAffected = await cmd.ExecuteNonQueryAsync();

                    if (rowsAffected > 0)
                    {
                        using (var deleteCredentialsCmd = new SqlCommand("DELETE FROM Credentials WHERE EmployeeID = @EmployeeId", con))
                        {
                            deleteCredentialsCmd.Parameters.AddWithValue("@EmployeeId", employeeId);
                            await deleteCredentialsCmd.ExecuteNonQueryAsync();
                        }
                        return RedirectToPage(); // Return 200 OK response if deletion is successful
                    }
                    else
                    {
                        return new NotFoundResult(); // Return 404 Not Found if employee with the given ID does not exist
                    }
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return new StatusCodeResult(500); // Return 500 Internal Server Error for any other exceptions
            }
        }
    }
}
