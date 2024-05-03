namespace FinalProject.Models
{
    public class Item
    {
        public string ItemName { get; set; }
        public string? AvailableQuantity { get; set; }
        public string Description { get; set; }
        public string MeasurementUnit { get; set; }
        public string SalePrice { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
    }
}
