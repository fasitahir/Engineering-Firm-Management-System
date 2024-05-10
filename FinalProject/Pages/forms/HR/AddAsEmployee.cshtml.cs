using FinalProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using System.Data.SqlClient;

namespace FinalProject.Pages.forms.HR
{
    public class ApplicantDetailsModel : PageModel
    {

        public static SqlConnection con = Configuration.getInstance().getConnection();
        int gender = 0, relation = 0, designation = 0;
        string firstName = "", lastName = "", address = "", phoneNumber = "", alternateNumber = "",
            email = "", cnic = "", employeeNo = "";
        DateTime dateOfBirth = DateTime.Today;
        double Salary = 0;

        public Applicant applicant = new Applicant();

        [BindProperty]
        public EmergencyContact emergencyContact { get; set; } = new EmergencyContact();
        [BindProperty]
        public Employee employee { get; set; } = new Employee();

        public void OnGet(int id)
        {
            applicant.applicantID = id;

            try
            {

                //View is used to load data
                SqlCommand cmd = new SqlCommand(@"SELECT FirstName, LastName, CNIC, PrimaryPhone, L.Value, PictureName, ApplicantID
                FROM ViewApplicants
                WHERE ApplicantID = @ApplicantID", con);


                // Add parameter for ApplicantID
                cmd.Parameters.AddWithValue("@ApplicantID", applicant.applicantID);


                // Execute SQL command and read results
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    // Parse results and assign to applicant.person properties
                    if (reader.Read())
                    {
                        applicant.person.FirstName = reader.GetString(0);
                        applicant.person.LastName = !reader.IsDBNull(1) ? reader.GetString(1) : "";
                        applicant.person.CNIC = reader.GetString(2);
                        applicant.person.PrimaryPhone = reader.GetString(3);
                        applicant.person.DesignationValue = reader.GetString(4);
                        applicant.person.PictureName = reader.GetString(5);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"An error occurred: {e.Message}");
            }
        }


        public void OnPost(int id)
        {
            employee.EmployeeId = id;
            int personId = 0;

            gender = Convert.ToInt32(Request.Form["emergencyContactGenderHidden"]);
            firstName = Request.Form["emergencyContactFirstNameHidden"];
            lastName = Request.Form["emergencyContactLastNameHidden"];
            cnic = Request.Form["emergencyContactCNICHidden"];
            phoneNumber = Request.Form["emergencyContactPrimaryPhoneHidden"];
            alternateNumber = Request.Form["emergencyContactAlternatePhoneHidden"];
            relation = Convert.ToInt32(Request.Form["emergencyContactrelationHidden"]);
            email = Request.Form["emergencyContactEmailHidden"];
            address = Request.Form["emergencyContactAddressHidden"];
            dateOfBirth = Convert.ToDateTime(Request.Form["emergencyContactDateOfBirthHidden"]);


            emergencyContact.applicantID = id;
            if (emergencyContact.person.Gender != 0)
            {
                gender = emergencyContact.person.Gender;
                firstName = emergencyContact.person.FirstName;
                lastName = emergencyContact.person.LastName;
                relation = emergencyContact.relation;
                dateOfBirth = emergencyContact.person.DateOfBirth;
                address = emergencyContact.person.Address;
                phoneNumber = emergencyContact.person.PrimaryPhone;
                alternateNumber = emergencyContact.person.AlternatePhone;
                email = emergencyContact.person.Email;
                cnic = emergencyContact.person.CNIC;

                OnGet(id);

            }
            else
            {
                TempData["Message"] = "Please fill out emergency contact details.";
                OnGet(id);
            }
            designation = employee.Designation;
            if (designation != 0 && gender != 0)
            {
                bool isSelectd = employee.applicant.isSelected;
                if (double.TryParse(Request.Form["baseSalary"], out double baseSalary))
                {
                    // Set the value to the employee's salary
                    employee.BaseSalary = baseSalary;
                }
                else
                {
                    // Handle invalid input or default value
                    TempData["ErrorMessage"] = "Invalid base salary value.";
                }
                employeeNo = "";
                Salary = employee.BaseSalary;

                if (designation == 3)
                {
                    employeeNo = "Worker-" + id.ToString();
                }
                else if (designation == 4)
                {
                    employeeNo = "Accountant-" + id.ToString();
                }
                else if (designation == 5)
                {
                    employeeNo = "HR-" + id.ToString();
                }
                else if (designation == 6)
                {
                    employeeNo = "Inventory Manager-" + id.ToString();
                }

                try
                {


                    SqlCommand cmd = new SqlCommand("stp_AddEmployee", con);

                    cmd.CommandType = CommandType.StoredProcedure;

                    // Add parameters
                    cmd.Parameters.AddWithValue("@Designation", designation);
                    cmd.Parameters.AddWithValue("@EmployeeId", id);
                    cmd.Parameters.AddWithValue("@EmployeeNo", employeeNo);
                    cmd.Parameters.AddWithValue("@StartDate", DateTime.Now);
                    cmd.Parameters.AddWithValue("@BaseSalary", Salary);
                    cmd.Parameters.AddWithValue("@FirstName", firstName);
                    cmd.Parameters.AddWithValue("@LastName", string.IsNullOrEmpty(lastName) ? (object)DBNull.Value : lastName);
                    cmd.Parameters.AddWithValue("@Gender", gender);
                    cmd.Parameters.AddWithValue("@DateOfBirth", dateOfBirth);
                    cmd.Parameters.AddWithValue("@Address", address);
                    cmd.Parameters.AddWithValue("@PrimaryPhone", phoneNumber);
                    cmd.Parameters.AddWithValue("@AlternatePhone", string.IsNullOrEmpty(alternateNumber) ? (object)DBNull.Value : alternateNumber);
                    cmd.Parameters.AddWithValue("@CNIC", cnic);
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@Relation", relation);
                    cmd.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    TempData["ErrorMessage"] = e.Message;
                    OnGet(id);

                }

            }
            else
            {

                OnGet(id);

            }
        }
    }
}
