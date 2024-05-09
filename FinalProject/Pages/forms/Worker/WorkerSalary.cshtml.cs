using FinalProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace FinalProject.Pages.forms.Worker
{
    public class WorkerSalaryModel : PageModel
    {
        public List<Employee> employees = new List<Employee>();
        private static SqlConnection con = Configuration.getInstance().getConnection();
        public void OnGet()
        {
            try
            {
                SqlCommand cmd = new SqlCommand("SELECT CAST(PaidAmount AS float), PaidDate, isPaid " +
                    "FROM ViewSalary " +
                    "WHERE EmployeeID = @EmployeeID ", con);
                cmd.Parameters.AddWithValue("@EmployeeID", IndexModel.employeeId);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while(reader.Read())
                    {
                        Employee e = new Employee();
                        e.Salary = reader.GetDouble(0);
                        e.PaidDate = reader.GetDateTime(1);
                        e.IsPaid = reader.GetBoolean(2);

                        employees.Add(e);
                    }
                }

			}
            catch (Exception e) 
            {
                TempData["ErrorMessage"] = e.Message;

            }
        }


    }
}
