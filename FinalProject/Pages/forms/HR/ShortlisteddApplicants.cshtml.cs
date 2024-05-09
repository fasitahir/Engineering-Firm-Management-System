using FinalProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Net.NetworkInformation;

namespace FinalProject.Pages.forms.HR
{
    public class ShortlisteddApplicantsModel : PageModel
    {

        public List<Applicant> applicants = new List<Applicant>();

        public static SqlConnection con = Configuration.getInstance().getConnection();

        public void OnGet()
        {
            try
            {
                SqlCommand cmd = new SqlCommand(@"SELECT ApplicantID, FirstName,Email, PrimaryPhone, LastName 
                    FROM ViewApplicants
                    where isRejected = 0 and isSelected = 0 and isShortlisted = 1 ", con);


                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Applicant a = new Applicant();
                        a.applicantID = reader.GetInt32(0);
                        a.person.FirstName = reader.GetString(1);
                        a.person.Email = reader.GetString(2);
                        a.person.PrimaryPhone = reader.GetString(3);
                        if(!reader.IsDBNull(4))
                        {
                            a.person.LastName = reader.GetString(4);

                        }
                        else
                        {
                            a.person.LastName = "";
                        }



                        applicants.Add(a);
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