namespace FinalProject.Models
{
    public class Stock
    {
        List<Item> items { get; set;} = new List<Item>();
        public int? UpdateBy { get; set;}
        public DateTime? UpdateOn { get; set;}
        
        public int InitialQuantity { get; set;}

        
    }
}
