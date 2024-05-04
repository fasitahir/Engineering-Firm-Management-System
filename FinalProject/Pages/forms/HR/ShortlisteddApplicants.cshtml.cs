using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FinalProject.Pages.forms.HR
{
    public class ShortlisteddApplicantsModel : PageModel
    {

        public List<Applicant> ShortlistedApplicants { get; set; }

        public void OnGet()
        {
            // Fetch shortlisted applicants from the database or any other data source
            // For demonstration purpose, let's assume we have a list of shortlisted applicants
            ShortlistedApplicants = GetShortlistedApplicants();
        }

        // Method to fetch shortlisted applicants (can be replaced with actual data retrieval logic)
        private List<Applicant> GetShortlistedApplicants()
        {
            // This is just a sample data, replace it with actual data retrieval logic
            return new List<Applicant>
            {
            new Applicant { ApplicantId = 1, Name = "John Doe" },
            new Applicant { ApplicantId = 2, Name = "Jane Smith" }
            // Add more applicants as needed
            };
        }

        // Model class for Applicant
        public class Applicant
        {
            public int ApplicantId { get; set; }
            public string Name { get; set; }
            // Other applicant properties can be added here
        }

        
    }
}
