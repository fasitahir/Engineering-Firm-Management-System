using FinalProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace FinalProject.Pages.forms
{
    public class ApplyNowModel : PageModel


    {


        public static SqlConnection con = Configuration.getInstance().getConnection();
        private readonly IWebHostEnvironment environment;

        [BindProperty]
        public Person person { get; set; }

        public ApplyNowModel(IWebHostEnvironment environment)
        {
            this.environment = environment;
        }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {

            if (string.IsNullOrWhiteSpace(person.FirstName) ||
                string.IsNullOrWhiteSpace(person.Email) ||
                string.IsNullOrWhiteSpace(person.PrimaryPhone) ||
                string.IsNullOrWhiteSpace(person.CNIC) ||
                string.IsNullOrWhiteSpace(person.Address) ||
                person.DateOfBirth == default(DateTime) ||
                person.Gender == 0 ||
                person.DesiredDesignation == 0 ||
                person.CV == null ||
                person.Picture == null)
            {
                // Handle the case where any required field is empty or null
                // For example, you can set an error message and return to the page
                TempData["ErrorMessage"] = "Please fill out all required fields.";
                return Page();
            }


            int gender = person.Gender;
            string firstName = person.FirstName;
            string? lastName = person.LastName;
            int desiredDesignation = person.DesiredDesignation;
            DateTime dateOfBirth = person.DateOfBirth;
            string address = person.Address;
            string phoneNumber = person.PrimaryPhone;
            string? alternateNumber = person.AlternatePhone;
            string email = person.Email;
            string cnic = person.CNIC;
            IFormFile cv = person.CV;
            IFormFile picture = person.Picture;


            try
            {

                SqlCommand cmd = new SqlCommand(@"INSERT INTO Person
                        VALUES (@Gender,@CNIC,@FirstName,@LastName, @Email, @PrimaryPhone, @AlternatePhone, @DateOfBirth, @Address)
                        INSERT INTO Applicant
                        VALUES((SELECT MAX(PersonID) FROM Person),@DesiredDesignation,0, 0, 0, @CVName, @PictureName) ", con);
                cmd.Parameters.AddWithValue("@FirstName", firstName);
                cmd.Parameters.AddWithValue("@DesiredDesignation", desiredDesignation);
                cmd.Parameters.AddWithValue("@Gender", gender);
                cmd.Parameters.AddWithValue("@DateOfBirth", dateOfBirth);
                cmd.Parameters.AddWithValue("@Address", address);
                cmd.Parameters.AddWithValue("@PrimaryPhone", phoneNumber);

                if (!string.IsNullOrEmpty(lastName))
                {
                    cmd.Parameters.AddWithValue("@LastName", lastName);

                }
                else
                {
                    cmd.Parameters.AddWithValue("@LastName", DBNull.Value);
                }



                if (!string.IsNullOrEmpty(alternateNumber))
                {
                    cmd.Parameters.AddWithValue("@AlternatePhone", alternateNumber);

                }
                else
                {
                    cmd.Parameters.AddWithValue("@AlternatePhone", DBNull.Value);
                }
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@CNIC", cnic);


                string pictureName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                pictureName += Path.GetExtension(person.Picture!.FileName);

                string picturePath = environment.WebRootPath + "/Pictures/" + pictureName;





                string CVName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                CVName += Path.GetExtension(person.CV!.FileName);

                string CVPath = environment.WebRootPath + "/CVs/" + CVName;


                if (picture != null)
                {
                    cmd.Parameters.AddWithValue("@PictureName", pictureName);

                }
                else
                {
                    cmd.Parameters.AddWithValue("@PictureName", DBNull.Value);
                }


                if (cv != null)
                {
                    cmd.Parameters.AddWithValue("@CVName", CVName);

                }
                else
                {
                    cmd.Parameters.AddWithValue("@CVName", DBNull.Value);
                }

                cmd.ExecuteNonQuery();

                using (var stream = System.IO.File.Create(picturePath))
                {
                    person.Picture.CopyTo(stream);
                }



                using (var stream = System.IO.File.Create(CVPath))
                {
                    person.CV.CopyTo(stream);
                }

            }
            catch (Exception e)
            {
                TempData["ErrorMessage"] = e.Message;
                Console.WriteLine(e.Message);
            }


            return RedirectToPage("/Index");
        }
    }
}
