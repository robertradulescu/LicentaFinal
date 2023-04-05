namespace LicentaFinal.Models
{
    public class OrderHistory
    {
        public int Id { get; set; }
        public DateTime DateChanged { get; set; }
        public int OrderItemId { get; set; }
        public OrderItem OrderItem { get; set; }
        public int OldQuantity { get; set; }
        public int NewQuantity { get; set; }
        public double OldPrice { get; set; }
        public double NewPrice { get; set; }
    }
}
