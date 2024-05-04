using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FinalProject.Pages.forms.HR
{
    public class ApplicantDetailsModel : PageModel
    {
        public int ApplicantId { get; set; }
        public string FullName { get; set; }
        public string CNIC { get; set; }
        public string PhoneNumber { get; set; }
        public string DesiredDesignation { get; set; }
        public string ProfilePhoto { get; set; }

        public void OnGet(int id)
        {
            // Here, you would fetch the applicant details from your database based on the provided id
            // For demonstration purpose, I'm setting some dummy values
            ApplicantId = id;
            FullName = "John Doe";
            CNIC = "123456789";
            PhoneNumber = "123-456-7890";
            DesiredDesignation = "Software Engineer";
            ProfilePhoto = "/path/to/profile/photo.jpg";
        }


    }
}
