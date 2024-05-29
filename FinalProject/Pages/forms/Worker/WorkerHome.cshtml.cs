using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using FinalProject.Models;
using iTextSharp.text;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Document = iTextSharp.text.Document;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Data.SqlClient;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using iText.StyledXmlParser.Jsoup.Select;
using iText.StyledXmlParser.Jsoup.Safety;
using static iTextSharp.text.pdf.AcroFields;

namespace FinalProject.Pages.forms.Worker
{
    public class WorkerHomeModel : PageModel
    {
        public static SqlConnection con = Configuration.getInstance().getConnection();
        List<makeList> List { get; set; } = new List<makeList>();
        public static string PictureName { get; set; }
        public static string Name { get; set; }
        public static int employeeID { get; set; }
        public static int RequiredemployeeID { get; set; }
        public static string designation { get; set; }
        public static string employeeNumber { get; set; }
        public static DateTime StartDate { get; set; }
        public static DateTime? EndDate { get; set; }
        public static DateTime? PaidDate { get; set; }
        public static decimal salary { get; set; }
        public static decimal PaidSalary { get; set; }
        public static decimal? PaidAmount { get; set; }
        public static decimal baseSalary { get; set; }

        public void OnGet(int EmployeeID)
        {
            IndexModel.employeeId = EmployeeID;
            RequiredemployeeID = EmployeeID;
            SqlCommand cmd = new SqlCommand(@"SELECT PictureName, FirstName + ' ' +ISNULL(LastName,'') AS FullName, EmployeeNumber, StartDate,EndDate, BaseSalary, l.Value AS Designation,E.BaseSalary,s.PaidAmount,s.PaidDate
                  FROM Applicant A
                  JOIN Person P ON A.ApplicantID = P.PersonID
				  JOIN Employee E ON E.EmployeeID = A.ApplicantID
                  JOIN Lookup l ON l.LookupID = E.DesignationID
				  LEFT Join Salary s On s.EmployeeID = E.EmployeeID
                  WHERE ApplicantID = @EmployeeID", con);
            cmd.Parameters.AddWithValue("@EmployeeID", EmployeeID);
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    PictureName = reader.GetString(0);
                    Name = reader.GetString(1);
                    employeeNumber = reader.GetString(2);
                    StartDate = reader.GetDateTime(3);
                    if (!reader.IsDBNull(4))
                    {
                        EndDate = reader.GetDateTime(4);
                    }
                    else
                    {
                        EndDate = null; // or DateTime.MinValue, depending on your preference
                    }

                    // Similarly, handle other nullable fields
                    if (!reader.IsDBNull(8))
                    {
                        PaidAmount = reader.GetDecimal(8);
                    }
                    else
                    {
                        PaidAmount = null; // or 0, depending on your preference
                    }

                    if (!reader.IsDBNull(9))
                    {
                        PaidDate = reader.GetDateTime(9);
                    }
                    else
                    {
                        PaidDate = null; // or DateTime.MinValue, depending on your preference
                    }

                    salary = reader.GetDecimal(5);
                    designation = reader.GetString(6);
                    baseSalary = reader.GetDecimal(7);
                }
            }
        }


