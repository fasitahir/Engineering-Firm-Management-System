namespace FinalProject.Models
{
	public class Employee
	{
        public Applicant applicant { get; set; } = new Applicant();
        public int EmployeeId { get; set; }
        public int Designation {  get; set; }
        public string DesignationName { get; set; }
        public double Salary { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime PaidDate { get; set; }
        public bool IsPaid { get; set; }
        public double PaidAmount { get; set; }
        public string EmployeeNo { get; set; }
        public bool IsActive { get; set; }
    }
}
