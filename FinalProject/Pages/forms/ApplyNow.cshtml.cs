using FinalProject.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.FileProviders;
using System.Data.SqlClient;

namespace FinalProject.Pages.forms
{
    public class ApplyNowModel : PageModel
    {
        [BindProperty]
        public Person person { get; set; }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                
                return Page();
            }

            int gender = person.Gender;
            string firstName = person.FirstName;
            string lastName = person.LastName;
            int desiredDesignation = person.DesiredDesignation;
            DateTime dateOfBirth = person.DateOfBirth;
            string address = person.Address;
            string phoneNumber = person.PrimaryPhone;
            string alternateNumber = person.AlternatePhone;
            string email = person.Email;
            string cnic = person.CNIC;
            IFormFile cv = person.CV;
            IFormFile picture = person.Picture;


            try
            {
                var con = Configuration.getInstance().getConnection();
                SqlCommand cmd = new SqlCommand(@"INSERT INTO Person
                        VALUES (@Gender,@CNIC,@FirstName,@LastName, @Email, @PrimaryPhone, @AlternatePhone, @DateOfBirth, @Address)
                        INSERT INTO Applicant
                        VALUES((SELECT MAX(PersonID) FROM Person),@DesiredDesignation,0, 0, null, null) ", con);
                cmd.Parameters.AddWithValue("@FirstName", firstName);
                cmd.Parameters.AddWithValue("@LastName", lastName);
                cmd.Parameters.AddWithValue("@DesiredDesignation", desiredDesignation);
                cmd.Parameters.AddWithValue("@Gender", gender);
                cmd.Parameters.AddWithValue("@DateOfBirth", dateOfBirth);
                cmd.Parameters.AddWithValue("@Address", address);
                cmd.Parameters.AddWithValue("@PrimaryPhone", phoneNumber);
                if(!string.IsNullOrEmpty(alternateNumber))
                {
                    cmd.Parameters.AddWithValue("@AlternatePhone", alternateNumber);

                }
                else
                {
                    cmd.Parameters.AddWithValue("@AlternatePhone", DBNull.Value);
                }
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@CNIC", cnic);
                

                /*if (cv != null)
                {
                    cmd.Parameters.AddWithValue("@CV", cv);

                }
                else
                {
                    cmd.Parameters.AddWithValue("@CV", DBNull.Value);
                }



                if (picture != null)
                {
                    cmd.Parameters.AddWithValue("@Picture", picture);

                }
                else
                {
                    cmd.Parameters.AddWithValue("@Picture", DBNull.Value);
                }


                */

                cmd.ExecuteNonQuery();

                
            }
            catch (Exception e)
            {
                TempData["ErrorMessage"] = e.Message;
                throw e;
            }

            
            return RedirectToPage("/Index");
        }
    }
}
