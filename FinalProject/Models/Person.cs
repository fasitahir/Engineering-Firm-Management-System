namespace FinalProject.Models
{
    public class Person
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PrimaryPhone { get; set; }
        public string? AlternatePhone { get; set;}
        public string CNIC { get; set; }
        public string Address { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public string Gender { get; set; }
        public IFormFile CV { get; set; }
        public IFormFile Picture { get; set; }
    }
}
