using FinalProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using static FinalProject.Pages.forms.HR.ShortlisteddApplicantsModel;

namespace FinalProject.Pages.forms.HR
{
    public class shortlistModel : PageModel
    {

        public static SqlConnection con = Configuration.getInstance().getConnection();
        public List<Applicant> applicants = new List<Applicant>();
        public string SelectedStatusText { get; set; }  = "All Applicants";
        public void OnGet(string status)
        {
            if(string.IsNullOrEmpty(status) || status == "all")
            {
                GetAllApplicants();
                SelectedStatusText = "All Applicants";
            }
            else
            {
                OnGetFilter(status);
            }
        }

        public void OnGetFilter(string status)
        {
            // Filter applicants based on status
            GetFilteredApplicants(status);
            SelectedStatusText = status switch
            {
                "shortlisted" => "Shortlisted Applicants",
                "rejected" => "Rejected Applicants",
                "all" => "All Applicants",
                "selected" => "Selected Applicants",
                _ => "All Applicants"
            };
        }

        private void GetAllApplicants()
        {
            try
            {
                SqlCommand cmd = new SqlCommand(@"select ApplicantID, FirstName, Email, PrimaryPhone, CVName 
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
                        a.CVPath = "/CVs/" + reader.GetString(4);

                        applicants.Add(a);
                    }
                }

            }
            catch (Exception e)
            {
                TempData["ErrorMessage"] = e.Message;

            }
        }

        private void GetFilteredApplicants(string status)
        {
            try
            {

                SqlCommand cmd;
                if (status == "shortlisted")
                {
                    cmd = new SqlCommand(@"select ApplicantID, FirstName, Email, PrimaryPhone, isShortlisted, isRejected, isSelected, CVName 
                    from Person p join Applicant a on p.PersonID = a.ApplicantID
                    where a.isRejected = 0 and a.isSelected = 0 and a.isShortlisted = 1
                    ", con);

                }
                else if(status == "rejected")
                {
                    cmd = new SqlCommand(@"select ApplicantID, FirstName, Email, PrimaryPhone, isShortlisted, isRejected, isSelected, CVName
                    from Person p join Applicant a on p.PersonID = a.ApplicantID
                    where a.isRejected = 1 and a.isSelected = 0
                    ", con);
                }
                else
                {
                    //All selected
                    cmd = new SqlCommand(@"select ApplicantID, FirstName, Email, PrimaryPhone, isShortlisted, isRejected, isSelected, CVName  
                    from Person p join Applicant a on p.PersonID = a.ApplicantID
                    where a.isRejected = 0 and a.isSelected = 1
                    ", con);
                }

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Applicant a = new Applicant();
                        a.applicantID = reader.GetInt32(0);
                        a.person.FirstName = reader.GetString(1);
                        a.person.Email = reader.GetString(2);
                        a.person.PrimaryPhone = reader.GetString(3);
                        a.isShortListed = reader.GetBoolean(4);
                        a.isRejected = reader.GetBoolean(5);
                        a.isSelected = reader.GetBoolean(6);
                        a.CVPath = "/CVs/" + reader.GetString(7);
                        applicants.Add(a);
                    }
                }

            }
            catch (Exception e)
            {
                TempData["ErrorMessage"] = e.Message;

            }
        }
        

        public async Task<IActionResult> OnPostShortlistAsync(int applicantId)
        {
            try
            {
                
                SqlCommand cmd = new SqlCommand(@"UPDATE Applicant SET isShortlisted = 1 WHERE ApplicantID = @ApplicantID", con);
                

                cmd.Parameters.AddWithValue("@ApplicantID", applicantId);
                await cmd.ExecuteNonQueryAsync();


                return RedirectToPage();
            }
            catch (Exception e)
            {
                return new JsonResult(new { error = e.Message }) { StatusCode = 500 };
            }
        }

        public async Task<IActionResult> OnPostRejectAsync(int applicantId)
        {
            try
            {
                
                SqlCommand cmd = new SqlCommand(@"UPDATE Applicant SET isRejected = 1 WHERE ApplicantID = @ApplicantID", con);
                cmd.Parameters.AddWithValue("@ApplicantID", applicantId);
                await cmd.ExecuteNonQueryAsync();

                return RedirectToPage();
            }
            catch (Exception e)
            {
                return new JsonResult(new { error = e.Message }) { StatusCode = 500 };
            }
        }

    }
}
