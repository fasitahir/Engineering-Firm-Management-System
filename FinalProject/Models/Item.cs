namespace FinalProject.Models
{
    public class Item
    {
        public int ItemID { get; set; }
        public string ItemName { get; set; }
        public string? AvailableQuantity { get; set; }
        public string? Description { get; set; }
        public string MeasurementUnit { get; set; }
        public double SalePrice { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public DateTime ArrivalDate { get; set; }
        public float CostPrice { get; set; }
        public int CurrentQuantity { get; set; }
    }
}
