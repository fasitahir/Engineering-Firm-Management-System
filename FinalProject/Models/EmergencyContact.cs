namespace FinalProject.Models
{
    public class EmergencyContact
    {
        public Person person { get; set; } = new Person();
        public int emergencyContactId { get; set; }
        public int applicantID { get; set; }
        public int relation { get; set; }

    }
}
