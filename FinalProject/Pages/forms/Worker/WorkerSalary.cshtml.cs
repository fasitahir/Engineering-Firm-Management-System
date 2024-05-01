using FinalProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace FinalProject.Pages.forms.Worker
{
    public class WorkerSalaryModel : PageModel
    {
        public List<Employee> employees = new List<Employee>();
        
        public void OnGet()
        {
            try
            {
				var con = Configuration.getInstance().getConnection();
                SqlCommand cmd = new SqlCommand("SELECT CAST(PaidAmount AS float), PaidDate, isPaid " +
                    "FROM Salary S " +
                    "WHERE S.EmployeeID = @EmployeeID ", con);
                cmd.Parameters.AddWithValue("@EmployeeID", 1);
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
