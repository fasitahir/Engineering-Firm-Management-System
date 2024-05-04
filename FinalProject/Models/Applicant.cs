namespace FinalProject.Models
{
    public class Applicant
    {
        public Person person = new Person();
        public bool isSelected { get; set; }
        public bool isShortListed { get; set; }
        public bool isRejected { get; set; }
        public int applicantID {  get; set; }
    }
}
