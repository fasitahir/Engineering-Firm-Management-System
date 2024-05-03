namespace FinalProject.Models
{
    public class Stock
    {
        List<Item> items { get; set;}
        public int? UpdateBy { get; set;}
        public DateTime? UpdateOn { get; set;}
        public DateTime ArrivalDate { get; set;}
        public float CostPrice { get; set;}
        public int? CurrentQuantity { get; set;}
        public int InitialQuantity { get; set;}

        
    }
}
