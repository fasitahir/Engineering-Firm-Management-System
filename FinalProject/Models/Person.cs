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
        public DateTime DateOfBirth { get; set; }
        public int Gender { get; set; }

        public int DesiredDesignation { get; set; }

        public IFormFile? CV { get; set; }
        public IFormFile? Picture { get; set; }

    }
}
