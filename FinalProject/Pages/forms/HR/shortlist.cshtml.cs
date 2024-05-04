using FinalProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System;

namespace FinalProject.Pages.forms.HR
{
    public class shortlistModel : PageModel
    {
        public string imagePath;
        public List<Applicant> applicants = new List<Applicant>();
       
              
        public void OnGet()
        {

            try
            {
                var con = Configuration.getInstance().getConnection();
                SqlCommand cmd = new SqlCommand(@"select ApplicantID, FirstName, Email, PrimaryPhone
                    from Person p join Applicant a on p.PersonID = a.ApplicantID
                    where a.isRejected = 0 and a.isSelected = 0 and a.isShortlisted = 0
                    ", con);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Applicant a = new Applicant();
                        a.applicantID = reader.GetInt32(0);
                        a.person.FirstName = reader.GetString(1);   
                        a.person.Email = reader.GetString(2);
                        a.person.PrimaryPhone = reader.GetString(3);


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