        public IActionResult OnPostAppointment()
        {
            try
            {
                // Create PDF document
                string fileName = $"Appointment_Letter.pdf";

                using (FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    Document document = new Document(PageSize.A4);
                    PdfWriter writer = PdfWriter.GetInstance(document, fs);
                    document.Open();

                    // Add company logo
                    string logoPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/company_logo.png");
                    iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(logoPath);

                    // Set compression level to zero (no compression)
                    logo.CompressionLevel = 0;

                    // Scale the image and set alignment
                    logo.ScaleAbsolute(100f, 100f);
                    logo.Alignment = Element.ALIGN_LEFT;

                    document.Add(logo);

                    // Add title
                    AddTitle(document, "Appointment Letter");

                    // Add employee information
                    AddEmployeeInfo(document);

                    // Add footer
                    AddFooter(document);

                    document.Close();
                }

                // Read the generated file and return it as a downloadable attachment
                byte[] fileBytes = System.IO.File.ReadAllBytes(fileName);
                return File(fileBytes, "application/pdf", fileName);
            }
            catch (Exception ex)
            {
                // Handle exceptions (e.g., log, show error message)
                return BadRequest("Error generating report: " + ex.Message);
            }
        }
        public IActionResult OnPostExperienceLetter()
        {
            try
            {
                // Create PDF document
                string fileName = $"Experince_Letter.pdf";

                using (FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    Document document = new Document(PageSize.A4);
                    PdfWriter writer = PdfWriter.GetInstance(document, fs);
                    document.Open();

                    // Add company logo
                    string logoPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/company_logo.png");
                    iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(logoPath);

                    // Set compression level to zero (no compression)
                    logo.CompressionLevel = 0;

                    // Scale the image and set alignment
                    logo.ScaleAbsolute(100f, 100f);
                    logo.Alignment = Element.ALIGN_LEFT;

                    document.Add(logo);

                    // Add title
                    AddTitle(document, "Experince Letter");

                    // Add employee information
                    EmployeeInfoForExperience(document);

                    // Add footer
                    AddFooter(document);

                    document.Close();
                }

                // Read the generated file and return it as a downloadable attachment
                byte[] fileBytes = System.IO.File.ReadAllBytes(fileName);
                return File(fileBytes, "application/pdf", fileName);
            }
            catch (Exception ex)
            {
                // Handle exceptions (e.g., log, show error message)
                return BadRequest("Error generating report: " + ex.Message);
            }
        }

        public IActionResult OnPostSalaryReport()
        {

            /*SqlCommand cmd = new SqlCommand(@"SELECT PictureName, FirstName + ' ' +ISNULL(LastName,'') AS FullName, EmployeeNumber, StartDate,EndDate, BaseSalary, l.Value AS Designation,E.BaseSalary,s.PaidAmount,s.PaidDate
                  FROM Applicant A
                  JOIN Person P ON A.ApplicantID = P.PersonID
				  JOIN Employee E ON E.EmployeeID = A.ApplicantID
                  JOIN Lookup l ON l.LookupID = E.DesignationID
				  Join Salary s On s.EmployeeID = E.EmployeeID
                  WHERE ApplicantID = @EmployeeID", con);
            cmd.Parameters.AddWithValue("@EmployeeID", 14);
            //RequiredemployeeID
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    makeList list = new makeList();
                    PictureName = reader.GetString(0);
                    Name = reader.GetString(1);
                    employeeNumber = reader.GetString(2);
                    StartDate = reader.GetDateTime(3);
                    EndDate = reader.GetDateTime(4);
                    salary = reader.GetDecimal(5);
                    designation = reader.GetString(6);
                    baseSalary = reader.GetDecimal(7);
                    PaidAmount = reader.GetDecimal(8);
                    PaidDate = reader.GetDateTime(9);
                    List.Add(list);

                }
            }
            cmd.ExecuteNonQuery();
            */
            try
            {
                // Create PDF document
                string fileName = $"Salary_Report.pdf";

                using (FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    Document document = new Document(PageSize.A4);
                    PdfWriter writer = PdfWriter.GetInstance(document, fs);
                    document.Open();

                    // Add company logo
                    string logoPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/company_logo.png");
                    iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(logoPath);

                    // Set compression level to zero (no compression)
                    logo.CompressionLevel = 0;

                    // Scale the image and set alignment
                    logo.ScaleAbsolute(100f, 100f);
                    logo.Alignment = Element.ALIGN_LEFT;

                    document.Add(logo);

                    // Add titleSalaExperince Letter");
                    AddTitle(document, "Salary Report");
                    // Add employee information
                    EmployeeInfoSalaryReport(document);

                    // Add footer
                    AddFooter(document);

                    document.Close();
                }

                // Read the generated file and return it as a downloadable attachment
                byte[] fileBytes = System.IO.File.ReadAllBytes(fileName);
                return File(fileBytes, "application/pdf", fileName);
            }
            catch (Exception ex)
            {
                // Handle exceptions (e.g., log, show error message)
                return BadRequest("Error generating report: " + ex.Message);
            }
        }
        private void AddTitle(Document document, string title)
        {
            // Add title to the PDF document
            Paragraph titleParagraph = new Paragraph(title, FontFactory.GetFont(FontFactory.HELVETICA, 16, iTextSharp.text.Font.BOLD));
            titleParagraph.Alignment = Element.ALIGN_CENTER;
            titleParagraph.SpacingAfter = 20f;  // Adding space after the title
            document.Add(titleParagraph);
        }


