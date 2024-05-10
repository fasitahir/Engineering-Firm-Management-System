using FinalProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace FinalProject.Pages.forms.Accountant
{
    public class EmployeesModel : PageModel
    {
        [BindProperty]
        public List<Employee> employees { get; set; } = new List<Employee>();
        [BindProperty]
        public double newSalary { get; set; } = 0;
        [BindProperty]
        public string backend_employeeID { get; set; }
        private static SqlConnection con = Configuration.getInstance().getConnection();
        public void OnGet()
        {
            try
            {
                
                SqlCommand cmd = new SqlCommand(@"
                    SELECT 
                    p.FirstName,
                    ISNULL(p.LastName,' '),
                    p.Email, 
                    e.StartDate AS Date, 
                    CAST(e.BaseSalary AS Float) AS BaseSalary,
                    CAST(ISNULL(SUM(s.PaidAmount), 0.00) AS Float) AS PaidAmount,
                    s.IsPaid,  -- Assuming 'IsPaid' field directly indicates paid status
                    e.EmployeeNumber  -- Include Employee Number
                    FROM Person p
                    INNER JOIN Employee e ON p.PersonID = e.EmployeeID
                    INNER JOIN Salary s ON e.EmployeeID = s.EmployeeID
                    GROUP BY p.PersonID, p.FirstName, p.LastName, p.Email, e.StartDate, e.BaseSalary, s.IsPaid, e.EmployeeNumber
                    ORDER BY p.LastName, p.FirstName;", con);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Employee e = new Employee();
                        e.FirstName = reader.GetString(0);
                        e.LastName = reader.GetString(1);
                        e.Email = reader.GetString(2);
                        e.StartDate = reader.GetDateTime(3);
                        e.BaseSalary = reader.GetDouble(4);
                        e.PaidAmount = reader.GetDouble(5);  // Assuming PaidAmount is a double in Employee
                        e.IsPaid = reader.GetBoolean(6);  // Assuming IsPaid is a boolean
                        e.EmployeeNo = reader.GetString(7);
                        employees.Add(e);
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

        public IActionResult OnPostEditSalary()
        {
            try
            {
                
                SqlCommand cmd = new SqlCommand(@"UPDATE Employee
                                 SET BaseSalary = @NewBaseSalary
                                 WHERE EmployeeNumber = @EmployeeNo;", con);
                cmd.Parameters.AddWithValue("@NewBaseSalary", newSalary);
                cmd.Parameters.AddWithValue("@EmployeeNo", backend_employeeID);

                cmd.ExecuteNonQuery();

                // Get the updated base salary from the database
                SqlCommand selectCmd = new SqlCommand(@"SELECT BaseSalary FROM Employee WHERE EmployeeNumber= @EmployeeNo;", con);
                selectCmd.Parameters.AddWithValue("@EmployeeNo", backend_employeeID);
                return Page();
            }
            catch (Exception e)
            {
                TempData["ErrorMessage"] = e.Message;
                throw e;
            }
        }
    }
}
