using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using FinalProject.Models;

namespace FinalProject.Pages.forms.HR
{
    public class HRHomeModel : PageModel
    {
        public static SqlConnection con = Configuration.getInstance().getConnection();
        public string PictureName { get; set; }
        public string Name { get; set; }

        public void OnGet(int EmployeeID)
        {
            IndexModel.employeeId = EmployeeID;
            SqlCommand cmd = new SqlCommand(@"SELECT PictureName, FirstName + ' ' +ISNULL(LastName,'')
                  FROM Applicant A
                  JOIN Person P ON A.ApplicantID = P.PersonID
                  WHERE ApplicantID = @EmployeeID", con);
            cmd.Parameters.AddWithValue("@EmployeeID", EmployeeID);
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    PictureName = reader.GetString(0);
                    Name = reader.GetString(1);
                }

            }
            cmd.ExecuteNonQuery();
            if(!string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(PictureName))
            {
                ViewData[PictureName] = PictureName;
                ViewData[Name] = Name;
            }
            
        }
    }
}
