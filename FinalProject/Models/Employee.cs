namespace FinalProject.Models
{
	public class Employee
	{
		public Applicant? Applicant { get; set; }
		public string? EmployeeId { get; set; }
		public double Salary { get; set; }
		public DateTime PaidDate { get; set;}
		public bool IsPaid { get; set; }

	}
}
