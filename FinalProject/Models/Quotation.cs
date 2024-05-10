namespace FinalProject.Models
{
    public class Quotation
    {
        public int quotationID { get; set; }
        public int clientID { get; set; }
        public string clientName { get; set; }
        public string itemName { get; set; }
        public int itemID { get; set; }
        public int itemQuantity { get; set; }
        public decimal totalAmount { get; set; }
        public decimal discount { get; set; }
        public decimal payableAmount { get; set; }
    }
}
