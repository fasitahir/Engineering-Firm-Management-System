namespace FinalProject.Models
{
    public class Applicant
    {
        public Person person { get; set; } = new Person();
        public bool isSelected { get; set; }
        public bool isShortListed { get; set; }
        public bool isRejected { get; set; }
        public int applicantID {  get; set; }
        public string CVPath { get; set;}
    }
}
