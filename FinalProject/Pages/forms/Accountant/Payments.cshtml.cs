using FinalProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace FinalProject.Pages.forms.Accountant
{
    public class PaymentsModel : PageModel
    {
        [BindProperty]
        public List<Employee> employees { get; set; } = new List<Employee>();
        private static SqlConnection con = Configuration.getInstance().getConnection();
        [BindProperty]
        public List<Employee> employees_unpaid { get; set; } = new List<Employee>();
        [BindProperty]
        public decimal amount { get; set; }
        [BindProperty]
        public string employeeNoExtract { get; set; }
        public void OnGet()
        {
            try
            {

                SqlCommand cmd = new SqlCommand(@"
						SELECT 
						p.FirstName,
						ISNULL(p.LastName,' '),
						p.Email, 
						s.PaidDate AS PaidDate, 
						CAST(e.BaseSalary AS Float) AS BaseSalary,
						CAST(ISNULL(SUM(s.PaidAmount), 0.00) AS Float) AS PaidAmount,
						s.IsPaid,  -- Assuming 'IsPaid' field directly indicates paid status
						e.EmployeeNumber  -- Include Employee Number
					FROM Person p
					INNER JOIN Employee e ON p.PersonID = e.EmployeeID
					INNER JOIN Salary s ON e.EmployeeID = s.EmployeeID
					GROUP BY p.PersonID, p.FirstName, p.LastName, p.Email, s.PaidDate, e.BaseSalary, s.IsPaid, e.EmployeeNumber
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


                SqlCommand cmd_2 = new SqlCommand(@"Select EmployeeNumber From Employee Join Salary On Salary.EmployeeID = Employee.EmployeeID Where Salary.IsPaid = 0 ", con);

                using (SqlDataReader reader_2 = cmd_2.ExecuteReader())
                {
                    while (reader_2.Read())
                    {
                        Employee e = new Employee();
                        e.EmployeeNo = reader_2.GetString(0);
                        employees_unpaid.Add(e);
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
        public IActionResult OnPost()
        {
            SqlCommand getSalary = new SqlCommand(@"SELECT BaseSalary FROM Employee WHERE EmployeeNumber = @EmployeeNumber", con);
            getSalary.Parameters.AddWithValue("@EmployeeNumber", employeeNoExtract);
            decimal salary = (decimal)getSalary.ExecuteScalar();
            SqlCommand cmd = new SqlCommand(@"Insert Into Salary (EmployeeID, PaidDate, IsPaid, PaidAmount, Salary) values((Select e.EmployeeID From Employee e Where e.EmployeeNumber = @EmployeeNumber), GetDate(), 1, @PaidAmount, @Salary)", con);

            cmd.Parameters.AddWithValue("@EmployeeNumber", employeeNoExtract);
            cmd.Parameters.AddWithValue("@PaidAmount", amount);
            cmd.Parameters.AddWithValue("@Salary", salary);


            cmd.ExecuteNonQuery();
            return Page();
        }
    }
}
