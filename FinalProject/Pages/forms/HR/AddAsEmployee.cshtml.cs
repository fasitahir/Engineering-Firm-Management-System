using FinalProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace FinalProject.Pages.forms.HR
{
    public class ApplicantDetailsModel : PageModel
    {
        public Applicant applicant = new Applicant();

        [BindProperty]
        public EmergencyContact emergencyContact { get; set; } = new EmergencyContact();
        [BindProperty]
        public Employee employee { get; set; } = new Employee();

        public void OnGet(int id)
        {
            applicant.applicantID = id;

            // Get database connection
            var con = Configuration.getInstance().getConnection();

            // SQL command to select applicant information
            SqlCommand cmd = new SqlCommand(@"SELECT FirstName, LastName, CNIC, PrimaryPhone, L.Value, PictureName
                                      FROM Person p 
                                      JOIN Applicant a ON p.PersonID = a.ApplicantID
                                      JOIN Lookup L ON a.DesiredDesignation = L.lookupID
                                      WHERE a.ApplicantID = @ApplicantID", con);

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

        public void OnPost(int id)
        {
            employee.EmployeeId = id;
            int gender = 0, relation = 0, designation = 0;
            string firstName = "", lastName = "", address = "", phoneNumber = "", alternateNumber = "",
                email = "", cnic = "", employeeNo = "";
            DateTime dateOfBirth = DateTime.Today;
            double Salary = 0;
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
            if (designation != 0 && emergencyContact.person.Gender != 0)
            {
                bool isSelectd = employee.applicant.isSelected;
                Salary = employee.Salary;
                employeeNo = "";

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
                    var con = Configuration.getInstance().getConnection();
                    SqlCommand cmd = new SqlCommand(@" INSERT INTO Employee
                        VALUES (@EmployeeId, @Designation, null, null, @EmployeeNo, @StartDate, null, 1, @BaseSalary)
                        INSERT INTO Person
                        VALUES (@Gender,@CNIC,@FirstName,@LastName, @Email, @PrimaryPhone, @AlternatePhone, @DateOfBirth, @Address)
                        INSERT INTO EmergencyContact
                        VALUES((SELECT MAX(PersonID) FROM Person),@EmployeeId, @Relationship) 
                        UPDATE Applicant
                        SET IsSelected = 1
                        WHERE ApplicantID = @EmployeeId
                        ", con);
                    cmd.Parameters.AddWithValue("@FirstName", firstName);
                    cmd.Parameters.AddWithValue("@Gender", gender);
                    cmd.Parameters.AddWithValue("@DateOfBirth", dateOfBirth);
                    cmd.Parameters.AddWithValue("@Address", address);
                    cmd.Parameters.AddWithValue("@PrimaryPhone", phoneNumber);

                    cmd.Parameters.AddWithValue("@Designation", designation);
                    cmd.Parameters.AddWithValue("@EmployeeId", id);
                    cmd.Parameters.AddWithValue("@StartDate", DateTime.Now);
                    cmd.Parameters.AddWithValue("@EndDate", DBNull.Value);
                    cmd.Parameters.AddWithValue("@EmployeeNo", employeeNo);
                    cmd.Parameters.AddWithValue("@BaseSalary", Salary);
                    cmd.Parameters.AddWithValue("@Relationship", relation);

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


                    cmd.ExecuteNonQuery();


                }
                catch (Exception e)
                {
                    TempData["ErrorMessage"] = e.Message;
                }
                OnGet(id);
            }
            else
            {
                TempData["ErrorMessage"] = "Please Fill all the information";
                OnGet(id);
            }
        }


        public void OnPostSubmission()
        {
            int designation = employee.Designation;
            double baseSalary = employee.Salary;
        }
    }
}
