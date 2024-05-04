using FinalProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System;
using System.Net;

namespace FinalProject.Pages
{
    public class IndexModel : PageModel
    {
        [BindProperty]
        public Credentials credential { get; set; }
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            
        }

        public IActionResult OnPost()
        {
            int designation = 0;
            int EmployeeID = 0;

            string userName = credential.userName;
            string password = credential.password;


            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                var con = Configuration.getInstance().getConnection();
                SqlCommand cmd = new SqlCommand(@"SELECT DesignationID, C.EmployeeID
                    FROM Employee E
                    JOIN Credentials C ON C.EmployeeID = E.EmployeeID
                    WHERE C.Username = @UserName AND C.Password = @Password", con);

                cmd.Parameters.AddWithValue("@UserName", userName);
                cmd.Parameters.AddWithValue("@Password", password);

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    
                    designation = int.Parse(reader["DesignationID"].ToString());
                    EmployeeID = int.Parse(reader["EmployeeID"].ToString());

                }
                else
                {
                    TempData["ErrorMessage"] = "Invalid username or password.";
                    return Page();
                }

                reader.Close();
            }
            catch (Exception e)
            {
                TempData["ErrorMessage"] = e.Message;
                
            }

            // Redirect to the desired page upon successful authentication
            
            if(designation == 3)
            {
                return RedirectToPage("/forms/Worker/WorkerHome");

            }
            else if(designation == 5)
            {
                return RedirectToPage("/forms/HR/shortlist");
            }
            else
            {
                return RedirectToPage("/index");
            }
            
        }

    }
}