        private void AddEmployeeInfo(Document document)
        {
            // Add employee information to the PDF document
            Paragraph employeeInfoParagraph = new Paragraph();
            Chunk nameTitle = new Chunk("Name: ", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12));
            Chunk nameValue = new Chunk(Name + "\n", FontFactory.GetFont(FontFactory.HELVETICA, 12));
            employeeInfoParagraph.Add(nameTitle);
            employeeInfoParagraph.Add(nameValue);

            Chunk empNumberTitle = new Chunk("Employee Number: ", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12));
            Chunk empNumberValue = new Chunk(employeeNumber + "\n", FontFactory.GetFont(FontFactory.HELVETICA, 12));
            employeeInfoParagraph.Add(empNumberTitle);
            employeeInfoParagraph.Add(empNumberValue);

            Chunk designationTitle = new Chunk("Designation: ", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12));
            Chunk designationValue = new Chunk(designation + "\n", FontFactory.GetFont(FontFactory.HELVETICA, 12));
            employeeInfoParagraph.Add(designationTitle);
            employeeInfoParagraph.Add(designationValue);

            Chunk startDateTitle = new Chunk("Start Date: ", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12));
            Chunk startDateValue = new Chunk(StartDate.ToString("MMMM dd, yyyy") + "\n", FontFactory.GetFont(FontFactory.HELVETICA, 12));
            employeeInfoParagraph.Add(startDateTitle);
            employeeInfoParagraph.Add(startDateValue);

            Chunk salaryTitle = new Chunk("Salary: $", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12));
            Chunk salaryValue = new Chunk(salary.ToString("0.00") + "\n\n", FontFactory.GetFont(FontFactory.HELVETICA, 12));
            employeeInfoParagraph.Add(salaryTitle);
            employeeInfoParagraph.Add(salaryValue);

            employeeInfoParagraph.SpacingAfter = 20f;

            //Paragraph appointmentLetterLines = new Paragraph();
            //appointmentLetterLines.Add("We are pleased to inform you that you have been appointed as a [position] at Ambala Engineering, effective from ");
            //appointmentLetterLines.Add(StartDate.ToString("MMMM dd, yyyy") + ".\n");
            //appointmentLetterLines.Add("Please find attached your appointment letter for further details.\n\n");
            //appointmentLetterLines.Add("Best Regards,\n[Your Name]\n[Your Position]\nAmbala Engineering\n\n");
            Paragraph appointmentLetterLines = new Paragraph();
            appointmentLetterLines.Add($"We are pleased to inform you that you have been appointed as a {designation} at Ambala Engineering, effective from ");
            appointmentLetterLines.Add($"{StartDate.ToString("MMMM dd, yyyy")}. \n\n");

            //appointmentLetterLines.Add("Best Regards,\n");
            //appointmentLetterLines.Add("Ch. Zeshan Ahmad\n");
            //appointmentLetterLines.Add("CEO\n");
            //appointmentLetterLines.Add("Ambala Engineering\n\n");
            //Font boldFont = FontFactory.GetFont(FontFactory.HELVETICA, 12, iTextSharp.text.Font.BOLD);
            //Chunk bestRegards = new Chunk("Best Regards,\n", boldFont);
            //Chunk ceoName = new Chunk("Ch. Zeshan Ahmad\n", boldFont);
            //Chunk companyName = new Chunk("CEO\n", boldFont);
            //Chunk company = new Chunk("Ambala Engineering\n\n", boldFont);

            //// Add bold chunks to the paragraph
            //appointmentLetterLines.Add(bestRegards);
            //appointmentLetterLines.Add(ceoName);
            //appointmentLetterLines.Add(companyName);
            //appointmentLetterLines.Add(company);

            //Paragraph appointmentStyle = new Paragraph("Ambala Engineering follows a modern and inclusive approach to employee appointments, fostering growth and innovation within our team.", FontFactory.GetFont(FontFactory.HELVETICA, 12));
            //appointmentStyle.Alignment = Element.ALIGN_JUSTIFIED;
            //appointmentStyle.SpacingAfter = 20f;

            document.Add(employeeInfoParagraph);
            document.Add(appointmentLetterLines);
            //document.Add(appointmentStyle);
        }



        private void AddFooter(Document document)
        {
            // Create a new paragraph for the footer
            Paragraph footer = new Paragraph();

            // Add the "Best Regards" lines with bold font and left alignment
            Font boldFont = FontFactory.GetFont(FontFactory.HELVETICA, 10, iTextSharp.text.Font.BOLD);
            Font normalFont = FontFactory.GetFont(FontFactory.HELVETICA, 10);
            Chunk bestRegards = new Chunk("Best Regards,\n", boldFont);
            Chunk ceoName = new Chunk("Ch. Zeshan Ahmad\n", normalFont);
            Chunk companyName = new Chunk("CEO\n", normalFont);
            Chunk company = new Chunk("Ambala Engineering\n\n", normalFont);

            footer.Add(bestRegards);
            footer.Add(ceoName);
            footer.Add(companyName);
            footer.Add(company);

            // Add the copyright message
            footer.Add("© 2024 Ambala Engineering. All rights reserved.");

            // Set alignment and spacing for the footer
            footer.Alignment = Element.ALIGN_LEFT; // Align the footer to the left
            footer.SpacingBefore = 120f;

            // Add the footer to the PDF document
            document.Add(footer);
        }


        private void EmployeeInfoForExperience(Document document)
        {


            // Add employee information to the PDF document
            Paragraph employeeInfoParagraph = new Paragraph();
            Chunk nameTitle = new Chunk("Name: ", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12));
            Chunk nameValue = new Chunk(Name + "\n", FontFactory.GetFont(FontFactory.HELVETICA, 12));
            employeeInfoParagraph.Add(nameTitle);
            employeeInfoParagraph.Add(nameValue);

            Chunk empNumberTitle = new Chunk("Employee Number: ", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12));
            Chunk empNumberValue = new Chunk(employeeNumber + "\n", FontFactory.GetFont(FontFactory.HELVETICA, 12));
            employeeInfoParagraph.Add(empNumberTitle);
            employeeInfoParagraph.Add(empNumberValue);

            Chunk designationTitle = new Chunk("Designation: ", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12));
            Chunk designationValue = new Chunk(designation + "\n", FontFactory.GetFont(FontFactory.HELVETICA, 12));
            employeeInfoParagraph.Add(designationTitle);
            employeeInfoParagraph.Add(designationValue);
            employeeInfoParagraph.SpacingAfter = 20f;


            Paragraph appointmentLetterLines = new Paragraph();
            appointmentLetterLines.Add($"This is certify that {Name} has been a part of our firm since {StartDate.ToString("MMMM dd, yyyy")}." +
                $" During their tenure with us {Name} has shown dedication,professionalism, and a strong commitment to his/her work. \n\n" +
                $"We thank {Name} for his/her service and wish them the best for the future endeavours."
                );



            document.Add(employeeInfoParagraph);
            document.Add(appointmentLetterLines);
            //document.Add(appointmentStyle);
        }
        private void EmployeeInfoSalaryReport(Document document)
        {


            // Add employee information to the PDF document
            Paragraph employeeInfoParagraph = new Paragraph();
            Chunk nameTitle = new Chunk("Name: ", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12));
            Chunk nameValue = new Chunk(Name + "\n", FontFactory.GetFont(FontFactory.HELVETICA, 12));
            employeeInfoParagraph.Add(nameTitle);
            employeeInfoParagraph.Add(nameValue);

            Chunk empNumberTitle = new Chunk("Employee Number: ", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12));
            Chunk empNumberValue = new Chunk(employeeNumber + "\n", FontFactory.GetFont(FontFactory.HELVETICA, 12));
            employeeInfoParagraph.Add(empNumberTitle);
            employeeInfoParagraph.Add(empNumberValue);

            Chunk designationTitle = new Chunk("Designation: ", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12));
            Chunk designationValue = new Chunk(designation + "\n", FontFactory.GetFont(FontFactory.HELVETICA, 12));
            employeeInfoParagraph.Add(designationTitle);
            employeeInfoParagraph.Add(designationValue);
            employeeInfoParagraph.SpacingAfter = 20f;


            //Paragraph appointmentLetterLines = new Paragraph();
            //appointmentLetterLines.Add($"This is certify that {Name} has been a part of our firm since {StartDate.ToString("MMMM dd, yyyy")}." +
            //    $" During their tenure with us {Name} has shown dedication,professionalism, and a strong commitment to his/her work. \n\n" +
            //    $"We thank {Name} for his/her service and wish them the best for the future endeavours."
            //    );
            SqlCommand cmd = new SqlCommand(@"SELECT PictureName, FirstName + ' ' +ISNULL(LastName,'') AS FullName, EmployeeNumber, StartDate,EndDate, BaseSalary, l.Value AS Designation,E.BaseSalary,s.PaidAmount,s.PaidDate
                  FROM Applicant A
                  JOIN Person P ON A.ApplicantID = P.PersonID
				  JOIN Employee E ON E.EmployeeID = A.ApplicantID
                  JOIN Lookup l ON l.LookupID = E.DesignationID
				  Join Salary s On s.EmployeeID = E.EmployeeID
                  WHERE ApplicantID = @EmployeeID", con);
            cmd.Parameters.AddWithValue("@EmployeeID", RequiredemployeeID);
            //RequiredemployeeID
            PdfPTable table = new PdfPTable(4); // Create a table with 4 columns

            // Add table headers
            table.AddCell("Base Salary");
            table.AddCell("Paid Amount");
            table.AddCell("Paid Date");
            table.AddCell("Designation");


            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    makeList list = new makeList();
                    PictureName = reader.GetString(0);
                    Name = reader.GetString(1);
                    employeeNumber = reader.GetString(2);
                    StartDate = reader.GetDateTime(3);
                    EndDate = reader.GetDateTime(4);
                    salary = reader.GetDecimal(5);
                    designation = reader.GetString(6);
                    baseSalary = reader.GetDecimal(7);
                    PaidAmount = reader.GetDecimal(8);
                    PaidDate = reader.GetDateTime(9);
                    List.Add(list);

                    table.AddCell(baseSalary.ToString());
                    table.AddCell(PaidAmount.ToString());
                    table.AddCell(PaidDate.ToString());
                    table.AddCell(designation);

                }
            }





            // Add data rows from the list
            /*foreach (var item in List)
            {
                table.AddCell(item.baseSalary.ToString());
                table.AddCell(item.PaidAmount.ToString());
                table.AddCell(item.PaidDate.ToString("MMMM dd, yyyy"));
                table.AddCell(item.designation);
            }*/

            // Add table to the PDF document
            document.Add(table);


            document.Add(employeeInfoParagraph);
            //document.Add(appointmentLetterLines);
            //document.Add(appointmentStyle);
        }

    }

    class makeList
    {
        public string Name { get; set; }
        public int employeeID { get; set; }
        public string designation { get; set; }
        public string employeeNumber { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? PaidDate { get; set; }
        public decimal salary { get; set; }
        public decimal PaidSalary { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal baseSalary { get; set; }

    }
}
