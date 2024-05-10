namespace FinalProject.Models
{
    public class Vehicle
    {
        public int vehicleId { get; set; }
        public Driver driver { get; set; } = new Driver();
        public string vehicleTypeName { get; set; }
        public int vehicleType { get; set; }
        public string plateNumber { get; set; }
    }
}
