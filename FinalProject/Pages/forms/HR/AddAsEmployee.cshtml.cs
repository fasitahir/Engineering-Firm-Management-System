using FinalProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Net;
using System.Reflection;

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
                    employee.Salary = baseSalary;
                }
                else
                {
                    // Handle invalid input or default value
                    TempData["ErrorMessage"] = "Invalid base salary value.";
                }
                employeeNo = "";
                Salary = employee.Salary;

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
                  

                    // Begin transaction
                    SqlTransaction transaction = con.BeginTransaction();

                    try
                    {
                        // Check if a person with the same CNIC and email exists
                        SqlCommand checkExistingPerson = new SqlCommand(@"SELECT PersonID 
                                                                FROM Person 
                                                                WHERE CNIC = @CNIC OR Email = @Email", con, transaction);
                        checkExistingPerson.Parameters.AddWithValue("@CNIC", cnic);
                        checkExistingPerson.Parameters.AddWithValue("@Email", email);

                        int existingPersonId = (int?)checkExistingPerson.ExecuteScalar() ?? 0;
                        personId = existingPersonId;
                        // Insert employee and person details
                        SqlCommand cmd = new SqlCommand(@"INSERT INTO Employee
                                                VALUES (@EmployeeId, @Designation, null, null, @EmployeeNo, @StartDate, null, 1, @BaseSalary);
                                                ", con, transaction);

                        cmd.Parameters.AddWithValue("@Designation", designation);
                        cmd.Parameters.AddWithValue("@EmployeeId", id);
                        cmd.Parameters.AddWithValue("@StartDate", DateTime.Now);
                        cmd.Parameters.AddWithValue("@EndDate", DBNull.Value);
                        cmd.Parameters.AddWithValue("@EmployeeNo", employeeNo);
                        cmd.Parameters.AddWithValue("@BaseSalary", Salary);

                        // Execute the query and get the inserted person's ID
                        cmd.ExecuteNonQuery();

                        // If a person with the same CNIC and email already exists, use their ID
                        if (existingPersonId == 0)
                        {
                            SqlCommand addPerson = new SqlCommand(@"   INSERT INTO Person
                                    OUTPUT INSERTED.PersonID
                                    VALUES(@Gender, @CNIC, @FirstName, @LastName, @Email, @PrimaryPhone, @AlternatePhone, @DateOfBirth, @Address)", con, transaction);
                            addPerson.Parameters.AddWithValue("@FirstName", firstName);
                            addPerson.Parameters.AddWithValue("@Gender", gender);
                            addPerson.Parameters.AddWithValue("@DateOfBirth", dateOfBirth);
                            addPerson.Parameters.AddWithValue("@Address", address);
                            addPerson.Parameters.AddWithValue("@PrimaryPhone", phoneNumber);
                            addPerson.Parameters.AddWithValue("@CNIC", cnic);
                            addPerson.Parameters.AddWithValue("@Email", email);
                            if (!string.IsNullOrEmpty(lastName))
                            {
                                addPerson.Parameters.AddWithValue("@LastName", lastName);

                            }
                            else
                            {
                                addPerson.Parameters.AddWithValue("@LastName", DBNull.Value);
                            }



                            if (!string.IsNullOrEmpty(alternateNumber))
                            {
                                addPerson.Parameters.AddWithValue("@AlternatePhone", alternateNumber);

                            }
                            else
                            {
                                addPerson.Parameters.AddWithValue("@AlternatePhone", DBNull.Value);
                            }
                            personId = (int)addPerson.ExecuteScalar();
                        }
                        // Insert emergency contact details
                        SqlCommand cmdEmergencyContact = new SqlCommand(@"INSERT INTO EmergencyContact
                                                                VALUES (@PersonId, @EmployeeId, @Relationship);", con, transaction);
                        cmdEmergencyContact.Parameters.AddWithValue("@PersonId", personId);
                        cmdEmergencyContact.Parameters.AddWithValue("@EmployeeId", id);
                        cmdEmergencyContact.Parameters.AddWithValue("@Relationship", relation);

                        // Execute emergency contact query
                        cmdEmergencyContact.ExecuteNonQuery();

                        // Update Applicant
                        SqlCommand updateApplicant = new SqlCommand(@"UPDATE Applicant
                                                            SET IsSelected = 1
                                                            WHERE ApplicantID = @EmployeeId", con, transaction);
                        updateApplicant.Parameters.AddWithValue("@EmployeeId", id);

                        // Execute update query
                        updateApplicant.ExecuteNonQuery();

                        // Commit transaction
                        transaction.Commit();
                        
                   
                    }
                    catch (Exception ex)
                    {
                        // Rollback transaction if an error occurs
                        transaction.Rollback();
                        TempData["ErrorMessage"] = ex.Message;
                    }
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
