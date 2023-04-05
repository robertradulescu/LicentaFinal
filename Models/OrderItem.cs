namespace LicentaFinal.Models
{
    public class OrderItem
    {
        public int Id { get; set; }
        public string NumeProdus { get; set; }

        public int Cantitate { get; set; }

       public double Pret { get; set; }

       public decimal ValoareStoc { get { return (decimal)(Cantitate * Pret); } }

        public decimal ValoareTotalaStoc { get; set; }
    }
}
